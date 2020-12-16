using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassFill : MonoBehaviour
{
    private Drink targetDrink = new Drink();
    [NonSerializedAttribute] public Drink currentDrink = new Drink();

    public bool purchased = false;

    // Smiley face prefabs
    public GameObject happyFace;
    public GameObject neutralFace;
    public GameObject frownFace;

    // references
    private EquipIngredient equipIngredient;
    private GlassMove glassMove;

    // sprite stuff
    public SpriteRenderer liquidSprite;
    public Sprite oneSixthSprite;
    public Sprite twoSixthSprite;
    public Sprite threeSixthSprite;
    public Sprite fourSixthSprite;
    public Sprite fiveSixthSprite;
    public Sprite sixSixthSprite;
    private Color targetColor;
    private float maxMixTime = 1.0f;

    // sounds
    public AudioSource addLiquid;
    public AudioSource addTopping;
    public AudioSource wellDone; // perfect drink
    public AudioSource notBad; // color is off, but correct ingredients
    public AudioSource ew; // wrong

    private RecipeManager recipeManager;

	public List<GameObject> toppings = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        equipIngredient = GameObject.FindWithTag("EquipIngredient").GetComponent<EquipIngredient>();
        glassMove = gameObject.GetComponent<GlassMove>();
        recipeManager = GameObject.FindWithTag("RecipeSheet").GetComponent<RecipeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // if current monster's order has changed, re-get the target drink
        Monster currentMonster = Monster.currentlyOrderingMonster;
        if(currentMonster != null) {
            string newDrinkName = currentMonster.drinkOrder;
            if (newDrinkName.CompareTo(targetDrink.name) != 0)
            {
                targetDrink = recipeManager.GetDrinkByName(newDrinkName);
            }
        }
    }

    IEnumerator LerpDrinkColor()
    {
        Color startColor = liquidSprite.color;
        float mixTimer = 0;

        while (mixTimer < maxMixTime)
        {
            liquidSprite.color = Color.Lerp(startColor, targetColor, mixTimer / maxMixTime);
            mixTimer += Time.deltaTime;

            yield return null;
        }

        liquidSprite.color = targetColor;
    }

    public void OnMouseUp()
    {
        if (equipIngredient.equippedObject != null)
        {
            AddIngredient(equipIngredient.equippedObject);
            
            if (MonsterSpawner.inTutorial && (!YarnBarTending.multipleIngredients || currentDrink.GetAmount() == 4))
            {
                equipIngredient.equippedObject.GetComponent<HoverHighlight>().isEnabled = false;
                equipIngredient.equippedObject.GetComponent<ClickIngredient>().isEnabled = false;
                equipIngredient.ClickOnObject(equipIngredient.equippedObject);
                YarnBarTending.EnableDialogueFunctions();
				YarnBarTending.multipleIngredients = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Monster" && !purchased && Monster.currentlyOrderingMonster != null && collisionInfo.gameObject.GetComponent<Monster>() == Monster.currentlyOrderingMonster)
        {
            Debug.Log("currentDrink.color = " + currentDrink.color.ToString() + ", targetDrink.color = " + targetDrink.color.ToString());

            // check if current drink = target drink
            GameObject face;
            if (currentDrink.Matches(targetDrink))
            {
                // if drink matches color, happy face
                face = Instantiate(happyFace);
                wellDone.Play();
                Debug.Log("Drink matches color");
            }
            else {
                // if doesn't match color, but has same ingredients, neutral face
                if(currentDrink.HasSameIngredients(targetDrink)) {
                    face = Instantiate(neutralFace);
                    notBad.Play();
                    Debug.Log("Drink matches ingredients, not color.");
                }
                // if totally wrong, frown face
                else {
                    face = Instantiate(frownFace);
                    ew.Play();
                    Debug.Log("Drink is wrong.");
                }
            }
            face.transform.parent = Monster.currentlyOrderingMonster.transform;
            
            // update monster's happiness
            Monster.currentlyOrderingMonster.GivenDrink(currentDrink.Matches(targetDrink) ? 1 : 0);

            // attach drink to monster so they carry it offscreen
            gameObject.transform.parent = collisionInfo.gameObject.transform;
            purchased = true;
            glassMove.holding = false;

            // spawn new glass
            GameObject.FindWithTag("GlassSpawner").GetComponent<GlassSpawner>().SpawnGlass();
        }
    }

    void AddIngredient(GameObject ingredient)
    {
        // if 6 ingredients already in the drink, don't do anything
        if(currentDrink.GetAmount() >= 6) {
            GlassIsFullAlert(); //alert that tells the user the glass is full
            return;
        }

        // add ingredient to current drink
        bool isLiquid = currentDrink.AddIngredient(ingredient.name);

        // update drink sprite
        UpdateDrinkSprite();

        // play pouring sound
        if (isLiquid) addLiquid.Play();
        else addTopping.Play();
    }

    public void UpdateDrinkSprite(bool lerp = true)
    {
        int total = currentDrink.liquids.Count + currentDrink.toppings.Count;
        //changes the sprite of the glass to the number of ingredients
        //TODO: change the fullSprite to different sprites
        switch (total)
        {
            case 0:
                break;
            case 1:
                liquidSprite.sprite = oneSixthSprite;
                break;
            case 2:
                liquidSprite.sprite = twoSixthSprite;
                break;
            case 3:
                liquidSprite.sprite = threeSixthSprite;
                break;
            case 4:
                liquidSprite.sprite = fourSixthSprite;
                break;
            case 5:
                liquidSprite.sprite = fiveSixthSprite;
                break;
            case 6:
                // This might end up being fullSprite - but in case we change from sixths to 
                // 8ths and such, I'll keep it this for now?
                liquidSprite.sprite = sixSixthSprite;
                break;
            default:
                break;
        }

        // set color of liquid to appropriate color
        if (!lerp)
        {
            liquidSprite.color = currentDrink.GetDisplayColor();
        }
        else
        {
            if (currentDrink.liquids.Count > 1)
            {
                targetColor = currentDrink.GetDisplayColor();
                StartCoroutine(LerpDrinkColor());
            }
            else if (currentDrink.liquids.Count == 1)
            {
                liquidSprite.color = currentDrink.GetDisplayColor();
            }
        }
        
        // Remove old toppings
		foreach (GameObject t in toppings) {
			Destroy(t);
		}
		toppings.Clear();
		
		// Variables for adding toppings
        string layerName = "", topping = "";
		GameObject layer = null;
		Vector3 position = gameObject.transform.position, offset = new Vector3(0,0,0);
		float layerHeight = gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 5.5f,
			  minY = gameObject.GetComponent<BoxCollider2D>().bounds.min.y + layerHeight / 2.0f;

        for (int curLevel = currentDrink.liquids.Count; curLevel < total; curLevel++)
        {
			// Reset position to cup position and get topping name from current drink
			position = gameObject.transform.position;
			topping = currentDrink.toppings[curLevel - currentDrink.liquids.Count];

			// Adjust variables per topping type
            if (topping == "goldenDust") { layerName = "OA GoldDustTop0"; offset.y = 3.5f; offset.x = 0f; }
            else if (topping == "mud") { layerName = "OA_GraveyardMudTop0"; offset.y = 3.5f; offset.x = 0.2f; }
			else if (topping == "zombieFlesh") { layerName = "OA ZombieFleshTop1"; offset.y = 1f; offset.x = 0f; }
			else if (topping == "nightmareFuel") { layerName = "OA NightmareTop0"; offset.y = 2f; offset.x = 0f; }
			
			// Calculate position to place object
			position.y = minY + curLevel * layerHeight;
			position += offset;

			// Instantiate topping at position and add it to the toppings list
			layer = Instantiate(Resources.Load<GameObject>("Prefabs/CupToppings/" + layerName), position, Quaternion.identity);
			layer.transform.SetParent(gameObject.transform);
			toppings.Add(layer);
		}
    }

    void GlassIsFullAlert()
    {
        Debug.Log("Cup is full");
    }

    // Clears the drink of ingredients and resets its sprite
    public void clearIngredients()
    {
        currentDrink = new Drink();
        liquidSprite.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
    }
}
