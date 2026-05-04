using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ShootAttackAuthoring : MonoBehaviour
{
    public float timerMax;
    public float damageAmount;
    public float attackDistance;
    public Transform bulletSpawnPosTrans;

    public class Baker : Baker<ShootAttackAuthoring>
    {
        public override void Bake(ShootAttackAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ShootAttack
            {
                timerMax = authoring.timerMax,
                damageAmount = authoring.damageAmount,
                attackDistance = authoring.attackDistance,
                bulletSpawnLocalPos = authoring.bulletSpawnPosTrans.localPosition
            });
        }
    }
}

public struct ShootAttack : IComponentData
{
    public float timer;
    public float timerMax;
    public float damageAmount;
    public float attackDistance;
    public float3 bulletSpawnLocalPos;
    public OnShootEvent onShoot;

    public struct OnShootEvent
    {
        public bool isTriggered;
        public float3 shootFromPos;
    }
}