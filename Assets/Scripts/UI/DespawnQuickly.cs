using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnQuickly : MonoBehaviour
{
    public float despawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        despawnTimer -= Time.deltaTime;

        if(despawnTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
