using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ShiftTranslationWithPlayer : IComponentData
{
    public float3 offset;

    public float shiftOffset;
    
}
