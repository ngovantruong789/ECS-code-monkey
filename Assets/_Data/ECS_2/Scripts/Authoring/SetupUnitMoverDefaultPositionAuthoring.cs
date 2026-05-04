using Unity.Entities;
using UnityEngine;

class SetupUnitMoverDefaultPositionAuthoring : MonoBehaviour
{
    public class Baker : Baker<SetupUnitMoverDefaultPositionAuthoring>
    {
        public override void Bake(SetupUnitMoverDefaultPositionAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new SetupUnitMoverPosition
            {
                
            });
        }
    }
}

public struct SetupUnitMoverPosition : IComponentData
{
    
}