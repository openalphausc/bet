﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RecipeManager : MonoBehaviour
{
	// csv files
    public TextAsset recipeFile;
    public TextAsset colorsFile;

    // public data
    public SortedDictionary<string, Color> ingredientColors = new SortedDictionary<string, Color>();
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

    // set all ingredients' 2D lights to the associated color
    void SetIngredientGlows() {
    	GameObject[] ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
    	foreach(GameObject ingredient in ingredients) {
    		Color lightColor = ingredientColors[ingredient.name]/255.0f;
    		ingredient.GetComponent<Light2D>().color = lightColor;
    	}
    }

    // Gets the ingredient colors from a CSV, stores in ingredientColors dictionary
    void GetColors()
    {
        // For each line in the CSV, set the drink name and the ingredients
        List<List<string>> csvResults = ReadCSV(colorsFile);
        for (int i = 0; i < csvResults.Count; i++)
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
            foreach(string ingredient in currentDrink.ingredients) {
            	currentDrink.color += ingredientColors[ingredient]/currentDrink.ingredients.Count;
            }
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
