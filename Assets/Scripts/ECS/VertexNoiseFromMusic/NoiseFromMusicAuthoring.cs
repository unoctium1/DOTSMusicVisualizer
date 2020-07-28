using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class NoiseFromMusicAuthoring : AbstractMusicAuthoring
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
