using NUnit.Framework.Constraints;
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set; }

    public EventHandler OnSelectionAreaStart;
    public EventHandler OnSelectionAreaEnd;
    public Vector2 selectStartMousePos;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            selectStartMousePos = Mouse.current.position.ReadValue();
            //Debug.Log("Down: " + selectStartMousePos);
            OnSelectionAreaStart?.Invoke(this, EventArgs.Empty);
        }

        if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Vector2 selectEndMousePos = Mouse.current.position.ReadValue();
            //Debug.Log("Up" + selectEndMousePos);
            Vector3 mouseWorldPos = MouseWorldPosition.Instance.GetPosition();
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().Build(entityManager);
            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<Selected> selectedArray = entityQuery.ToComponentDataArray<Selected>(Allocator.Temp);
            for (int i = 0; i < entityArray.Length; i++){
                entityManager.SetComponentEnabled<Selected>(entityArray[i], false);
                Selected selected = selectedArray[i];
                selected.onDeselected = true;
                entityManager.SetComponentData(entityArray[i], selected);
            }

            Rect rect = GetSelectionAreaRect();
            float selectionAreaSize = rect.width + rect.height;
            float multipSelectionMinSize = 40f;
            if (selectionAreaSize > multipSelectionMinSize)
            {
                entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, Unit>().WithPresent<Selected>().Build(entityManager);
                entityArray = entityQuery.ToEntityArray(Allocator.Temp);
                NativeArray<LocalTransform> localTransArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
                for (int i = 0; i < localTransArray.Length; i++)
                {
                    LocalTransform localTransform = localTransArray[i];
                    Vector2 unitScreenPos = Camera.main.WorldToScreenPoint(localTransform.Position);
                    if (rect.Contains(unitScreenPos))
                    {
                        entityManager.SetComponentEnabled<Selected>(entityArray[i], true);
                        Selected selected = entityManager.GetComponentData<Selected>(entityArray[i]);
                        selected.onSelected = true;
                        entityManager.SetComponentData(entityArray[i], selected);
                    }
                }
            }
            else
            {
                entityQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                PhysicsWorldSingleton physicsWorldSingleton = entityQuery.GetSingleton<PhysicsWorldSingleton>();
                CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
                UnityEngine.Ray cameraRay = Camera.main.ScreenPointToRay(selectEndMousePos);
                RaycastInput raycastInput = new RaycastInput
                {
                    Start = cameraRay.GetPoint(0f),
                    End = cameraRay.GetPoint(9999f),
                    Filter = new CollisionFilter
                    {
                        BelongsTo = ~0u,
                        CollidesWith = 1u << GameAssets.UNIT_PLAYER,
                        GroupIndex = 0,
                    }
                };

                if(collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit))
                {
                    if (entityManager.HasComponent<Unit>(raycastHit.Entity) && entityManager.HasComponent<Selected>(raycastHit.Entity))
                    {
                        entityManager.SetComponentEnabled<Selected>(raycastHit.Entity, true);
                        Selected selected = entityManager.GetComponentData<Selected>(raycastHit.Entity);
                        selected.onSelected = true;
                        entityManager.SetComponentData(raycastHit.Entity, selected);
                    }
                }
                
            }
            

            OnSelectionAreaEnd?.Invoke(this, EventArgs.Empty);
        }

        if (Mouse.current.rightButton.isPressed)
        {
            Vector3 mouseWorldPos = MouseWorldPosition.Instance.GetPosition();
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover, Selected>().Build(entityManager);
            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<UnitMover> unitMovers = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);
            NativeArray<float3> movePosArray = GenerateMovePositionArray(mouseWorldPos, entityArray.Length);

            for (int i = 0; i < unitMovers.Length; i++)
            {
                UnitMover unitMover = unitMovers[i];
                unitMover.targetPos = movePosArray[i];
                unitMover.isMoved = true;
                unitMovers[i] = unitMover;
            }

            entityQuery.CopyFromComponentDataArray(unitMovers);
        }
    }

    public Rect GetSelectionAreaRect()
    {
        Vector2 selectionEndMousePos = Mouse.current.position.ReadValue();
        Vector2 lowerLeftCorner = new Vector2
        (
            Mathf.Min(selectStartMousePos.x, selectionEndMousePos.x),
            Mathf.Min(selectStartMousePos.y, selectionEndMousePos.y)
        );
        Vector2 upperRightCorner = new Vector2
        (
            Mathf.Max(selectStartMousePos.x, selectionEndMousePos.x),
            Mathf.Max(selectStartMousePos.y, selectionEndMousePos.y)
        );

        return new Rect(
            lowerLeftCorner.x,
            lowerLeftCorner.y,
            upperRightCorner.x - lowerLeftCorner.x,
            upperRightCorner.y - lowerLeftCorner.y
        );
    }

    //Giống cây kim đồng hồ, tất cả pos đều hướng sang phải với khoảng cách nhất định, sau đó xoay để ra vòng tròn
    private NativeArray<float3> GenerateMovePositionArray(float3 targetPos, int posCount)
    {
        NativeArray<float3> posArray = new NativeArray<float3>(posCount, Allocator.Temp);
        if(posCount == 0)
        {
            return posArray;
        }

        posArray[0] = targetPos;
        if (posCount == 1)
        {
            return posArray;
        }

        float ringSize = 2.2f;//Độ dài của vòng
        int ring = 0;//Vòng hiện tại
        int posIndex = 1;//Pos hiện tại
        while (posIndex < posCount)//If toàn bộ pos, nếu đã setup xong vị trí thì hết while
        {
            int ringPosCount = 3 + ring * 2;//Sức chứa của vòng, 1 vòng chứa bao nhiêu pos
            for (int i = 0; i < ringPosCount; i++)//For mỗi pos của vòng hiện tại
            {
                float angle = i * (math.PI2 / ringPosCount);//Lấy gốc được chia đều của vòng
                float3 ringVector = math.rotate(quaternion.RotateY(angle), new float3(ringSize * (ring + 1)));//Xoay và chỉnh khoảng cách vòng
                float3 ringPos = targetPos + new float3(ringVector.x, targetPos.y, ringVector.z);//Lấy được pos

                posArray[posIndex] = ringPos;
                posIndex++;
                if (posIndex == posCount) break;
            }

            ring++;
        }

        return posArray;
    }
}
