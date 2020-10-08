using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RecipeSheet : MonoBehaviour
{
    private List<Drink> recipesStored = new List<Drink>();
    public GlassFill glassFill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUp() {
        ShowRecipes();
    }

    public void AddRecipeToSheet(string drinkName) {
        Debug.Log("Calling AddRecipeToSheet()... ");
        glassFill = GameObject.FindWithTag("Glass").GetComponent<GlassFill>();
        List<Drink> allRecipes = glassFill.recipes;
        Drink drink = allRecipes.Find(item => item.name == drinkName);
        recipesStored.Add(drink);
    }

    void ShowRecipes() {
        foreach (Drink drink in recipesStored)
        {
            Debug.Log(drink.ToString());
        }
    }
}
