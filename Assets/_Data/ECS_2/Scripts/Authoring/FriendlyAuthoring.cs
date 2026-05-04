using Unity.Entities;
using UnityEngine;

public class FriendlyAuthoring : MonoBehaviour
{
    public class Baker : Baker<FriendlyAuthoring>
    {
        public override void Bake(FriendlyAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Friendly());
        }
    }
}

public struct Friendly : IComponentData
{
    
}
