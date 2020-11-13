using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class YarnBarTending : MonoBehaviour
{	

    [YarnCommand("inviteToAfterHours")]
    public void StayAfter()
    {

        // Add the current monster to the stayingMonsters list
        // NOTE: Adding a clone since the original gets despawned/destroyed

        // if (gameObject.GetComponent<Monster>().happiness >= 0)
        // {
            dataStorage.stayingMonster = gameObject.name;
            Debug.Log(gameObject.name + " invited to after hours.");
        // }
        // else
        // {
        //     Debug.Log(gameObject + " does not have a positive happiness.");
        // }
        
    }

    [YarnCommand("tutorialUseIngredient")]
    public void TutorialUseIngredient(string ingredient)
    {
        GameObject.Find(ingredient).GetComponent<HoverHighlight>().isEnabled = true;
        GameObject.Find(ingredient).GetComponent<ClickIngredient>().isEnabled = true;
		
		YarnBarTending.DisableDialogueFunctions();
    }

	public static void DisableDialogueFunctions() {
		GameObject.Find("Click to Continue").GetComponent<Button>().interactable = false;
		Continue.isEnabled = false;
	}

	public static void EnableDialogueFunctions() {
		GameObject.Find("Click to Continue").GetComponent<Button>().interactable = true;
		Continue.isEnabled = true;
	}

}
