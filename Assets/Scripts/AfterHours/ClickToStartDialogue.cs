using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToStartDialogue : MonoBehaviour
{

    private int proximityRadius = 100;
    
    public bool clickable;
    public string dialogueNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseDown()
    {
        // Only runs if you aren't already talking to the NPC
        if (clickable)
        {
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(dialogueNode);
        }
    }

    public void SetClickable(bool value)
    {
        clickable = value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
