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
        if (dataStorage.totalPointsOverall > 6500)
        {
            GameObject.Find("BadEnding").SetActive(false);
            GameObject.Find("GoodEnding").SetActive(true);
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("TutorialGhostGoodEnding");
        }
        else
        {
            GameObject.Find("BadEnding").SetActive(true);
            GameObject.Find("GoodEnding").SetActive(false);
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("TutorialGhostBadEnding");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
