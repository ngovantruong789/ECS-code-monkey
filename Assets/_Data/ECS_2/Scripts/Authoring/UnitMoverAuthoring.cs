using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class UnitMoverAuthoring : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public float3 targetPos;
    public bool isMoved;

    public class Baker : Baker<UnitMoverAuthoring>
    {
        public override void Bake(UnitMoverAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new UnitMover
            {
                moveSpeed = authoring.moveSpeed,
                rotationSpeed = authoring.rotationSpeed,
                targetPos = authoring.targetPos,
                isMoved = authoring.isMoved
            });
        }
    }
}

public struct UnitMover : IComponentData
{
    public float moveSpeed;
    public float rotationSpeed;
    public float3 targetPos;
    public bool isMoved;
}