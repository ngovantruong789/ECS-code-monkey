using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct RotatingCubeSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RotateSpeed>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        return;
        /*foreach((RefRW<LocalTransform> localTrans, RefRO<RotateSpeed> rotateSpeed) 
            in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>().WithAll<RotatingCube>())
        {
            float power = 1f;
            for(int i = 0; i <= 100000; i++)
            {
                power *= 2f;
                power /= 2f;
            }
            localTrans.ValueRW = localTrans.ValueRW.RotateY(rotateSpeed.ValueRO.value * SystemAPI.Time.DeltaTime * power);
        }*/

        RotatingCubeJob rotatingCubeJob = new RotatingCubeJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };

        rotatingCubeJob.ScheduleParallel();
    }

    [BurstCompile]
    [WithAll(typeof(RotatingCube))]
    public partial struct RotatingCubeJob : IJobEntity
    {
        public float deltaTime;

        public void Execute(ref LocalTransform localTrans, in RotateSpeed rotateSpeed)
        {
            float power = 1f;
            for (int i = 0; i <= 100000; i++)
            {
                power *= 2f;
                power /= 2f;
            }
            localTrans = localTrans.RotateY(rotateSpeed.value * deltaTime * power);
        }
    }
}
