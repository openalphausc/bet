using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeSheet : MonoBehaviour
{
    private List<Drink> recipesStored = new List<Drink>();

    public RecipeManager recipeManager;

    public GameObject recipeSheetWindow;
    public TextMeshProUGUI recipeText;

    // Start is called before the first frame update
    void Start()
    {
        recipeText.text = "";
        recipeManager = GameObject.FindWithTag("RecipeSheet").GetComponent<RecipeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUp() {
        if(!recipeSheetWindow.activeSelf) ShowRecipes();
    }

    public void AddRecipeToSheet(string drinkName) {
        // if already stored, do nothing
        Drink alreadyContains = recipesStored.Find(item => item.name == drinkName);
        if (alreadyContains != null) return;

        // find recipe and add to recipesStored
        List<Drink> allRecipes = recipeManager.recipes;
        Drink drink = allRecipes.Find(item => item.name == drinkName);
        recipesStored.Add(drink);
    }

    void ShowRecipes() {
        recipeSheetWindow.SetActive(true);
        string temp = "";
        foreach (Drink drink in recipesStored)
        {
            //Debug.Log(drink.ToString());
            temp += drink.name + ": ";
            for(int i = 0; i < drink.ingredients.Count; i++)
            {
                temp += drink.ingredients[i];
                if (i < drink.ingredients.Count - 1) temp += ", ";
            }
            temp += "\n";
        }
        recipeText.text = temp;
    }
}
