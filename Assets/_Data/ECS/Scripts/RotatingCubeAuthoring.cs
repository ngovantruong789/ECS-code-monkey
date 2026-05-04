using Unity.Entities;
using UnityEngine;

public class RotatingCubeAuthoring : MonoBehaviour
{
    public class Baker : Baker<RotatingCubeAuthoring>
    {
        public override void Bake(RotatingCubeAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new RotatingCube());
        }
    }
}

public struct RotatingCube : IComponentData
{

}
