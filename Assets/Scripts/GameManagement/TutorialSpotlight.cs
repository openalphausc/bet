using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TutorialSpotlight : MonoBehaviour
{
    public static Light2D spot;

    // Start is called before the first frame update
    void Start()
    {
        spot = GetComponent<Light2D>();
        spot.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}