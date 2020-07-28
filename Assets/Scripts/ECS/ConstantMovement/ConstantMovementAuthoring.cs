using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class ConstantMovementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    
    public Vector3 direction;
    public float velocity;  

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Vector3 dir = direction.normalized;
        dir *= velocity;

        dstManager.AddComponentData(entity, new ConstantMovementComponent{
            movement = new float3(dir.x, dir.y, dir.z)
        });
        //dstManager.AddComponentData(entity, new CopyInitialTransformFromGameObject());
        dstManager.AddComponentData(entity, new CopyTransformToGameObject());
        
    }
}
