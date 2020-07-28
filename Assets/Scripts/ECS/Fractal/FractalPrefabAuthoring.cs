using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct FractalColorPatternElement : IBufferElementData
{
    public float4 TipColor;
    public float4 InitColor;
    public float4 EndColor;
}

[RequiresEntityConversion]
public class FractalPrefabAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject[] prefabs;
    public ColorPatternMono[] colorPatterns;

    [System.Serializable]
    public class ColorPatternMono
    {
        public Color tipColor;
        public Color initColor;
        public Color endColor;

        public FractalColorPatternElement output
        {
            get => new FractalColorPatternElement
            {
                TipColor = ColorToFloat4(tipColor),
                InitColor = ColorToFloat4(initColor),
                EndColor = ColorToFloat4(endColor)
            };
        }

        private static float4 ColorToFloat4(Color col)
        {
            return new float4(col.r, col.g, col.b, col.a);
        }
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var fractalPrefabElements = dstManager.AddBuffer<PrefabElement>(entity);
        fractalPrefabElements.Capacity = prefabs.Length;
        foreach (GameObject go in prefabs)
        {
            fractalPrefabElements.Add(new PrefabElement
            {
                Value = conversionSystem.GetPrimaryEntity(go)
            });
        }

        var fractalColorElements = dstManager.AddBuffer<FractalColorPatternElement>(entity);
        fractalColorElements.Capacity = colorPatterns.Length;
        foreach (ColorPatternMono col in colorPatterns)
        {
            fractalColorElements.Add(col.output);
        }
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(prefabs);
    }
}
