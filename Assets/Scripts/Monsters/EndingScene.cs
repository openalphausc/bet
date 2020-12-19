using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(dataStorage.allMonsterPoints);
        /*if (dataStorage.allMonsterPoints > 1750)
        {
            GameObject.Find("BadEnding").SetActive(false);
            GameObject.Find("GoodEnding").SetActive(true);
        }
        else
        {
            GameObject.Find("BadEnding").SetActive(true);
            GameObject.Find("GoodEnding").SetActive(false);
        }*/
        GameObject.Find("BadEnding").SetActive(false);
        GameObject.Find("GoodEnding").SetActive(true);
        FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("TutorialGhostGoodEnding");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
