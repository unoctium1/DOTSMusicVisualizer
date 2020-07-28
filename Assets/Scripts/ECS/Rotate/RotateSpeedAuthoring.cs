using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class RotateSpeedAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [Range(-360f, 360f)]
    public float minSpeed = -90f;

    [Range(-360f, 360f)]
    public float maxSpeed = 90f;

    public void OnValidate(){
        Debug.Assert(maxSpeed >= minSpeed, "Max Speed must be greater than min speed!");
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        float speedAngles = UnityEngine.Random.Range(minSpeed, maxSpeed);
        float radSpeed = (speedAngles * math.PI) / 180f;   

        dstManager.AddComponentData(entity, new RotateSpeed{
            Value = radSpeed
        });
        
    }
}
