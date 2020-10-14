using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Drink
{
    // drinkName like "Bloody Mary" or "Oktoberfest"
    public string name;

    // the order of ingredients required to make this drink
    public List<string> ingredients;

    // the current color of the drink
    public Color color;

    // reference to the RecipeManager
    private RecipeManager recipeManager = null;

    public Drink()
    {
        name = "";
        ingredients = new List<string>();
        color = new Color(0.0f, 0.0f, 0.0f);
    }

    // Returns whether two drinks match
    public bool Matches(Drink otherDrink)
    {
        float maxDifference = 10.0f;
        if(Mathf.Abs(this.color.r - otherDrink.color.r) > maxDifference) return false;
        if(Mathf.Abs(this.color.g - otherDrink.color.g) > maxDifference) return false;
        if(Mathf.Abs(this.color.b - otherDrink.color.b) > maxDifference) return false;
        // for now, ignore transparency component?
        return true;
    }

    // format the drink for Debug.Log-ing
    public override string ToString()
    {
        string output = name + "; ";
        foreach(string ingredient in ingredients)
        {
            output += ingredient + ", ";
        }
        return output;
    }

    public void AddIngredient(string newIngredient) {
        // add to ingredients list
        ingredients.Add(newIngredient);

        // re-calculate color
        if(recipeManager == null) recipeManager = GameObject.FindWithTag("RecipeSheet").GetComponent<RecipeManager>();
        color = new Color(0, 0, 0);
        foreach(string ingredient in ingredients) {
            color += recipeManager.ingredientColors[ingredient];
        }
        color /= ingredients.Count;
 
        // look for a recipe that current drink conforms to, then change the name
        // bool matchesWithRecipe = false;
        // foreach(Drink recipe in recipes) {
        //     if(currentDrink.matches(recipe)) {
        //         // if found, store name of recipe
        //         currentDrink.name = recipe.name;
        //         matchesWithRecipe = true;
        //         break;
        //     }
        // }
        // // if nothing found
        // if(!matchesWithRecipe) {
        //     currentDrink.name = "";
        // }
    }

    public Color GetDisplayColor() {
        Color displayColor = color / 255.0f;
        displayColor.a *= 255.0f;
        return displayColor;
    }

    public class DrinkComp : IComparer<Drink>
    {
        // returns 0 if x and y have the same names
        public int Compare(Drink x, Drink y)
        {
            // compare the recipes' drink names
            return String.Compare(x.name, y.name);
        }
    }
}