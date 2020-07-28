using Unity.Assertions;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public abstract class AbstractMusicAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField, Tooltip("Emission is set from the global amplitude, otherwise a specific level must be assigned")]
    protected bool useAmplitude = true;
    [SerializeField, Tooltip("Level to use")]
    protected int level = 0;

    public virtual void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Assert.IsTrue(level >= 0 && level < AudioSpectrum.Instance.BandCount, "level is not valid");
        if (useAmplitude)
            dstManager.AddComponent<AmplitudeTagComponent>(entity);
        else
            dstManager.AddComponentData(entity, new LevelComponent
            {
                Value = level
            });

    }
}
