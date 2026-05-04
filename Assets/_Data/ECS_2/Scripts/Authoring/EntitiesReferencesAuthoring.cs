using Unity.Entities;
using UnityEngine;

class EntitiesReferencesAuthoring : MonoBehaviour
{
    public GameObject bulletPrefabObj;
    public GameObject zombiePrefabObj;
    public GameObject shootLightPrefabObj;

    public class Baker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.None), new EntitiesReferences
            {
                bulletPrefabEntity = GetEntity(authoring.bulletPrefabObj, TransformUsageFlags.Dynamic),
                zombiePrefabEntity = GetEntity(authoring.zombiePrefabObj, TransformUsageFlags.Dynamic),
                shootLightPrefabEntity = GetEntity(authoring.shootLightPrefabObj, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct EntitiesReferences : IComponentData
{
    public Entity bulletPrefabEntity;
    public Entity zombiePrefabEntity;
    public Entity shootLightPrefabEntity;
}
