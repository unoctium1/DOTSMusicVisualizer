using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class ScaleOnMusicAuthoring : AbstractMusicAuthoring
{
    public override void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        base.Convert(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new ScaleValue
        {
            Value = 1
        });

    }
}