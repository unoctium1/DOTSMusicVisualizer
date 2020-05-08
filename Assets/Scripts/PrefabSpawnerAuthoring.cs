using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class PrefabSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject[] prefabs;

    public Vector3 spawnRange;

    [Tooltip("If true, spawns a fixed amount of entities. Otherwise, repeatedly spawns.")]
    public bool SpawnFixedAmount = false;
    [Tooltip("Only used if SpawnFixedAmount is false. Seconds until next spawn.")]
    public float TimeTillSpawn;

    [Tooltip("Only used if SpawnFixedAmount is true. This many prefabs will be spawned.")]
    public int NumToSpawn;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var buffer = dstManager.AddBuffer<PrefabElement>(entity);
        buffer.Capacity = prefabs.Length;
        
        foreach(GameObject go in prefabs){
            buffer.Add(new PrefabElement{
                Value = conversionSystem.GetPrimaryEntity(go)
            });
        }

        float3 spawnRangeFloat3 = new float3(spawnRange.x, spawnRange.y, spawnRange.z);
        if(SpawnFixedAmount){
            dstManager.AddComponentData(entity, new FixedPrefabSpawner(NumToSpawn, spawnRangeFloat3));
        }else{
            dstManager.AddComponentData(entity, new PrefabSpawner{
                TimeTillSpawn = TimeTillSpawn,
                TimeSinceLastSpawn = 0f,
                SpawnRange = spawnRangeFloat3
            });
        }
        //dstManager.AddComponent(entity, typeof(CopyTransformFromGameObject));
        
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(prefabs);
    }

    public void OnDrawGizmos(){
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector3.zero, spawnRange);
    }
}
