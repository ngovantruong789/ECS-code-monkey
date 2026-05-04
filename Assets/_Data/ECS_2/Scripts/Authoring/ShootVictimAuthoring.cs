using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ShootVictimAuthoring : MonoBehaviour
{
    public Transform hitPosTrans;

    public class Baker : Baker<ShootVictimAuthoring>
    {
        public override void Bake(ShootVictimAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ShootVictim
            {
                hitLocalPos = authoring.hitPosTrans.localPosition,
            });
        }
    }
}

public struct ShootVictim : IComponentData
{
    public float3 hitLocalPos;
}