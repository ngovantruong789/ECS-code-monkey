using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct RandomWalkingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<RandomWalking> randomWalking, RefRW<UnitMover> unitMover, RefRO<LocalTransform> localtrans) in
            SystemAPI.Query<RefRW<RandomWalking>, RefRW<UnitMover>, RefRO<LocalTransform>>())
        {
            if(math.distancesq(localtrans.ValueRO.Position, randomWalking.ValueRO.targetPos) < UnitMoverSystem.REACHED_TARGET_POSITION_DISTANCE_SQ)
            {
                Random random = randomWalking.ValueRO.random;
                float3 randomDirection = new float3(random.NextFloat(-1f, +1f), 0, random.NextFloat(-1f, +1f));
                randomDirection = math.normalize(randomDirection);

                randomWalking.ValueRW.targetPos =
                    randomWalking.ValueRO.originPos +
                    randomDirection * random.NextFloat(randomWalking.ValueRO.distanceMin, randomWalking.ValueRO.distanceMax);
                randomWalking.ValueRW.random = random;
            }
            else
            {
                unitMover.ValueRW.targetPos = randomWalking.ValueRW.targetPos;
            }
        }
    }
}
