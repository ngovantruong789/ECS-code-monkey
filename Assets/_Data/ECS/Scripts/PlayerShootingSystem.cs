using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerShootingSystem : SystemBase
{
    public event EventHandler OnShoot;

    protected override void OnCreate()
    {
        RequireForUpdate<Player>();
    }

    protected override void OnUpdate()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();
            EntityManager.SetComponentEnabled<Stunned>(playerEntity, true);
        }

        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();
            EntityManager.SetComponentEnabled<Stunned>(playerEntity, false);
        }

        if (!Keyboard.current.spaceKey.wasPressedThisFrame) return;

        SpawnCubesConfig config = SystemAPI.GetSingleton<SpawnCubesConfig>();
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(WorldUpdateAllocator);
        foreach ((RefRW<LocalTransform> localTransform, Entity entity) in 
            SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Player>().WithDisabled<Stunned>().WithEntityAccess())
        {
            Entity spawnedEntity = entityCommandBuffer.Instantiate(config.cubePrefabEntity);
            entityCommandBuffer.SetComponent(spawnedEntity, new LocalTransform
            {
                Position = new float3(UnityEngine.Random.Range(-10f, 5f), 0.6f, UnityEngine.Random.Range(-4f, 7f)),
                Rotation = Quaternion.identity,
                Scale = 1
            });

            //OnShoot?.Invoke(entity, EventArgs.Empty);
            PlayerShootManager.Instance.PlayerShoot(localTransform.ValueRO.Position);
        }

        entityCommandBuffer.Playback(EntityManager);
    }
}
