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

            lightColor = new Color(255.0f, 255.0f, 255.0f, 1.0f); // make them all white
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
        // For each line in the CSV, set the drink name and the ingredients
        List<List<string>> csvResults = ReadCSV(recipeFile);
        for (int i = 0; i < csvResults.Count; i++)
        {
        	Drink currentDrink = new Drink();
            bool hasTopping = false;
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
                    if(!currentDrink.AddIngredient(csvResults[i][j]) && csvResults[i][j].Contains(";"))
                    {
                        string[] toppingManip = csvResults[i][j].Split(';');
                        currentDrink.AddIngredient(toppingManip[0]);
                        currentDrink.BlendToppings();
                        currentDrink.AddIngredient(toppingManip[1]);
                        hasTopping = true;
                    }
                }
            }

            if(!hasTopping) currentDrink.BlendToppings();
            recipes.Add(currentDrink);
        }
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
