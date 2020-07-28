using Unity.Entities;
using Unity.Rendering;
using System;

[Serializable]
[GenerateAuthoringComponent]
[MaterialProperty("_Scale", MaterialPropertyFormat.Float)]
public struct ScaleValue : IComponentData
{
    public float Value;
}
