using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class SpawnCubesSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<SpawnCubesConfig>();
    }

    protected override void OnUpdate()
    {
        this.Enabled = false;
        SpawnCubesConfig config = SystemAPI.GetSingleton<SpawnCubesConfig>();
        for (int i = 0; i <= config.amountToSpawn; i++)
        {
            Entity spawnedEntity = EntityManager.Instantiate(config.cubePrefabEntity);
            EntityManager.SetComponentData(spawnedEntity, new LocalTransform
            {
                Position = new float3(UnityEngine.Random.Range(-10f, 5f), 0.6f, UnityEngine.Random.Range(-4f, 7f)),
                Rotation = Quaternion.identity,
                Scale = 1
            });
        }
    }
}
