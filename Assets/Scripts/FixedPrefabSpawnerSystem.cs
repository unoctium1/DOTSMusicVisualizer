using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public struct FixedPrefabSpawner : IComponentData{
    public int NumPrefabsToSpawn;
    public int NumSpawned;
    public float3 SpawnRange;

    public FixedPrefabSpawner(int numToSpawn, float3 spawnRange) =>
        (NumPrefabsToSpawn, NumSpawned, SpawnRange) = (numToSpawn, 0, spawnRange);
}

public class FixedPrefabSpawnerSystem : SystemBase
{


    BeginInitializationEntityCommandBufferSystem m_Barrier;

    protected override void OnCreate(){
        base.OnCreate();
        m_Barrier = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate(){
        var ecb = m_Barrier.CreateCommandBuffer().ToConcurrent();

        Random random = new Random((uint)UnityEngine.Random.Range(1,999999));

        Entities
            .WithName("PrefabSpawnerSystem")
            .ForEach(
                (Entity e, int entityInQueryIndex, ref FixedPrefabSpawner spawn, in DynamicBuffer<PrefabElement> elements, in LocalToWorld ltw) => 
                {
                    if(spawn.NumSpawned < spawn.NumPrefabsToSpawn){
                        float3 pos = random.NextFloat3(-spawn.SpawnRange/2, spawn.SpawnRange/2);
                        Entity newEntity = ecb.Instantiate(entityInQueryIndex, elements[random.NextInt(0,elements.Length)].Value);
                        ecb.SetComponent(entityInQueryIndex, newEntity, new Translation{
                            Value = math.transform(ltw.Value, pos)
                        });
                        ecb.SetComponent(entityInQueryIndex, newEntity, new Rotation{
                            Value = random.NextQuaternionRotation()
                        });

                        spawn.NumSpawned++;
                    }else{
                        ecb.RemoveComponent<FixedPrefabSpawner>(entityInQueryIndex, e);
                    }
                }).ScheduleParallel();
        
        m_Barrier.AddJobHandleForProducer(Dependency);

    }
}
