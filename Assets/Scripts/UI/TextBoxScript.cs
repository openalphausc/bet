using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBoxScript : MonoBehaviour
{
    // textFile is the file from which we will import the drinks, separated by \n
    public TextAsset textFile;
    public static string[] drinks;

    // The monster current ordering and which drink they want
    public Monster monster;
    public string drink;
    public List<string> ingredients = new List<string>();
    private bool drinkRetrieved = false;

    public void Start()
    {
        drinks = textFile.text.Split('\n');
    }

    public void Update()
    {
        // Change in the future when there will be more than one monster on the screen
        monster = FindObjectOfType<Monster>();

        if (monster == null)
        {
            return;
        }

        // If the monster is at the center
        if (monster.state == Monster.MonsterState.center)
        {
            // If the monster's order hasn't yet been shown
            if (!drinkRetrieved)
            {
                // Create an order and display it
                string drink = getRandomDrink();
                drinkRetrieved = true;

                // the monster wants this drink
                this.drink = drink;
            }
        }
        else
        {
            // Otherwise, the monster is leaving / has left
            drinkRetrieved = false;
        }
    }

    // Gets a random drink from the imported list of drinks
    public string getRandomDrink()
    {
        int index = Random.Range(0, drinks.Length - 1);
        return drinks[index];
    }
}
