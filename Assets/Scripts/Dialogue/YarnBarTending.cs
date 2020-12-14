using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using UnityEngine.Experimental.Rendering.Universal;

public class YarnBarTending : MonoBehaviour
{	

	public static bool multipleIngredients = false;
	public static YarnBarTending instance = null;

	void Start()
	{
		if (instance == null) instance = this;
	}

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
		MonsterSpawner.tutorialHasRun = true;
		
        GameObject.Find("CloseBarButton").GetComponent<Button>().interactable = true;
		MonsterSpawner.SkipTutorialButton.SetActive(false);
		
		// ingredients
        foreach (Transform ingredient in GameObject.Find("Ingredients").transform)
        {
            ingredient.GetComponent<HoverHighlight>().isEnabled = true; // Disable hover highlighting
            ingredient.GetComponent<ClickIngredient>().isEnabled = true; // Disable clicking
        }

		GlassMove.cupCanMove = true;

		// toppings
		// GameObject.Find("nightmareFuel").SetActive(true);
		// GameObject.Find("goldenDust").SetActive(true);
		// GameObject.Find("mud").SetActive(true);
		// GameObject.Find("zombieFlesh").SetActive(true);

		// misc
		// GameObject.Find("Blender").SetActive(true);
	}

	public void SkipTutorial()
	{
		MonsterSpawner.bob.readyToLeave = true;
		MonsterSpawner.timeUntilNextSpawn = 2.0f;
		EndTutorial();
	}

    

	public static void DisableDialogueFunctions() {
		GameObject.Find("Click to Continue").GetComponent<Button>().interactable = false;
		Continue.isEnabled = false;
	}

	public static void EnableDialogueFunctions()
	{
		// Start a new thread that waits for the continue button to appear (must be called on an instance)
		instance.StartCoroutine(EnableContinueButton());
		Continue.isEnabled = true;
	}


    //light stuff

    [YarnCommand("tutorialLightCues")]
    public void TutorialLightCues(string firstItem, string secondItem)
    {
        TutorialSpotlight.spot1.enabled = true;
        if (secondItem != "_")
        {
            TutorialSpotlight.spot2.enabled = true;
            TutorialSpotlight.spot2.transform.position = new Vector3(GameObject.Find(secondItem).transform.position.x, GameObject.Find(secondItem).transform.position.y, 0);
        }
        else
        {
            TutorialSpotlight.spot2.enabled = false;
        }
        if (firstItem == "ClearGlassButton")
        {
            TutorialSpotlight.spot1.transform.position = new Vector3(0, -17.2f, 0);
        }
        else if (firstItem == "_")
        {
            TutorialSpotlight.spot1.enabled = false;
        }
        else
        {
            TutorialSpotlight.spot1.transform.position = new Vector3(GameObject.Find(firstItem).transform.position.x, GameObject.Find(firstItem).transform.position.y, 0);
        }
    }

	static IEnumerator EnableContinueButton()
	{
		while (true)
		{
			var button = GameObject.Find("Click to Continue");
			if (button)
			{
				button.GetComponent<Button>().interactable = true;
				break;
			}

			yield return null;
		}
	}

}
