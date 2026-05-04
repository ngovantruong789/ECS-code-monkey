using Unity.Entities;
using UnityEngine;

class TargetAuthoring : MonoBehaviour
{
    public GameObject targetObj;

    public class Baker : Baker<TargetAuthoring>
    {
        public override void Bake(TargetAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Target
            {
                targetEntity = GetEntity(authoring.targetObj, TransformUsageFlags.Dynamic)
            });
        }
    }
}

public struct Target : IComponentData
{
    public Entity targetEntity;
}