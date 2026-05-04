using Unity.Entities;
using UnityEngine;

public class SelectedAuthoring : MonoBehaviour
{
    public GameObject visualGameobject;
    public float showScale;

    public class SelectedAuthoringBaker : Baker<SelectedAuthoring>
    {
        public override void Bake(SelectedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Selected
            {
                visualGameobject = GetEntity(authoring.visualGameobject, TransformUsageFlags.Dynamic),
                showScale = authoring.showScale
            });
            SetComponentEnabled<Selected>(entity, false);
        }
    }
}

public struct Selected : IComponentData, IEnableableComponent
{
    public Entity visualGameobject;
    public float showScale;
    public bool onSelected;
    public bool onDeselected;
}