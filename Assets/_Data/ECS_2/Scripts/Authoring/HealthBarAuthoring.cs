using Unity.Entities;
using UnityEngine;

class HealthBarAuthoring : MonoBehaviour
{
    public GameObject barVisualObj;
    public GameObject healthObj;

    public class Baker : Baker<HealthBarAuthoring>
    {
        public override void Bake(HealthBarAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new HealthBar
            {
                barVisualEntity = GetEntity(authoring.barVisualObj, TransformUsageFlags.NonUniformScale),
                healthEntity = GetEntity(authoring.healthObj, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct HealthBar : IComponentData
{
    public Entity barVisualEntity;
    public Entity healthEntity;
}