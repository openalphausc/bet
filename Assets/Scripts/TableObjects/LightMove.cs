using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMove : MonoBehaviour
{
    public float speed;

    private float rightBorder = 100.0f;
    private float leftBorder = -100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0.0f, 0.0f));

        if(transform.position.x > rightBorder)
        {
            transform.position = new Vector3(leftBorder, transform.position.y, transform.position.z);
        }
    }
}
