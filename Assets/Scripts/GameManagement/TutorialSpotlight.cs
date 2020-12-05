using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TutorialSpotlight : MonoBehaviour
{
    public static Light2D spot1 = null;
    public static Light2D spot2;

    // Start is called before the first frame update
    void Start()
    {
        if (spot1 == null)
        {
            spot1 = GetComponent<Light2D>();
            spot1.enabled = false;
        }
        else
        {
            spot2 = GetComponent<Light2D>();
            spot2.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}