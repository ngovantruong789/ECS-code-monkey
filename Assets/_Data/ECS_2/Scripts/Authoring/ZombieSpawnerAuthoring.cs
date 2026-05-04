using Unity.Entities;
using UnityEngine;

class ZombieSpawnerAuthoring : MonoBehaviour
{
    public float timer;
    public float timerMax;
    public float randomWalkingDistanceMin;
    public float randomWalkingDistanceMax;

    public class Baker : Baker<ZombieSpawnerAuthoring>
    {
        public override void Bake(ZombieSpawnerAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ZombieSpawner
            {
                timer = authoring.timer,
                timerMax = authoring.timerMax,
                randomWalkingDistanceMax = authoring.randomWalkingDistanceMax,
                randomWalkingDistanceMin = authoring.randomWalkingDistanceMin,
            });
        }
    }
}

public struct ZombieSpawner : IComponentData
{
    public float timer;
    public float timerMax;
    public float randomWalkingDistanceMin;
    public float randomWalkingDistanceMax;
}