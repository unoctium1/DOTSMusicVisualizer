using System.Collections;
using System.Collections.Generic;
using Unity.Assertions;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;

public class PostProcessingMusic : MonoBehaviour
{

    Volume postProcessVolume;
    //Vignette vignetteComponent;

    [SerializeField, Range(0f, 1f)] private float maxVal = 0.8f;
    [SerializeField, Range(0f, 1f)] private float minVal = 0;
    /*
    // Start is called before the first frame update
    void Start()
    {
        if (minVal > maxVal) minVal = 0f;
        postProcessVolume = GetComponent<Volume>();
        if(!postProcessVolume.profile.TryGet<Vignette>(out vignetteComponent))
        {
            vignetteComponent = postProcessVolume.profile.Add<Vignette>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        vignetteComponent.intensity.value = Mathf.Lerp(minVal, maxVal, AudioSpectrum.Instance.AmplitudeBuffer);
    }
    */
}
