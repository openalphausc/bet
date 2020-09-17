using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PersistentManagerScript : MonoBehaviour
{
    //Treat "Instance" as the equivalent of "player" here. Player.Buy(item) == Instance.Buy(item).
    public static PersistentManagerScript Instance { get; private set; }
    
    //This runs when the presistentManagerScript is first run, before "Start()" of all items, so that it will be non-nul
    //if called early in a scene
    private void Awake()
    {
        //If there isn't an instance yet, create one
        if (Instance == null)
        {
            Instance = this;
            //and ensure it won't destroy when you load another scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //otherwise, destroy the gameObject trying to create another persistent manager instance (one exists)
            Destroy(gameObject);
        }
    }
}
