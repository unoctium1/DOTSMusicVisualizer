using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class RemoveColliders
{

    [MenuItem("CONTEXT/Transform/RemoveColliders")]
    static void RemoveAll(MenuCommand command){

        Transform go = (Transform)command.context;

        RemoveOnTransform(go);

    }

    static void RemoveOnTransform(Transform t){
        Collider col = t.GetComponent<Collider>();
        if(col != null){
            Object.DestroyImmediate(col);
            Undo.RecordObject(t, "Removed collider");
            
        }
        
        for(int i = 0; i < t.transform.childCount; i++){
            Transform child = t.transform.GetChild(i);
            RemoveOnTransform(child);   
        }

        PrefabUtility.RecordPrefabInstancePropertyModifications(t);
    }

}
