using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Yarn.Unity;

public class YarnBarTending : MonoBehaviour
{

    [YarnCommand("inviteToAfterHours")]
    public void StayAfter()
    {
        // The code below was to see the progression of the stayingMonsters list
        //Debug.Log("Currently invited monsters:");
        //foreach (GameObject monster in dataStorage.stayingMonsters)
        //{
        //    Debug.Log("\t" + monster);
        //}

        // Add the current monster to the stayingMonsters list
        // NOTE: Adding a clone since the original gets despawned/destroyed

       // if (gameObject.GetComponent<Monster>().happiness >= 0)
        //{
            dataStorage.stayingMonster = gameObject.name;
       /* }
        else
        {
            Debug.Log(gameObject + " does not have a positive happiness.");
        }*/
    }

}
