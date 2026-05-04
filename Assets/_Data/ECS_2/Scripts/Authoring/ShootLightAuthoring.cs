using Unity.Entities;
using UnityEngine;

class ShootLightAuthoring : MonoBehaviour
{
    public float timerDespawn;

    public class Baker : Baker<ShootLightAuthoring>
    {
        public override void Bake(ShootLightAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ShootLight
            {
                timerDespawn = authoring.timerDespawn,
            });
        }
    }
}

public struct ShootLight : IComponentData
{
    public float timerDespawn;
}