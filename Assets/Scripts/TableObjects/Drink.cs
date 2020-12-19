using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Drink
{
    // drinkName like "Bloody Mary" or "Oktoberfest"
    public string name;

    // lists of current ingredients, split up by type
    public List<string> liquids;
    public List<string> toppings;

    // the current color of the drink
    public Color color;

    // reference to the RecipeManager
    private RecipeManager recipeManager = null;
    
    // static lists
    private static List<string> allLiquids;
    private static List<string> allToppings;

    public Drink()
    {
        name = "";
        liquids = new List<string>();
        toppings = new List<string>();
        color = new Color(255.0f, 255.0f, 255.0f);
        // initialize static lists
        allLiquids = new List<string>(new string[]
            {"blood", "vodka", "whiskey", "angelTears", "cornpagne", "appleJuice", "cheeryBlossom"});
        allToppings = new List<string>(new string[]
            {"zombieFlesh", "nightshade", "mud", "nightmareFuel", "goldenDust", "mushrooms"});
    }

    // returns whether two drinks match
    public bool Matches(Drink otherDrink)
    {
        float maxDifference = 25.5f; // 10% of the color range?
        // treat the 3 components of color as a vector so you can do some ez pz vector math
        Vector3 thisColor = new Vector3(color.r, color.g, color.b);
        Vector3 otherColor = new Vector3(otherDrink.color.r, otherDrink.color.g, otherDrink.color.b);
        return (Vector3.Distance(thisColor, otherColor) < maxDifference);
    }

    // returns whether two drinks have the same ingredients
    public bool HasSameIngredients(Drink otherDrink) {
        // check this drink
        
        foreach (string liquid in this.liquids) {
            
            if (!otherDrink.liquids.Contains(liquid))
            {
                //Debug.Log("Wrong liquid in this drink");
                return false;
            }
        }
        foreach(string topping in this.toppings) {
            
            if (!otherDrink.toppings.Contains(topping))
            {
               // Debug.Log("Wrong topping in this drink");
                return false;
            }
        }

        // check other drink
        foreach(string liquid in otherDrink.liquids) {
            
            if (!this.liquids.Contains(liquid))
            {
                //Debug.Log("Wrong liquid in other drink");
                return false;
            }
        }
        foreach(string topping in otherDrink.toppings) {
            
            if (!this.toppings.Contains(topping))
            {
                //Debug.Log("Wrong topping in other drink");
                return false;
            }
        }

        return true;
    }

    // format the drink for Debug.Log-ing
    public override string ToString()
    {
        string output = name + "; ";
        foreach(string liquid in liquids)
        {
            output += liquid + ", ";
        }
        output += " with toppings: ";
        foreach (string topping in toppings)
        {
            output += topping + ", ";
        }
        return output;
    }

    public bool AddIngredient(string newIngredient) {
        bool isLiquid = allLiquids.Contains(newIngredient);
        bool isTopping = allToppings.Contains(newIngredient);
        if (isLiquid == isTopping)
        {
            //Debug.Log("do we ever even hit this??");
           // Debug.Log(newIngredient);
            if (newIngredient == "") return false;
            return false;
        }

        // add to ingredients list
        if (isLiquid) liquids.Add(newIngredient);
        else toppings.Add(newIngredient);

        // re-calculate color if liquid is added
        if (isLiquid) CalculateColor();

        return isLiquid;
    }

    public void BlendToppings()
    {
        // make all toppings into liquids
        foreach (string topping in toppings)
        {
            liquids.Add(topping);
        }
        toppings.Clear();
        
        // recalculate color
        CalculateColor();
    }

    private void CalculateColor()
    {
        if(recipeManager == null) recipeManager = GameObject.FindWithTag("RecipeSheet").GetComponent<RecipeManager>();
        color = new Color(0, 0, 0);
        foreach(string liquid in liquids)
        {
            color += recipeManager.ingredientColors[liquid];
        }
        color /= liquids.Count;
    }

    public Color GetDisplayColor() {
        Color displayColor = color / 255.0f;
        displayColor.a *= 255.0f;
        return displayColor;
    }

    public int GetAmount()
    {
        return liquids.Count + toppings.Count;
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