using Unity.Entities;
using UnityEngine;

public class ZombieAuthoring : MonoBehaviour
{
    public class Baker : Baker<ZombieAuthoring>
    {
        public override void Bake(ZombieAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Zombie());
        }
    }
}

public struct Zombie : IComponentData
{

}