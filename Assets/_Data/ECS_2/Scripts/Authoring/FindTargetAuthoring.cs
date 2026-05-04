using Unity.Entities;
using UnityEngine;

public class FindTargetAuthoring : MonoBehaviour
{
    public Faction targetFaction;
    public float range;
    public float timer;
    public float timerMax;

    public class Baker : Baker<FindTargetAuthoring>
    {
        public override void Bake(FindTargetAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new FindTarget
            {
                targetFaction = authoring.targetFaction,
                range = authoring.range,
                timer = authoring.timer,
                timerMax = authoring.timerMax
            });
        }
    }
}

public struct FindTarget : IComponentData
{
    public Faction targetFaction;
    public float range;
    public float timer;
    public float timerMax;
}