using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{

    public enum MonsterState
    {
        slidingOn,
        center,
        slidingOff,
        offscreen,
    }

    public MonsterState state = MonsterState.slidingOn;

    private float slidingSpeed = 30.0f;
    private float currentSpeed;
    public bool readyToLeave = false;

    public string dialogueToStart = "";

    public string drinkOrder = "";

    public Monster prefab;

    public int happiness; // on a scale from -10 to 10, or whatever

    private RecipeSheet recipeSheet;
    private RecipeManager recipeManager;

    private GameObject drinkIcon;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-50, transform.position.y, 0);

        GameObject recipeSheetObject = GameObject.FindWithTag("RecipeSheet");
        recipeSheet = recipeSheetObject.GetComponent<RecipeSheet>();
        recipeManager = recipeSheetObject.GetComponent<RecipeManager>();
        drinkIcon = GameObject.FindWithTag("DrinkIcon");
    }

    //Checks if it has encountered the drink, if it has, then it is ready to leave
    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Glass")
        {
            readyToLeave = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + Time.deltaTime * currentSpeed, transform.position.y, transform.position.z);

        // slide in to the right
        if (state == MonsterState.slidingOn) currentSpeed = slidingSpeed;

        // stop in the center
        if(state == MonsterState.slidingOn && transform.position.x >= 0)
        {
            // stop moving
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            currentSpeed = 0.0f;
            state = MonsterState.center;

            // if there is a dialogue for the monster, load it
            if(dialogueToStart != "")
            {
                FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(dialogueToStart);
            }

            // show a picture of the drink they want
            drinkIcon.transform.GetChild(0).gameObject.SetActive(true);
            GameObject liquidIcon = drinkIcon.transform.GetChild(1).gameObject;
            liquidIcon.SetActive(true);
            liquidIcon.GetComponent<Image>().color = recipeManager.GetDrinkByName(drinkOrder).GetDisplayColor();

            // add recipe to the recipe sheet
            recipeSheet.AddRecipeToSheet(drinkOrder);
        }

        // slide off when ready
        if(state == MonsterState.center && readyToLeave)
        {
            state = MonsterState.slidingOff;
            // stop dialogue
            FindObjectOfType<Yarn.Unity.DialogueUI>().DialogueComplete();
            FindObjectOfType<NodeVisitedTracker>().NodeComplete(dialogueToStart);
            // hide the drink icon
            drinkIcon.transform.GetChild(0).gameObject.SetActive(false);
            drinkIcon.transform.GetChild(1).gameObject.SetActive(false);
        }
        if(state == MonsterState.slidingOff)
        {
            currentSpeed = slidingSpeed;
        }

        // set state to offscreen (ready to be despawned) if offscreen
        float offscreenX = 80.0f;
        if(state == MonsterState.slidingOff && transform.position.x > offscreenX)
        {
            state = MonsterState.offscreen;
        }
    }

    // functions for all the ways in which the player can change the monster's happiness
    
    // correctness is a float from 0.0 to 1.0, where 0.0 is totally wrong and 1.0 is totally correct
    public void GivenDrink(float correctness) {
        int change = (int)Mathf.Round(4 * (correctness - 0.5f));
        Debug.Log("correctness = " + correctness + ", change = " + change);
        happiness += change; 
        prefab.GetComponent<Monster>().happiness = happiness; // update the prefab's data
    }
}
