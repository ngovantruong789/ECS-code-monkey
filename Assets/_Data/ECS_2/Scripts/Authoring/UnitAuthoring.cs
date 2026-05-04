using Unity.Entities;
using UnityEngine;

class UnitAuthoring : MonoBehaviour
{
    public Faction faction;

    public class UnitAuthoringBaker : Baker<UnitAuthoring>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Unit
            {
                faction = authoring.faction,
            });
        }
    }
}

public struct Unit : IComponentData
{
    public Faction faction;
}
