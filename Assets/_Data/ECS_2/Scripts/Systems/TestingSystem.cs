using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct TestingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndVariableRateSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach((RefRW<Health> health, Entity entity) in
            SystemAPI.Query<RefRW<Health>>().WithEntityAccess())
        {
            if (health.ValueRO.healthAmount <= 0)
            {
                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}
