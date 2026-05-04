using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct HealthBarSystem : ISystem
{
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Vector3 cameraForward = Vector3.zero;
        if (Camera.main != null)
        {
            cameraForward = Camera.main.transform.forward;
        }

        foreach ((RefRW<LocalTransform> localTrans, RefRW<HealthBar> healthBar) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<HealthBar>>())
        {
            LocalTransform parentLocalTrans = SystemAPI.GetComponent<LocalTransform>(healthBar.ValueRO.healthEntity);
            if(localTrans.ValueRO.Scale == 1f)
            {
                localTrans.ValueRW.Rotation = parentLocalTrans.InverseTransformRotation(quaternion.LookRotation(cameraForward, math.up()));
            }

            Health health = SystemAPI.GetComponent<Health>(healthBar.ValueRO.healthEntity);
            if (!health.onHealthChanged) continue;

            float healthNormalized = health.healthAmount / health.healthAmountMax;
            if (healthNormalized >= 1f)
            {
                localTrans.ValueRW.Scale = 0;
            }
            else
            {
                localTrans.ValueRW.Scale = 1;
            }

            RefRW<PostTransformMatrix> barVisualTransMatrix = 
                SystemAPI.GetComponentRW<PostTransformMatrix>(healthBar.ValueRO.barVisualEntity);
            barVisualTransMatrix.ValueRW.Value = float4x4.Scale(healthNormalized, 1, 1);
            //LocalTransform barVisualLocalTrans = SystemAPI.GetComponent<LocalTransform>(healthBar.ValueRO.barVisualEntity);
            //barVisualLocalTrans.Scale = healthNormalized;
        }
    }
}
