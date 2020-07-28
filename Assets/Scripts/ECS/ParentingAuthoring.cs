using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class ParentingAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
      
    public GameObject parent;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if(parent == null)
            return;

        var parentEntity = conversionSystem.GetPrimaryEntity(parent);

        dstManager.AddComponentData(entity, new Parent{
            Value = parentEntity
        });   

        dstManager.AddComponent<LocalToParent>(entity); 
        dstManager.AddComponent<CopyInitialTransformFromGameObject>(entity);
        
    }
}
