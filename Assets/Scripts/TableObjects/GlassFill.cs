using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassFill : MonoBehaviour
{
    private List<string> ingredients = new List<string>();
    public bool purchased = false;
    private GameObject monsterCol;

    private bool mouseDown = false;
    private bool holding = false;

    public bool full = false;

    // Data members for reading recipes
    public TextAsset recipeFile; // the csv of recipes
    public List<Recipe> recipes = new List<Recipe>();

    // The drink the monster wants
    public string drink;

    // Remove this once the data persistence is solved
    public TextBoxScript textBox = null;

    // Smiley face prefabs
    public GameObject happyFace;
    public GameObject neutralFace;
    public GameObject frownFace;

    // Reference to emotion slider
    private Slider emotionSlider;

    // sprite stuff
    private SpriteRenderer spriteRenderer;
    public Sprite emptySprite;
    public Sprite fullSprite;

    // reference to EquipIngredient
    private EquipIngredient equipIngredient;

    // Start is called before the first frame update
    void Start()
    {
        getRecipes();

        emotionSlider = GameObject.Find("EmotionSlider").GetComponent<Slider>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        equipIngredient = GameObject.FindWithTag("EquipIngredient").GetComponent<EquipIngredient>();
    }

    public void OnMouseDown()
    {
       mouseDown = true;
    }

    public void OnMouseExit()
    {
        mouseDown = false;
    }

    public void OnMouseUp()
    {
        // only let the player hold the glass if they aren't already holding an ingredient
        if (equipIngredient.equippedObject == null && mouseDown && !holding && !purchased)
        {
            holding = true;
        }
        else if (holding)
        {
            holding = false;
        }
        mouseDown = false;

        if(equipIngredient.equippedObject != null) {
            AddIngredient(equipIngredient.equippedObject);
        }
    }

    public void AddIngredient(GameObject ingredient)
    {
        // add to ingredients
        if (textBox == null) textBox = FindObjectOfType<TextBoxScript>();
        textBox.ingredients.Add(ingredient.name);

        // add to ingredients
        ingredients.Add(ingredient.name);

        // change the sprite
        full = true;
        spriteRenderer.sprite = fullSprite;
    }

    //determines what occurs when a glass comes into contact with an ingredient versus a monster, since the glass only moves when it is full, we don't need to check for that
    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Monster" && !purchased)
        {
            if (textBox == null)
                textBox = FindObjectOfType<TextBoxScript>();
            purchased = true;
            holding = false;
            gameObject.transform.parent = GameObject.FindWithTag("Monster").transform;
            monsterCol = collisionInfo.gameObject;
            List<string> ingredients = textBox.ingredients;

            // Check if drink is correct. Create a Recipe object meant just for searching the array
            Recipe drinkNamedRecipe = new Recipe();
            drinkNamedRecipe.drinkName = textBox.drink;
            this.drink = drinkNamedRecipe.drinkName;
            // Find the drink in the sorted list of drinks
            int index = recipes.BinarySearch(drinkNamedRecipe, new RecipeComp());
            if (index < 0)
            {
                Debug.Log("Recipe for " + this.drink + " not found");
            }
            else
            {
                Recipe targetDrink = recipes[index];
                bool drinkIsCorrect = targetDrink.isCorrect(ingredients);

                Debug.Log((drinkIsCorrect ? "Drink is correct" : "Drink is wrong"));

                GameObject face;
                if(drinkIsCorrect)
                {
                    // if drink is correct and timely, happy face
                    if (emotionSlider.value > 0.5f) face = Instantiate(happyFace);
                    // if drink is correct and badly timed, neutral face
                    else face = Instantiate(neutralFace);
                }
                else
                {
                    // if drink is wrong, frown face
                    face = Instantiate(frownFace);
                }
                face.transform.parent = GameObject.FindWithTag("Monster").transform;
            }
            GameObject.FindWithTag("GlassSpawner").GetComponent<GlassSpawner>().SpawnGlass();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (holding)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30.0f)) - transform.position;
            transform.Translate(point);
        }

        float offscreenX = 80.0f;
        if (transform.position.x > offscreenX)
        {
            Destroy(gameObject);
        }
    }

    public class Recipe
    {
        // drinkName like "Bloody Mary" or "Oktoberfest"
        public string drinkName;

        // The order of ingredients required to make this drink
        public List<string> ingredients;

        // Returns whether or not a drink has the right ingredients for the recipe
        public bool isCorrect(List<string> drinkIngredients)
        {
            // If the drink and the recipe have different numbers of ingredients,
            // then of course the drink has the wrong list of ingredients
            if (drinkIngredients.Count != ingredients.Count)
            {
                Debug.Log("The recipes are not equal because the number of ingredients is not the same:");
                //Debug.Log(drinkIngredients.Count);
                //Debug.Log(ingredients.Count);
                return false;
            }

            // If but one of the ingredients is off, then the drink is not correct
            for (int i = 0; i < drinkIngredients.Count; ++i)
                if (!drinkIngredients[i].Equals(ingredients[i]))
                {
                    Debug.Log("The recipes are not equal because the ingredients are different:");
                    //Debug.Log(drinkIngredients[i]);
                    //Debug.Log(ingredients[i]);
                    return false;
                }
            return true;
        }
    }

    public class RecipeComp : IComparer<Recipe>
    {
        // returns 0 if x and y have the same drink names
        public int Compare(Recipe x, Recipe y)
        {
            // compare the recipes' drink names
            string xName = x.drinkName;
            string yName = y.drinkName;
            return String.Compare(xName, yName);
        }
    }

    // Gets the recipes from a CSV
    void getRecipes()
    {
        // For each line in the CSV, set the drink name and the ingredients
        List<List<string>> csvResults = readRecipeCSV();
        for (int i = 0; i < csvResults.Count; i++)
        {
            Recipe currentRecipe = new Recipe();
            List<string> ingredients = new List<string>();
            for (int j = 0; j < csvResults[i].Count; j++)
            {
                if (j == 0)
                {
                    // Recipe name
                    currentRecipe.drinkName = csvResults[i][j];
                }
                else
                {
                    // Ingredient
                    //Debug.Log(csvResults[i][j]);
                    ingredients.Add(csvResults[i][j]);
                }
            }

            currentRecipe.ingredients = ingredients;
            recipes.Add(currentRecipe);
        }
        RecipeComp recipeComp = new RecipeComp();
        recipes.Sort(recipeComp);
    }

    private List<List<string>> readRecipeCSV()
    {
        string[] stringRecipes = recipeFile.text.Split('\n');
        // first row is the header 
        List<List<string>> recipes = new List<List<string>>();
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
            recipes.Add(recipe);
        }

        return recipes;
    }

}

