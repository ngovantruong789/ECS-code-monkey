using Unity.Entities;
using UnityEngine;

public class RotateSpeedAuthoring : MonoBehaviour
{
    public float value;

    private class Baker : Baker<RotateSpeedAuthoring>
    {
        public override void Bake(RotateSpeedAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new RotateSpeed
            {
                value = authoring.value
            });

        }
    }
}

public struct RotateSpeed : IComponentData
{
    public float value;
}
