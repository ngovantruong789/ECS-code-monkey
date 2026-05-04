using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct MoveOverrideSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<LocalTransform> localTrans, 
                RefRO<MoveOverride> moveOverride, 
                EnabledRefRW<MoveOverride> moveOverrideEnabled, 
                RefRW<UnitMover> unitMover) in 
            SystemAPI.Query<RefRO<LocalTransform>, 
                RefRO<MoveOverride>, 
                EnabledRefRW<MoveOverride>, 
                RefRW<UnitMover>>())
        {
            float3 moveOverrideTargetPos = new float3(moveOverride.ValueRO.targetPos.x, unitMover.ValueRO.targetPos.y, moveOverride.ValueRO.targetPos.z);
            if(math.distancesq(localTrans.ValueRO.Position, moveOverrideTargetPos) > UnitMoverSystem.REACHED_TARGET_POSITION_DISTANCE_SQ)
            {
                unitMover.ValueRW.targetPos = moveOverrideTargetPos;
            }
            else
            {
                moveOverrideEnabled.ValueRW = false;
            }
        }
    }
}
