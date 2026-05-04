using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct SetupUnitMoverDefaultPositionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach ((RefRO<LocalTransform> localTrans, RefRW<UnitMover> unitMover, RefRO<SetupUnitMoverPosition> setupUnitMoverDefaultPos, Entity entity) in 
        SystemAPI.Query<RefRO<LocalTransform>, RefRW<UnitMover>, RefRO<SetupUnitMoverPosition>>().WithEntityAccess())
        {
            unitMover.ValueRW.targetPos = localTrans.ValueRO.Position;
            entityCommandBuffer.RemoveComponent<SetupUnitMoverPosition>(entity);
        }
    }
}
