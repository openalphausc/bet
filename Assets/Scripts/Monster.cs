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
        offscreen,
    }

    public MonsterState state = MonsterState.slidingOn;

    public float slidingSpeed = 50.0f;
    public float currentSpeed = 0.0f;

    public bool readyToLeave = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-30, 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + Time.deltaTime * currentSpeed, transform.position.y, transform.position.z);

        // slide in to the right
        if (state == MonsterState.slidingOn) currentSpeed = slidingSpeed;

        // stop in the center
        if(state == MonsterState.slidingOn && transform.position.x >= 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            currentSpeed = 0.0f;
            state = MonsterState.center;
        }

        // slide off when ready
        if (Input.GetKeyDown(KeyCode.Space)) readyToLeave = true; // temporary for testing, just set "readyToLeave" to true whenever the drink is completed/dialogue is finished
        if(state == MonsterState.center && readyToLeave)
        {
            currentSpeed = slidingSpeed;
            state = MonsterState.slidingOff;
        }

        // set state to offscreen (ready to be despawned) if offscreen
        float offscreenX = 100.0f;
        if(state == MonsterState.slidingOff && transform.position.x > offscreenX)
        {
            state = MonsterState.offscreen;
        }
    }
}
