using Unity.Entities;
using UnityEngine;

class HealthAuthoring : MonoBehaviour
{
    public float healthAmount;
    public float healthAmountMax;

    public class Baker : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Health
            {
                healthAmount = authoring.healthAmount,
                healthAmountMax = authoring.healthAmountMax,
                onHealthChanged = true
            });
        }
    }
}

public struct Health : IComponentData
{
    public float healthAmount;
    public float healthAmountMax;
    public bool onHealthChanged;
}