using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class YarnBarTending : MonoBehaviour
{	

	public static bool multipleIngredients = false;

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

	[YarnCommand("tutorialEnableClearButton")]
	public void TutorialEnableClearButton()
    {
		YarnBarTending.DisableDialogueFunctions();
		GameObject.Find("ClearGlassButton").GetComponent<Button>().interactable = true;
		
	}

	[YarnCommand("tutorialEnableIngredient")]
	public void TutorialEnableIngredient(string ingredient)
    {
		
        GameObject.Find(ingredient).GetComponent<HoverHighlight>().isEnabled = true;
        GameObject.Find(ingredient).GetComponent<ClickIngredient>().isEnabled = true;

		YarnBarTending.DisableDialogueFunctions();
		
		YarnBarTending.multipleIngredients = true;
	}

	[YarnCommand("tutorialEnableCup")]
	public void TutorialEnableCup()
    {
		GlassMove.cupCanMove = true;
		YarnBarTending.DisableDialogueFunctions();
	}

	[YarnCommand("endTutorial")]
	public void EndTutorial()
    {
		MonsterSpawner.inTutorial = false;
		
        GameObject.Find("CloseBarButton").GetComponent<Button>().interactable = true;
		
		// ingredients
        foreach (Transform ingredient in GameObject.Find("Ingredients").transform)
        {
            ingredient.GetComponent<HoverHighlight>().isEnabled = true; // Disable hover highlighting
            ingredient.GetComponent<ClickIngredient>().isEnabled = true; // Disable clicking
        }
        
        // toppings
        // GameObject.Find("nightmareFuel").SetActive(true);
        // GameObject.Find("goldenDust").SetActive(true);
        // GameObject.Find("mud").SetActive(true);
        // GameObject.Find("zombieFlesh").SetActive(true);
        
        // misc
        // GameObject.Find("Blender").SetActive(true);
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
