using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateBefore(typeof(ResetEventsSystem))]
partial struct SelectedVisualSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>().WithPresent<Selected>())
        {
            RefRW<LocalTransform> visualLocalTrans = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualGameobject);
            if(selected.ValueRO.onSelected)
            {
                visualLocalTrans.ValueRW.Scale = selected.ValueRO.showScale;
            }
            else if(selected.ValueRO.onDeselected)
            {
                visualLocalTrans.ValueRW.Scale = 0;
            }
        }
    }
}
