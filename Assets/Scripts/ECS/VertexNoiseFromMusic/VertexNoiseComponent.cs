using System;
using Unity.Entities;
using Unity.Rendering;

[Serializable]
[GenerateAuthoringComponent]
[MaterialProperty("_Amount", MaterialPropertyFormat.Float)]
public struct VertexNoiseComponent : IComponentData
{
    public float Value;
}
