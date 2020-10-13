using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Drink
{
    // drinkName like "Bloody Mary" or "Oktoberfest"
    public string name;

    // The order of ingredients required to make this drink
    public List<string> ingredients;

    public Drink()
    {
        name = "";
        ingredients = new List<string>();
    }

    // Returns whether two drinks match
    public bool matches(Drink otherDrink)
    {
        return Enumerable.SequenceEqual(this.ingredients, otherDrink.ingredients);
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

    public class DrinkComp : IComparer<Drink>
    {
        // returns 0 if x and y have the same names
        public int Compare(Drink x, Drink y)
        {
            // compare the recipes' drink names
            string xName = x.name;
            string yName = y.name;
            return String.Compare(xName, yName);
        }
    }
}