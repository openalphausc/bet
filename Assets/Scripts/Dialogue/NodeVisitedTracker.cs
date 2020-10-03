using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisitedTracker : MonoBehaviour
{

    // The dialogue runner that we want to attach the 'visited' function to
    private Yarn.Unity.DialogueRunner dialogueRunner;

    private HashSet<string> visitedNodes = new HashSet<string>();

    void Start()
    {
        dialogueRunner = gameObject.GetComponent<Yarn.Unity.DialogueRunner>();

        // Register a function on startup called "visited" that lets Yarn scripts query to see if a node has been run before.
        dialogueRunner.AddFunction("visited", 1, delegate (Yarn.Value[] parameters)
        {
            var nodeName = parameters[0];
            return visitedNodes.Contains(nodeName.AsString);
        });

    }

    // Called by the Dialogue Runner to notify us that a node finished running. 
    public void NodeComplete(string nodeName)
    {
        // Log that the node has been run.
        visitedNodes.Add(nodeName);
    }


    // Called by the Dialogue Runner to notify us that a new node started running. 
    public void NodeStart(string nodeName)
    {
        // Log that the node has been run.

        var tags = new List<string>(dialogueRunner.GetTagsForNode(nodeName));

        Debug.Log($"Starting the execution of node {nodeName} with {tags.Count} tags.");
    }

}
