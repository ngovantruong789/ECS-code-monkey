using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial struct FindTargetSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<DistanceHit> distanceHits = new NativeList<DistanceHit>(Allocator.Temp);
        
        foreach((RefRO<LocalTransform> localTransform, RefRW<FindTarget> findTarget, RefRW<Target> target) in 
            SystemAPI.Query<RefRO<LocalTransform>, RefRW<FindTarget>, RefRW<Target>>())
        {
            findTarget.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (findTarget.ValueRO.timer > 0f) continue;

            findTarget.ValueRW.timer = findTarget.ValueRO.timerMax;
            distanceHits.Clear();
            CollisionFilter filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNIT_PLAYER,
                GroupIndex = 0
            };
            if (collisionWorld.OverlapSphere(localTransform.ValueRO.Position, findTarget.ValueRO.range, ref distanceHits, filter))
            {
                foreach (DistanceHit distanceHit in distanceHits)
                {
                    if (!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity)) continue;

                    Unit unit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if(findTarget.ValueRO.targetFaction == unit.faction)
                    {
                        target.ValueRW.targetEntity = distanceHit.Entity;
                        break;
                    }
                }
            }
        }
    }
}
