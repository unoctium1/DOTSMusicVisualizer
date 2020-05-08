using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    [SerializeField] int radius = 8;
    [SerializeField] float speed = 0.5f;
    [SerializeField] bool forward = true;

    private float angle = 0;
    private float t = 0;

    private float y;
    private void Start()
    {
        y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (t >= 1.0f) {
            angle = 0f;
            t = 0f;
        }

        angle = forward ? Mathf.Lerp(0f, 360f, t) : Mathf.Lerp(360f, 0f, t);
        t += Time.deltaTime * speed;

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y, z);
    }
}
