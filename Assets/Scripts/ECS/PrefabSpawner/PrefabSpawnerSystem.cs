using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public struct PrefabSpawner : IComponentData{
    public float TimeTillSpawn;
    public float TimeSinceLastSpawn;
    public float3 SpawnRange;
}

public class PrefabSpawnerSystem : SystemBase
{


    BeginInitializationEntityCommandBufferSystem m_Barrier;

    protected override void OnCreate(){
        base.OnCreate();
        m_Barrier = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate(){
        float dt = Time.DeltaTime;
        var ecb = m_Barrier.CreateCommandBuffer().ToConcurrent();

        Random random = new Random((uint)UnityEngine.Random.Range(1,999999));

        Entities
            .WithName("PrefabSpawnerSystem")
            .ForEach(
                (Entity e, int entityInQueryIndex, ref PrefabSpawner spawn, in DynamicBuffer<PrefabElement> elements, in LocalToWorld ltw) => 
                {
                    spawn.TimeSinceLastSpawn += dt;
                    while(spawn.TimeSinceLastSpawn >= spawn.TimeTillSpawn){
                        float3 pos = random.NextFloat3(-spawn.SpawnRange/2, spawn.SpawnRange/2);
                        Entity newEntity = ecb.Instantiate(entityInQueryIndex, elements[random.NextInt(0,elements.Length)].Value);
                        ecb.SetComponent(entityInQueryIndex, newEntity, new Translation{
                            Value = math.transform(ltw.Value, pos)
                        });
                        ecb.SetComponent(entityInQueryIndex, newEntity, new Rotation{
                            Value = random.NextQuaternionRotation()
                        });

                        spawn.TimeSinceLastSpawn -= spawn.TimeTillSpawn;
                    }
                }).ScheduleParallel();
        
        m_Barrier.AddJobHandleForProducer(Dependency);

    }
}
