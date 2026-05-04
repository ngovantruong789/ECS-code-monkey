using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MovementAuthoring : MonoBehaviour
{
    //public float3 movementVector;
    public class Baker : Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Movement
            {
                movementVector = new float3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f))
            });
        }
    }
}

public struct Movement : IComponentData
{
    public float3 movementVector;
}
