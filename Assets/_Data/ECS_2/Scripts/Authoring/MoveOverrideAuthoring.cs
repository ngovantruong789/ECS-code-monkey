using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class MoveOverrideAuthoring : MonoBehaviour
{
    class MoveOverrideAuthoringBaker : Baker<MoveOverrideAuthoring>
    {
        public override void Bake(MoveOverrideAuthoring authoring)
        {
            Entity enitty = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(enitty, new MoveOverride
            {
                
            });
            SetComponentEnabled<MoveOverride>(enitty, false);
        }
    }
}

public struct MoveOverride : IComponentData, IEnableableComponent
{
    public float3 targetPos;
}