using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PickupGraphics : MonoBehaviour
{
    public float frequency = 1;
    public float minIntensity = 1;
    public float maxIntensity = 2;
    private float elapsed = 0;


    private Light2D light;

    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;

        float max = maxIntensity, min = minIntensity, f = frequency, theta = elapsed;
        light.intensity = (max-min)/2.0f * Mathf.Cos(theta*f) + (max-min) + .5f*min-.5f*max + min;
    }
}
