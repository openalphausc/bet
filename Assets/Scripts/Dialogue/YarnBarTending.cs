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

	public static bool readyToBlend = false;

	void Start()
	{
		if (instance == null) instance = this;
	}

    [YarnCommand("inviteToAfterHours")]
    public void StayAfter()
    {
		dataStorage.stayingMonster = gameObject.name;
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

	[YarnCommand("tutorialBlendDrink")]
	public void TutorialBlendDrink()
    {
		GlassMove.cupCanMove = true;
		YarnBarTending.DisableDialogueFunctions();
		readyToBlend = true;
	}

	public void EndTutorial()
    {
		MonsterSpawner.inTutorial = false;
		MonsterSpawner.tutorialHasRun = true;

		//this is to activate the requirement for the monster to go off screen
		GameObject.Find("Ghost").GetComponent<Monster>().leaving = true;

		GameObject.Find("CloseBarButton").GetComponent<Button>().interactable = true;
		MonsterSpawner.SkipTutorialButton.SetActive(false);
		
		// ingredients
        foreach (Transform ingredient in GameObject.Find("Ingredients").transform)
        {
            ingredient.GetComponent<HoverHighlight>().isEnabled = true; // Disable hover highlighting
            ingredient.GetComponent<ClickIngredient>().isEnabled = true; // Disable clicking
        }

		GlassMove.cupCanMove = true;
		GameObject.Find("ClearGlassButton").GetComponent<Button>().interactable = true;

		TutorialLightCues("_", "_");

		//dataStorage.stayingMonster = "Ghost";

		// toppings
		// GameObject.Find("nightmareFuel").SetActive(true);
		// GameObject.Find("goldenDust").SetActive(true);
		// GameObject.Find("mud").SetActive(true);
		// GameObject.Find("zombieFlesh").SetActive(true);

		// misc
		// GameObject.Find("Blender").SetActive(true);
	}

	[YarnCommand("endTutorial")]
	public void SkipTutorial()
	{
		MonsterSpawner.bob.readyToLeave = true;
		MonsterSpawner.timeUntilNextSpawn = 2.0f;
		EnableDialogueFunctions();
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

	[YarnCommand("goodbye")]
	public void DialogueEnd()
	{
		//this is called when the afterhours invite has concluded
		//gameObject.GetComponent<Monster>().leaving = true;
		Monster.currentlyOrderingMonster.leaving = true;
	}


	//light stuff

	[YarnCommand("tutorialLightCues")]
    public void TutorialLightCues(string firstItem, string secondItem)
    {
        TutorialSpotlight.spot1.enabled = true;
        TutorialSpotlight.spot1.pointLightOuterRadius = 10;
		if (firstItem == "toppings")
		{
			TutorialSpotlight.spot1.transform.position = new Vector3(-29.05f, -11.2f, 0);
			TutorialSpotlight.spot1.pointLightOuterRadius = 22;
			TutorialSpotlight.spot2.enabled = false;
			return;
		}
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

	[YarnCommand("changeEmotion")]
	public void changeEmotion(string emotion)
	{
		int temp = 2;
		if (emotion == "angry")
		{
			temp = 0;
		}
		if (emotion == "happy")
		{
			temp = 1;
		}
		if (emotion == "neutral")
		{
			temp = 2;
		}
		if (emotion == "sad")
		{
			temp = 3;
		}
		if (emotion == "surprised")
		{
			temp = 4;
		}
		gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<Monster>().emotions[temp];
	}
}
