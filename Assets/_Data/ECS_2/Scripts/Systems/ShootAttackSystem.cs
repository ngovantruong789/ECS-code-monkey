using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct ShootAttackSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        foreach((RefRW<LocalTransform> localTrans, RefRW<ShootAttack> shootAtk, RefRO<Target> target, RefRW<UnitMover> unitMover) in 
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<ShootAttack>, RefRO<Target>, RefRW<UnitMover>>())
        {
            if (target.ValueRO.targetEntity == Entity.Null) continue;
            if (!SystemAPI.Exists(target.ValueRO.targetEntity)) continue;
            if (!SystemAPI.HasComponent<LocalTransform>(target.ValueRO.targetEntity)) continue;

            LocalTransform targetLocalTrans = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            shootAtk.ValueRW.timer -= SystemAPI.Time.DeltaTime;

            if (math.distance(localTrans.ValueRO.Position, targetLocalTrans.Position) > shootAtk.ValueRO.attackDistance)
            {
                unitMover.ValueRW.targetPos = targetLocalTrans.Position;
                continue;
            }
            else
            {
                unitMover.ValueRW.targetPos = localTrans.ValueRO.Position;
            }

            float3 aimDirection = math.normalize(targetLocalTrans.Position - localTrans.ValueRO.Position);
            quaternion targetRot = quaternion.LookRotation(aimDirection, math.up());
            localTrans.ValueRW.Rotation = math.slerp(localTrans.ValueRO.Rotation, targetRot, unitMover.ValueRO.rotationSpeed * SystemAPI.Time.DeltaTime);

            if (shootAtk.ValueRO.timer > 0f) continue;
            shootAtk.ValueRW.timer = shootAtk.ValueRW.timerMax;

            Entity bulletEntity = state.EntityManager.Instantiate(entitiesReferences.bulletPrefabEntity);
            float3 bulletSpawnWorldPos = localTrans.ValueRO.TransformPoint(shootAtk.ValueRO.bulletSpawnLocalPos);
            SystemAPI.SetComponent(bulletEntity, LocalTransform.FromPosition(bulletSpawnWorldPos));

            RefRW<Bullet> bullet = SystemAPI.GetComponentRW<Bullet>(bulletEntity);
            bullet.ValueRW.damageAmount = shootAtk.ValueRO.damageAmount;

            RefRW<Target> bulletTarget = SystemAPI.GetComponentRW<Target>(bulletEntity);
            bulletTarget.ValueRW.targetEntity = target.ValueRO.targetEntity;

            shootAtk.ValueRW.onShoot.isTriggered = true;
            shootAtk.ValueRW.onShoot.shootFromPos = bulletSpawnWorldPos;
        }
    }
}