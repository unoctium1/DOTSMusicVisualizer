using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Assertions;
using Unity.Rendering;
using System.Collections.Generic;

public struct FractalSpawner : IComponentData
{
    public int DepthValue;
    public float ChildScale;
    public float SpawnProbability;
    public bool isFirst;
    public int MaxDepth;
    public int NumDraws
    {
        get => FractalDepthAuthoring.NumChildren(DepthValue).Item1;
    }
}

public class FractalSpawnerSystem : SystemBase
{
    private static readonly float3[] directions =
    {
        math.up(),
        new float3(1f,0f,0f),
        new float3(-1f,0f,0f),
        new float3(0f,0f,1f),
        new float3(0f,0f,-1f),
        new float3(0f,-1f,0f)
    };
    private static readonly quaternion[] orientations =
    {
        quaternion.identity,
        quaternion.EulerZXY(0f, 0f, -(math.PI/2)),
        quaternion.EulerZXY(0f, 0f, (math.PI/2)),
        quaternion.EulerZXY((math.PI/2), 0f, 0f),
        quaternion.EulerZXY(-(math.PI/2), 0f, 0f),
        quaternion.EulerZXY(math.PI, 0f, 0f),
    };

    private NativeArray<float3> childDirections;
    private NativeArray<quaternion> childRotations;
    private EntityQuery query;

    BeginInitializationEntityCommandBufferSystem entityCommandBufferSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        childDirections = new NativeArray<float3>(directions, Allocator.Persistent);
        childRotations = new NativeArray<quaternion>(orientations, Allocator.Persistent);
        entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnDestroy()
    {
        childRotations.Dispose();
        childDirections.Dispose();
    }

    protected override void OnUpdate()
    {
        var ecb = entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

        var dir = childDirections;
        var rotations = childRotations;

            int dataCount = query.CalculateEntityCount();
            NativeArray<uint> seeds
                = new NativeArray<uint>(dataCount, Allocator.TempJob);

            for (int i = 0; i < dataCount; i++)
            {
                seeds[i] = (uint)UnityEngine.Random.Range(1, uint.MaxValue-1);
            }

            Entities
                .WithStoreEntityQueryInField(ref query)
                .WithName("FractalSpawnerSystem")
                .ForEach((Entity entity, 
                    int entityInQueryIndex, 
                    ref MaterialColor col, 
                    in DynamicBuffer<PrefabElement> prefabBuffer, 
                    in DynamicBuffer<FractalColorPatternElement> colorBuffer, 
                    in FractalSpawner spawn,
                    in LocalToWorld ltw) =>
                {
                    var random = new Random(seeds[entityInQueryIndex]);
                    if (spawn.DepthValue > 0 && (spawn.DepthValue == spawn.MaxDepth || random.NextFloat(0.0f, 1.0f) < spawn.SpawnProbability))
                    {
                        FractalSpawner newSpawn = spawn;
                        newSpawn.isFirst = false;
                        newSpawn.DepthValue--;
                        bool setScale = HasComponent<NonUniformScale>(entity);
                        float3 scale = new float3(1f);
                        if(setScale){
                            scale = GetComponent<NonUniformScale>(entity).Value;
                        }

                        for (int i = 0; i < (spawn.isFirst ? 6 : 5); i++)
                        {
                            int maxPrefab = prefabBuffer.Length;
                            Entity toSpawn = prefabBuffer[random.NextInt(0, maxPrefab)].Value;
                            var newFractal = ecb.Instantiate(entityInQueryIndex, toSpawn);
                            float3 position = dir[i];
                            quaternion rot = rotations[i];
                            position *= (0.5f + (0.5f * spawn.ChildScale));
                            ecb.AddComponent(entityInQueryIndex, newFractal, newSpawn);
                            ecb.AddComponent<MaterialColor>(entityInQueryIndex, newFractal);
                            var pBuffer = ecb.AddBuffer<PrefabElement>(entityInQueryIndex, newFractal);
                            pBuffer.CopyFrom(prefabBuffer);
                            var cBuffer = ecb.AddBuffer<FractalColorPatternElement>(entityInQueryIndex, newFractal);
                            cBuffer.CopyFrom(colorBuffer);
                            ecb.AddComponent(entityInQueryIndex, newFractal, new Parent
                            {
                                Value = entity
                            });
                            ecb.AddComponent(entityInQueryIndex, newFractal, new LocalToParent
                            {
                                Value = new float4x4()
                            });
                            ecb.AddComponent(entityInQueryIndex, newFractal, new Scale
                            {
                                Value = spawn.ChildScale
                            });

                            ecb.AddComponent(entityInQueryIndex, newFractal, new Translation
                            {
                                Value = position
                            });
                            ecb.AddComponent(entityInQueryIndex, newFractal, new Rotation
                            {
                                Value = rot
                            });
                            ecb.AddComponent(entityInQueryIndex, newFractal, new RotateSpeed
                            {
                                Value = random.NextFloat(-0.5f, 0.5f)
                            });

                        }
                    }
                    FractalColorPatternElement p = colorBuffer[random.NextInt(0, colorBuffer.Length)];
                    if(spawn.DepthValue == 0)
                    {
                        col = new MaterialColor
                        {
                            Value = p.TipColor
                        };
                    }
                    else
                    {
                        col = new MaterialColor
                        {
                            Value = math.lerp(p.InitColor, p.EndColor, (spawn.MaxDepth - spawn.DepthValue) / (spawn.MaxDepth - 1f))
                        };
                    }
                    ecb.RemoveComponent<FractalSpawner>(entityInQueryIndex, entity);

                })
                .WithDeallocateOnJobCompletion(seeds)
                .ScheduleParallel();
            entityCommandBufferSystem.AddJobHandleForProducer(Dependency);

    }
}
