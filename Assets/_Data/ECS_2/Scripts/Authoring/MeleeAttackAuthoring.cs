using Unity.Entities;
using UnityEngine;

public class MeleeAttackAuthoring : MonoBehaviour
{
    public float recoveryAttackTimer;
    public float recoveryAttackTimerMax;
    public float damageAmount;
    public float colliderSize;

    public class Baker : Baker<MeleeAttackAuthoring>
    {
        public override void Bake(MeleeAttackAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new MeleeAttack
            {
                recoveryAttackTimer = authoring.recoveryAttackTimer,
                recoveryAttackTimerMax = authoring.recoveryAttackTimerMax,
                damageAmount = authoring.damageAmount,
                colliderSize = authoring.colliderSize
            });
        }
    }
}

public struct MeleeAttack : IComponentData
{
    public float recoveryAttackTimer;
    public float recoveryAttackTimerMax;
    public float damageAmount;
    public float colliderSize;
}
