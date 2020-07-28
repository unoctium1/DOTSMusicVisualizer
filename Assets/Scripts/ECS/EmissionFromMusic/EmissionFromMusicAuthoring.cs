using Unity.Assertions;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
public class EmissionFromMusicAuthoring : AbstractMusicAuthoring
{
    public override void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        base.Convert(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new EmissionComponent
        {
            Value = 0
        });

    }
}
