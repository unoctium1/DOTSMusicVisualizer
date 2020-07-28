using System;
using Unity.Entities;
using Unity.Rendering;

[Serializable]
[GenerateAuthoringComponent]
[MaterialProperty("_EmissionStrength", MaterialPropertyFormat.Float)]
public struct EmissionComponent : IComponentData
{
    public float Value;
}
