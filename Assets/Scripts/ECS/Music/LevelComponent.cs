using System;
using Unity.Entities;

[Serializable, GenerateAuthoringComponent]
public struct LevelComponent : IComponentData
{
    public int Value;
}
