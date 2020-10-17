using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFadeUp : MonoBehaviour
{
    private Light2D light;

    private float maxIntensity; // set by each individual light

    private float fadeUpTime = 3.0f;
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
        maxIntensity = light.intensity;
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < fadeUpTime)
        {
            timer += Time.deltaTime;
            // intensity of light fades up, proportional with the time it takes
            light.intensity = Mathf.Lerp(0, maxIntensity, timer / fadeUpTime);
        }
    }
}
