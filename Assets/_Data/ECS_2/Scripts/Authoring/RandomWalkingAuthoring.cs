using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class RandomWalkingAuthoring : MonoBehaviour
{
    public float3 targetPos;
    public float3 originPos;
    public float distanceMin;
    public float distanceMax;
    public uint randomSeed;

    public class Baker : Baker<RandomWalkingAuthoring>
    {
        public override void Bake(RandomWalkingAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new RandomWalking
            {
                targetPos = authoring.targetPos,
                originPos = authoring.originPos,
                distanceMin = authoring.distanceMin,
                distanceMax = authoring.distanceMax,
                random = new Unity.Mathematics.Random(authoring.randomSeed)
            });
        }
    }
}

public struct RandomWalking : IComponentData
{
    public float3 targetPos;
    public float3 originPos;
    public float distanceMin;
    public float distanceMax;
    public Unity.Mathematics.Random random;
}