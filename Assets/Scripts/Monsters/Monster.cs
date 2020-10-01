﻿using System.Collections;
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

    private float slidingSpeed = 30.0f;
    private float currentSpeed;

    public bool readyToLeave = false;

    public string dialogueToStart = "";

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-50, 0, 0);
    }

    //Checks if it has encoutnered the drink, if it has, then it is ready to leave
    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Glass")
        {
            readyToLeave = true;
        }
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

            // if there is a dialogue for the monster, load it
            if(dialogueToStart != "")
            {
                FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(dialogueToStart);
            }
        }

        // slide off when ready
        if(state == MonsterState.center && readyToLeave)
        {
            state = MonsterState.slidingOff;
        }
        if(state == MonsterState.slidingOff)
        {
            currentSpeed = slidingSpeed;
        }

        // set state to offscreen (ready to be despawned) if offscreen
        float offscreenX = 80.0f;
        if(state == MonsterState.slidingOff && transform.position.x > offscreenX)
        {
            state = MonsterState.offscreen;
        }
    }
}
