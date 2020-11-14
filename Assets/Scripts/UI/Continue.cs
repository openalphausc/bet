using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continue : MonoBehaviour
{
    public Yarn.Unity.DialogueUI dialogueUI;

    public static bool isEnabled = true;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
             if (Continue.isEnabled) dialogueUI.MarkLineComplete();
        }
    }
}
