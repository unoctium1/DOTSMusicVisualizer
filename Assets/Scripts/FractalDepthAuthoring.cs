using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;

[RequiresEntityConversion]
public class FractalDepthAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{

    [Range(0,6)]
    public int DepthValue;
    [Range(0f,1f)]
    public float ChildScale;
    [Range(0f,1f)]
    public float SpawnProbability;

    public (int,bool) NumDrawCalls
    {
        get => NumChildren(DepthValue);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var fractalSpawn = new FractalSpawner
        {
            DepthValue = DepthValue,
            ChildScale = ChildScale,
            SpawnProbability = SpawnProbability,
            MaxDepth = DepthValue,
            isFirst = true
        };
        dstManager.AddComponent(entity, typeof(MaterialColor));
        dstManager.AddComponentData(entity, fractalSpawn);
    }

    public static (int,bool) NumChildren(int depth)
    {
        if (depth == 1)
        {
            return (6,true);
        }
        else
        {
            int value;
            bool valcheck = true;
            try
            {
                if (NumChildren(depth - 1).Item2)
                {
                    value = checked((5 * NumChildren(depth - 1).Item1) + 1);
                }
                else
                {
                    value = 0;
                    valcheck = false;
                }
            }
            catch (System.OverflowException)
            {
                value = 0;
                valcheck = false;
            }
            return (value, valcheck);
            
        }
    }

}
