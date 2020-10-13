﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassFill : MonoBehaviour
{
    private Drink targetDrink = new Drink();
    private Drink currentDrink = new Drink();

    public bool purchased = false;

    // Data members for reading recipes
    public TextAsset recipeFile; // the csv of recipes
    public List<Drink> recipes = new List<Drink>(); // a list of all recipes

    // Smiley face prefabs
    public GameObject happyFace;
    public GameObject neutralFace;
    public GameObject frownFace;
    // Reference to emotion slider
    private Slider emotionSlider;

    // references
    private EquipIngredient equipIngredient;
    private GlassMove glassMove;

    // sprite stuff
    private SpriteRenderer spriteRenderer;
    public Sprite emptySprite;
    public Sprite oneSixthSprite;
    public Sprite twoSixthSprite;
    public Sprite threeSixthSprite;
    public Sprite fourSixthSprite;
    public Sprite fiveSixthSprite;
    public Sprite sixSixthSprite;

    // sound
    public AudioSource pourDrink;

    // Start is called before the first frame update
    void Start()
    {
        getRecipes();

        equipIngredient = GameObject.FindWithTag("EquipIngredient").GetComponent<EquipIngredient>();
        glassMove = gameObject.GetComponent<GlassMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        emotionSlider = GameObject.Find("EmotionSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        // if current monster's order has changed, re-get the target drink
        GameObject currentMonster = GameObject.FindWithTag("Monster");
        if(currentMonster != null) {
            string newDrinkName = currentMonster.GetComponent<Monster>().drinkOrder;
            if (newDrinkName.CompareTo(targetDrink.name) != 0)
            {
                Drink nameToSearch = new Drink();
                nameToSearch.name = newDrinkName;
                int index = recipes.BinarySearch(nameToSearch, new Drink.DrinkComp());
                if (index >= 0) targetDrink = recipes[index];
            }
        }
    }

    public void OnMouseUp()
    {
        if (equipIngredient.equippedObject != null)
        {
            AddIngredient(equipIngredient.equippedObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Monster" && !purchased)
        {
            // check if current drink = target drink
            bool drinkIsCorrect = (currentDrink.name.CompareTo(targetDrink.name) == 0);
            Debug.Log("currentDrink.name = " + currentDrink.name + ", targetDrink.name = " + targetDrink.name);
            Debug.Log((drinkIsCorrect ? "Drink is correct" : "Drink is wrong"));

            // show emoji face
            GameObject face;
            if (drinkIsCorrect)
            {
                // if drink is correct and timely, happy face
                if (emotionSlider.value > 0.4f) face = Instantiate(happyFace);
                // if drink is correct and badly timed, neutral face
                else face = Instantiate(neutralFace);
            }
            else face = Instantiate(frownFace);
            face.transform.parent = GameObject.FindWithTag("Monster").transform;

            // attach drink to monster so they carry it offscreen
            gameObject.transform.parent = collisionInfo.gameObject.transform;
            purchased = true;
            glassMove.holding = false;

            // spawn new glass
            GameObject.FindWithTag("GlassSpawner").GetComponent<GlassSpawner>().SpawnGlass();
        }
    }

    public void AddIngredient(GameObject ingredient)
    {
        // add to current ingredients
        currentDrink.ingredients.Add(ingredient.name);

        // look for a recipe that current drink conforms to
        bool matchesWithRecipe = false;
        foreach(Drink recipe in recipes)
        {
            if(currentDrink.matches(recipe))
            {
                // if found, store name of recipe
                currentDrink.name = recipe.name;
                matchesWithRecipe = true;
                break;
            }
        }

        // if nothing found
        if(!matchesWithRecipe)
        {
            currentDrink.name = "";
        }

        //changes the sprite of the glass to the number of ingredients
        //TODO: change the fullSprite to different sprites
        switch (currentDrink.ingredients.Count)
        {
            case 1:
                spriteRenderer.sprite = oneSixthSprite;
                break;
            case 2:
                spriteRenderer.sprite = twoSixthSprite;
                break;
            case 3:
                spriteRenderer.sprite = threeSixthSprite;
                break;
            case 4:
                spriteRenderer.sprite = fourSixthSprite;
                break;
            case 5:
                spriteRenderer.sprite = fiveSixthSprite;
                break;
            case 6:
                // This might end up being fullSprite - but in case we change from sixths to 
                // 8ths and such, I'll keep it this for now?
                spriteRenderer.sprite = sixSixthSprite;
                break;
            default:
                //alert that tells the user the glass is full
                GlassIsFullAlert();
                break;
        }

        // play pouring sound
        pourDrink.Play();
    }

    void GlassIsFullAlert()
    {
        //TODO Charlie and Helen
    }

    // Clears the drink of ingredients and resets its sprite
    public void clearIngredients()
    {
        currentDrink.ingredients = new List<string>();
        spriteRenderer.sprite = emptySprite;
    }
    

    // Gets the drink recipes from a CSV, stores in recipes List
    void getRecipes()
    {
        // For each line in the CSV, set the drink name and the ingredients
        List<List<string>> csvResults = readRecipeCSV();
        for (int i = 0; i < csvResults.Count; i++)
        {
            Drink currentDrink = new Drink();
            List<string> ingredients = new List<string>();
            for (int j = 0; j < csvResults[i].Count; j++)
            {
                if (j == 0)
                {
                    // Recipe name
                    currentDrink.name = csvResults[i][j];
                }
                else
                {
                    // Ingredient
                    ingredients.Add(csvResults[i][j]);
                }
            }

            currentDrink.ingredients = ingredients;
            recipes.Add(currentDrink);
        }
        recipes.Sort(new Drink.DrinkComp());
    }

    private List<List<string>> readRecipeCSV()
    {
        string[] stringRecipes = recipeFile.text.Split('\n');
        // first row is the header 
        List<List<string>> recipesTemp = new List<List<string>>();
        for (int i = 1; i < stringRecipes.Length; ++i)
        {
            // Get the drink name and all the ingredients. Put them into an ArrayList
            string[] individualIngredients = stringRecipes[i].Split(',');
            List<string> recipe = new List<string>();
            recipe.AddRange(individualIngredients);

            // last one is blank
            if (i == stringRecipes.Length - 1)
                recipe.RemoveAt(recipe.Count - 1);

            // put this ArrayList into recipes
            recipesTemp.Add(recipe);
        }

        return recipesTemp;
    }
    

    
}
