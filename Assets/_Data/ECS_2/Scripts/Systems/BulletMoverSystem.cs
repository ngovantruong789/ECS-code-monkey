using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct BulletMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRW<LocalTransform> localTrans, RefRO<Bullet> bullet, RefRO<Target> target, Entity entity) in 
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<Bullet>, RefRO<Target>>().WithEntityAccess())
        {
            if (target.ValueRO.targetEntity == Entity.Null)
            {
                entityCommandBuffer.DestroyEntity(entity);
                continue;
            }

            LocalTransform targetLocalTrans = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            ShootVictim shootVictim = SystemAPI.GetComponent<ShootVictim>(target.ValueRO.targetEntity);
            float3 targetPos = targetLocalTrans.TransformPoint(shootVictim.hitLocalPos);

            float distanceBeforeSq = math.distancesq(localTrans.ValueRO.Position, targetPos);
            float3 moveDirection = math.normalize(targetPos - localTrans.ValueRW.Position);

            localTrans.ValueRW.Position += moveDirection * bullet.ValueRO.speed * SystemAPI.Time.DeltaTime;
            float distanceAfterSq = math.distancesq(localTrans.ValueRO.Position, targetPos);

            if(distanceAfterSq > distanceBeforeSq)
            {
                localTrans.ValueRW.Position = targetPos;
            }

            if (math.distancesq(localTrans.ValueRO.Position, targetPos) <= 0.2f)
            {
                RefRW<Health> health = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                health.ValueRW.healthAmount -= bullet.ValueRO.damageAmount;
                health.ValueRW.onHealthChanged = true;
                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}
