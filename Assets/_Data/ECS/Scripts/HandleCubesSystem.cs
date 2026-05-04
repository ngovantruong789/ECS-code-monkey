using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct HandleCubesSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach((RefRW<LocalTransform> localTransform, RefRO<RotateSpeed> rotateSpeed, RefRO<Movement> Movement) in 
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>, RefRO<Movement>>().WithAll<RotatingCube>())
        {
            localTransform.ValueRW = localTransform.ValueRO.RotateY(rotateSpeed.ValueRO.value * SystemAPI.Time.DeltaTime);
            localTransform.ValueRW = localTransform.ValueRO.Translate(Movement.ValueRO.movementVector * SystemAPI.Time.DeltaTime);
        }
    }
}
