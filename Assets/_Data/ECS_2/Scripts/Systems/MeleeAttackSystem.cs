using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct MeleeAttackSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<RaycastHit> raycastHits = new NativeList<RaycastHit>(Allocator.Temp);

        foreach ((RefRW<LocalTransform> localTrans, RefRW<MeleeAttack> meleeAtk, RefRW<Target> target, RefRW<UnitMover> unitMover) in 
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<MeleeAttack>, RefRW<Target>, RefRW<UnitMover>>())
        {
            if (target.ValueRO.targetEntity == Entity.Null) continue;
            if (!SystemAPI.Exists(target.ValueRO.targetEntity)) continue;
            if (!SystemAPI.HasComponent<LocalTransform>(target.ValueRO.targetEntity)) continue;

            LocalTransform targetLocalTrans = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            float meleeAtkDistanceSq = 2f;
            bool isCloseEnoughToAttack = math.distancesq(localTrans.ValueRO.Position, targetLocalTrans.Position) <= meleeAtkDistanceSq;
            bool isTouchingTarget = false;

            if (!isCloseEnoughToAttack)
            {
                float3 dirToTarget = math.normalize(targetLocalTrans.Position - localTrans.ValueRO.Position);
                float distanceExtraToTestRaycast = 0.4f;

                RaycastInput raycastInput = new RaycastInput
                {
                    Start = localTrans.ValueRO.Position,
                    End = localTrans.ValueRO.Position + dirToTarget * (meleeAtk.ValueRO.colliderSize + distanceExtraToTestRaycast),
                    Filter = CollisionFilter.Default
                };
                raycastHits.Clear();
                if(collisionWorld.CastRay(raycastInput, ref raycastHits))
                {
                    foreach(RaycastHit raycastHit in raycastHits)
                    {
                        if (raycastHit.Entity == target.ValueRO.targetEntity)
                        {
                            isTouchingTarget = true;
                        }
                    }
                }

            }

            if(!isCloseEnoughToAttack && !isTouchingTarget)
            {
                unitMover.ValueRW.targetPos = targetLocalTrans.Position;
                continue;
            }

            unitMover.ValueRW.targetPos = localTrans.ValueRO.Position;
            meleeAtk.ValueRW.recoveryAttackTimer -= SystemAPI.Time.DeltaTime;
            if (meleeAtk.ValueRO.recoveryAttackTimer >= 0) continue;

            RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
            targetHealth.ValueRW.healthAmount -= meleeAtk.ValueRO.damageAmount;
            targetHealth.ValueRW.onHealthChanged = true;
            meleeAtk.ValueRW.recoveryAttackTimer = meleeAtk.ValueRO.recoveryAttackTimerMax;
        }
    }
}
