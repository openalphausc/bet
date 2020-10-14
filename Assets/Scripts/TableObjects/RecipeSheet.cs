using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeSheet : MonoBehaviour
{
    private List<string> drinkNamesStored = new List<string>();
    private List<List<string>> recipesStored = new List<List<string>>();

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
        bool alreadyContains = drinkNamesStored.Contains(drinkName);
        if (alreadyContains) return;

        // find Drink object in recipes
        Drink drink = recipeManager.recipes.Find(item => item.name == drinkName);

        // add drink name
        drinkNamesStored.Add(drink.name);

        // use a hash set to remove duplicates and add list of ingredients
        HashSet<string> ingredientsToAdd = new HashSet<string>();
        foreach(string ingredient in drink.ingredients) {
            ingredientsToAdd.Add(ingredient);
        }
        List<string> recipe = new List<string>();
        foreach(string ingredientToAdd in ingredientsToAdd) {
            recipe.Add(ingredientToAdd);
        }
        recipesStored.Add(recipe);
    }

    void ShowRecipes() {
        recipeSheetWindow.SetActive(true);
        string temp = "";
        
        for(int i = 0; i < drinkNamesStored.Count; i++)
        {
            //Debug.Log(drink.ToString());
            temp += drinkNamesStored[i] + ": ";
            for(int j = 0; j < recipesStored[i].Count; j++)
            {
                temp += recipesStored[i][j];
                if (j < recipesStored[i].Count - 1) temp += ", ";
            }
            temp += "\n";
        }
        recipeText.text = temp;
    }
}
