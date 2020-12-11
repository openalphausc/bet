using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

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

	public TextAsset ordersFile;
    [NonSerializedAttribute] public string drinkOrder = "";
	private string orderNotes;

    public Monster prefab;

    public int happiness; // on a scale from -10 to 10, or whatever

    private RecipeSheet recipeSheet;
    private RecipeManager recipeManager;

    private GameObject drinkIcon;

    [NonSerializedAttribute] public Boolean inAfterHours = false;

    // members for moving to seats
    public Seat seat = null; // the seat the monster is occupying / will occupy
    private Vector3 entrance = new Vector3(0,0,0); // chooses a side from which the monster will enter
    private Vector3 exit = new Vector3(0,0,0); // will be the opposite of entrance
    private const float entranceExitX = 70.0f; // indicates how far to the side the monsters will spawn
    private Boolean alreadyClickedOn = false;
    [NonSerializedAttribute] public Boolean alreadyOrdered = false;
    [NonSerializedAttribute] public static Boolean currentlyOrdering = false; // only one monster can order at once.
    [NonSerializedAttribute] public static Monster currentlyOrderingMonster = null; // to access the currently ordering monster,
                                                                                    // do Monster.currentlyOrderingMonster
    private float transparency = 1.0f;
    private float decreaseTransparencyRate;

    private float seatTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!inAfterHours)
        {
            GetOrdersFromFile();
            entrance = GetRandomSide();
            transform.position = entrance;
            exit = new Vector3(-2 * transform.position.x, transform.position.y, transform.position.z);

            GameObject recipeSheetObject = GameObject.FindWithTag("RecipeSheet");
            recipeSheet = recipeSheetObject.GetComponent<RecipeSheet>();
            recipeManager = recipeSheetObject.GetComponent<RecipeManager>();
            drinkIcon = GameObject.FindWithTag("DrinkIcon");
        }
    }

    //Checks if it has encountered the drink, if it has, then it is ready to leave
    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        // check if the monster is moving. if it's moving, then don't allow it to take the glass
        if (currentSpeed != 0 || state != MonsterState.center || !alreadyClickedOn)
            return;
        if (collisionInfo.gameObject.tag == "Glass")
        {
            readyToLeave = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!inAfterHours)
        {
            // slide in to correct seat
            if (state == MonsterState.slidingOn) SlideTo(seat.seatLocation);

            // governs how large the snapping to the bar seat will be
            int epsilon = 1;
            // stop at the bar seat, with a little bit of leeway (equal to 2 * epsilon)
            if (state == MonsterState.slidingOn && transform.position.x >= seat.seatLocation.x - epsilon && transform.position.x <= seat.seatLocation.x + epsilon)
            {
                // stop moving
                transform.position = new Vector3(seat.seatLocation.x, transform.position.y, transform.position.z);
                currentSpeed = 0.0f;
                state = MonsterState.center;
            }

            // Timer update
            if (state == MonsterState.center && seatTimer < 45.0f)
            {
                seatTimer += Time.deltaTime;
                // Debug.Log(seatTimer);

                if (seatTimer >= 45.0f) { happiness -= 1; Debug.Log("decreased happiness"); }
            }

            // slide off when ready
            if (state == MonsterState.center && readyToLeave && !MonsterSpawner.inTutorial)
            {
                Monster.currentlyOrdering = false;
                Monster.currentlyOrderingMonster = null;
                state = MonsterState.slidingOff;
				recipeSheet.ClearOrderNotes(); // clear order notes
                seat.SetOccupancy(false);
                // math
                decreaseTransparencyRate = 2 * slidingSpeed / (exit.x - transform.position.x);
                if (decreaseTransparencyRate < 0)
                    decreaseTransparencyRate = -decreaseTransparencyRate;
                // if an exit isn't already set, set an exit
                if (exit.x == 0 && exit.y == 0 && exit.z == 0)
                {
                    exit = GetRandomSide();
                    exit.x = exit.x * 2;
                }
                // stop dialogue
                FindObjectOfType<Yarn.Unity.DialogueUI>().DialogueComplete();
                FindObjectOfType<NodeVisitedTracker>().NodeComplete(dialogueToStart);
                // hide the drink icon
                GameObject.Find("Dialogue System").GetComponent<DialoguePositionTracker>().HideDialogueSystem();
            }

            if (state == MonsterState.slidingOff)
            {
                //Debug.Log("Sliding to exit");
                SlideTo(exit);
				GameObject.Find("CloseBarButton").GetComponent<Button>().interactable = true;
            }

            // set state to offscreen (ready to be despawned) if offscreen
            float offscreenXRight = 80.0f;
            float offscreenXLeft = -80.0f;
            if (state == MonsterState.slidingOff && (transform.position.x > offscreenXRight || transform.position.x < offscreenXLeft))
            {
                state = MonsterState.offscreen;
            }
        }
    }

    // functions for all the ways in which the player can change the monster's happiness
    
    // correctness is a float from 0.0 to 1.0, where 0.0 is totally wrong and 1.0 is totally correct
    public void GivenDrink(float correctness) {
        int change = (int)Mathf.Round(4 * (correctness - 0.5f));
        Debug.Log("correctness = " + correctness + ", change = " + change);
        happiness += change; 
        prefab.GetComponent<Monster>().happiness = happiness; // update the prefab's data

		// hide the drink icon
		GameObject.Find("Dialogue System").GetComponent<DialoguePositionTracker>().HideDialogueSystem();

		if (MonsterSpawner.inTutorial) {
			YarnBarTending.EnableDialogueFunctions();
		}
    }

    public void OnMouseDown()
    {
        if (alreadyClickedOn || Monster.currentlyOrdering) return;
        alreadyClickedOn = true;
        Monster.currentlyOrdering = true;
        Monster.currentlyOrderingMonster = this;
        FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(dialogueToStart);
        
        // remove hover highlight
        Destroy(GetComponent<HoverHighlight>());
        Destroy(GetComponent<Light2D>());

		// activate dialogue system
		string currentSeatName = "";
		if(seat.seatLocation.x < 0) currentSeatName = "leftSeat";
		else if(seat.seatLocation.x > 0) currentSeatName = "rightSeat";
		else if(seat.seatLocation.x == 0) currentSeatName = "centerSeat";
		DialoguePositionTracker dialogueSystem = GameObject.Find("Dialogue System").GetComponent<DialoguePositionTracker>();
		dialogueSystem.SetDialoguePosition(currentSeatName);
		dialogueSystem.SetDrinkIconColor(recipeManager.GetDrinkByName(drinkOrder).GetDisplayColor());

		// update order notes
		recipeSheet.AddOrderNotes(drinkOrder, orderNotes);

        // Increase happiness if clicked within first 15 s of sitting down
        if (seatTimer <= 15.0f) { happiness += 1; Debug.Log("Increased happiness"); }

		GameObject.Find("CloseBarButton").GetComponent<Button>().interactable = false;
    }

    // Slides the monster towards a location
    void SlideTo(Vector3 loc)
    {
        // if the monster is to the left of the target, set the speed to the right. otherwise, set the speed to the left
        if (transform.position.x < loc.x)
            currentSpeed = slidingSpeed;
        else
            currentSpeed = -slidingSpeed;
        transform.position = new Vector3(transform.position.x + Time.deltaTime * currentSpeed, transform.position.y, transform.position.z);

        // if exiting, then decrease opacity
        if (state == MonsterState.slidingOff)
        {
            transparency -= Time.deltaTime * decreaseTransparencyRate;

            // set transparency
            Color color = gameObject.GetComponent<SpriteRenderer>().color;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, transparency);
        }
    }

    // Returns either the left side or the right side randomly
    Vector3 GetRandomSide()
    {
        int randomIndex = UnityEngine.Random.Range(0, 2);
        if (randomIndex == 1)
            return new Vector3(entranceExitX, transform.position.y, 0);
        else
            return new Vector3(-entranceExitX, transform.position.y, 0);
    }

	void GetOrdersFromFile() {
		string[] lines = ordersFile.text.Split('\n');
        // first row is the header 
        List<List<string>> allOrders = new List<List<string>>();
        for (int i = 1; i < lines.Length; ++i)
        {
            string[] elements = lines[i].Split(',');
            List<string> order = new List<string>();
            for (int j = 0; j < elements.Length; ++j)
            {
                order.Add(elements[j].Trim());
            }

            // last one is blank
            if (i == lines.Length - 1)
                order.RemoveAt(order.Count - 1);

            // put this ArrayList into recipes
            allOrders.Add(order);
        }

        // string temp = "";
        // foreach(List<string> order in allOrders) {
        //     foreach(string element in order) {
        //         temp += element + " ";
        //     }
        //     temp += "\n";
        // }
        // Debug.Log(temp);

        drinkOrder = allOrders[0][0];
        orderNotes = allOrders[0][1];
	}
}
