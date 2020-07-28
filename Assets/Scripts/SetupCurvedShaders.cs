using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCurvedShaders : MonoBehaviour
{

    [Range(0f, 0.001f)]
    public float curvatureVal = 0.0003f;
    public bool applyCurvature = true;

    [SerializeField] bool applyOnMusic = false;
    [SerializeField] float maxCurvature = 0.001f;
    [SerializeField] float minCurvature = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Shader.SetGlobalFloat("_Curvature", curvatureVal);
        Shader.SetGlobalFloat("_ApplyCurvature", applyCurvature || applyOnMusic ? 1f : 0f);
    }

    void OnValidate(){
        Shader.SetGlobalFloat("_Curvature", curvatureVal);
        Shader.SetGlobalFloat("_ApplyCurvature", (applyCurvature ? 1f : 0f));
    }

    private void Update()
    {
        if (applyOnMusic)
        {
            float amp = AudioSpectrum.Instance.AmplitudeBuffer;
            Shader.SetGlobalFloat("_Curvature", Mathf.Lerp(minCurvature, maxCurvature, amp));
        }
    }




}
