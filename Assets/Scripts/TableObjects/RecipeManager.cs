using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RecipeManager : MonoBehaviour
{
	// csv files
    public TextAsset recipeFile;
    public TextAsset colorsFile;

    // public data
    public Dictionary<string, Color> ingredientColors = new Dictionary<string, Color>();
    public List<Drink> recipes = new List<Drink>();

    // Start is called before the first frame update
    void Start()
    {
    	GetData();
    }

    void GetData() {
    	GetColors();
    	GetRecipes();
    	SetIngredientGlows();
    }

    public Drink GetDrinkByName(string drinkName) {
    	Drink nameToSearch = new Drink();
        nameToSearch.name = drinkName;
        int index = recipes.BinarySearch(nameToSearch, new Drink.DrinkComp());
        // foreach(Drink drink in recipes) {
        //     Debug.Log(drink.name);
        // }
        return recipes[index];
    }

    // set all ingredients' 2D lights to the associated color
    void SetIngredientGlows() {
    	GameObject[] ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
    	foreach(GameObject ingredient in ingredients) {
    		Color lightColor = ingredientColors[ingredient.name];
            // zombie flesh has special dark green glow because black glow = no light
            if(ingredient.name == "zombie flesh") {
                lightColor = new Color(8.0f, 145.0f, 0.0f, 1.0f);
            }
            lightColor /= 255.0f;
    		ingredient.GetComponent<Light2D>().color = lightColor;
    	}
    }

    // Gets the ingredient colors from a CSV, stores in ingredientColors dictionary
    void GetColors()
    {
        // For each line in the CSV, set the drink name and the ingredients
        List<List<string>> csvResults = ReadCSV(colorsFile);
        for (int i = 0; i < csvResults.Count - 1; i++)
        {
            Drink currentDrink = new Drink();
            List<string> ingredients = new List<string>();
            string ingredientName = csvResults[i][0];
            Color ingredientColor = new Color(float.Parse(csvResults[i][1]), float.Parse(csvResults[i][2]), float.Parse(csvResults[i][3]));

            ingredientColors.Add(ingredientName, ingredientColor);
        }
    }

    // Gets the drink recipes from a CSV, stores in recipes List
    void GetRecipes()
    {
        // get csv data
        string[] stringRecipes = recipeFile.text.Split('\n');
        
        // iterate through each line, char by char
        for (int i = 1; i < stringRecipes.Length; ++i)
        {
            bool addingToName = true;
            bool addingToLiquids = true;
            string currentIngredient = "";

            Drink drink = new Drink();
            for (int c = 0; c < stringRecipes[i].Length; c++)
            {
                char currentChar = stringRecipes[i][c];

                // if character is comma (,) add to temp ingredients list
                if (currentChar == ',')
                {
                    if (addingToName)
                    {
                        drink.name = currentIngredient;
                        addingToName = false;
                    }
                    else
                    {
                        drink.AddIngredient(currentIngredient);
                    }
                    
                    currentIngredient = "";
                }
                // if character is semicolon (;) add temp list to liquids then start adding to toppings
                else if (currentChar == ';')
                {
                    drink.AddIngredient(currentIngredient);
                    currentIngredient = "";
                    drink.BlendToppings();
                    addingToLiquids = false;
                }
                // if character is letter, add it to string
                else
                {
                    currentIngredient += currentChar;
                }
            }

            // add last ingredient to drink
            // Debug.Log(currentIngredient);
            if (!string.IsNullOrEmpty(currentIngredient))
            {
                drink.AddIngredient(currentIngredient);
                if (addingToLiquids)
                {
                    drink.BlendToppings();
                }
            }

            // add to recipes
            recipes.Add(drink);

            Debug.Log(drink);
        }
        // sort recipes list
        recipes.Sort(new Drink.DrinkComp());
    }

    private List<List<string>> ReadCSV(TextAsset file)
    {
        string[] stringRecipes = file.text.Split('\n');
        // first row is the header 
        List<List<string>> recipesTemp = new List<List<string>>();
        for (int i = 1; i < stringRecipes.Length; ++i)
        {
            // Get the drink name and all the ingredients. Put them into an ArrayList
            string[] individualIngredients = stringRecipes[i].Split(',');
            List<string> recipe = new List<string>();
            for (int j = 0; j < individualIngredients.Length; ++j)
            {
                recipe.Add(individualIngredients[j].Trim());
            }

            // last one is blank
            if (i == stringRecipes.Length - 1)
                recipe.RemoveAt(recipe.Count - 1);

            // put this ArrayList into recipes
            recipesTemp.Add(recipe);
        }

        return recipesTemp;
    }
}
