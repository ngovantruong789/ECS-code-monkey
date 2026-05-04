using Unity.Burst;
using Unity.Entities;

partial struct ShootLightDestroySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach((RefRW<ShootLight> shootLight, Entity entity) in SystemAPI.Query<RefRW<ShootLight>>().WithEntityAccess())
        {
            shootLight.ValueRW.timerDespawn -= SystemAPI.Time.DeltaTime;
            if (shootLight.ValueRO.timerDespawn > 0) continue;

            entityCommandBuffer.DestroyEntity(entity);
        }
    }
}
