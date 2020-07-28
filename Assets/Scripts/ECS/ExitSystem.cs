using System.Collections;
using Unity.Entities;
using UnityEngine;

public class ExitSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){

#if !UNITY_EDITOR && UNITY_STANDALONE
            Application.Quit();
#elif UNITY_EDITOR
            Debug.Log("Application quit");
#endif

        }
    }
}
