using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

partial struct UnitMoverSystem : ISystem
{
    public const float REACHED_TARGET_POSITION_DISTANCE_SQ = 2f;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        /*foreach((RefRW<LocalTransform> localTransform, RefRO<UnitMover> unitMover, RefRW<PhysicsVelocity> physVel) in 
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>())
        {
            float3 moveDirection = unitMover.ValueRO.targetPos - localTransform.ValueRO.Position;
            if (math.lengthsq(new float3(moveDirection.x, 0, moveDirection.z)) <= 0.0001f) return;

            moveDirection = math.normalize(moveDirection);
            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, 
                                                        quaternion.LookRotation(moveDirection, math.up()), 
                                                        SystemAPI.Time.DeltaTime * unitMover.ValueRO.rotationSpeed);
            physVel.ValueRW.Linear = moveDirection * unitMover.ValueRO.moveSpeed;
            physVel.ValueRW.Angular = float3.zero;
        }*/

        UnitMoverJob unitMoverJob = new UnitMoverJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        unitMoverJob.Schedule();
    }
}

[BurstCompile]
public partial struct UnitMoverJob : IJobEntity
{
    public float deltaTime;

    public void Execute(ref LocalTransform localTransform, in UnitMover unitMover, ref PhysicsVelocity physicsVel)
    {
        float3 moveDirection = unitMover.targetPos - localTransform.Position;
        moveDirection = new float3(moveDirection.x, 0, moveDirection.z);
        if (math.lengthsq(moveDirection) <= UnitMoverSystem.REACHED_TARGET_POSITION_DISTANCE_SQ)
        {
            physicsVel.Linear = float3.zero;
            physicsVel.Angular = float3.zero;
            return;
        }

        moveDirection = math.normalize(moveDirection);
        localTransform.Rotation = math.slerp(localTransform.Rotation,
                                                    quaternion.LookRotation(moveDirection, math.up()),
                                                    deltaTime * unitMover.rotationSpeed);
        physicsVel.Linear = moveDirection * unitMover.moveSpeed;
        physicsVel.Angular = float3.zero;
    }
}