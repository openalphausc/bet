using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public enum MonsterState
    {
        slidingOn,
        center,
        slidingOff,
        despawned,
    }

    public MonsterState state = MonsterState.slidingOn;

    public float speed = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-30, 0, 5);

        Debug.Log("monster is spawned");
    }

    // Update is called once per frame
    void Update()
    {
        // slide in to the right
        if(state == MonsterState.slidingOn) transform.position = new Vector3(transform.position.x + Time.deltaTime * speed, transform.position.y, transform.position.z);
    }
}
