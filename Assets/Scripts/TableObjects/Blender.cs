using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blender : MonoBehaviour
{
    private GlassMove glassMove;

    private bool blending;
    
    // Start is called before the first frame update
    void Start()
    {
        blending = false;
    }

    // Update is called once per frame
    void Update()
    {
        glassMove = GameObject.FindWithTag("Glass").GetComponent<GlassMove>();
    }

    public void OnMouseUp()
    {
        if (glassMove.holding)
        {
            glassMove.holding = false;
            // disappear the glass
            blending = true;
            Debug.Log("Start blending!");
        }
    }
}
