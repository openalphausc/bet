using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeSheet : MonoBehaviour
{
    private List<Drink> recipesStored = new List<Drink>();

    public GlassFill glassFill;

    public GameObject recipeSheetWindow;
    public TextMeshProUGUI recipeText;

    private SortedDictionary<string, string> ingredientNames;

    // Start is called before the first frame update
    void Start()
    {
        recipeText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        PopulateIngredientNames();
    }

    public void OnMouseUp() {
        if(!recipeSheetWindow.activeSelf) ShowRecipes();
    }

    public void AddRecipeToSheet(string drinkName) {
        // if already stored, do nothing
        Drink alreadyContains = recipesStored.Find(item => item.name == drinkName);
        if (alreadyContains != null) return;

        // find Drink object from GlassFill and add to recipesStored
        glassFill = GameObject.FindWithTag("Glass").GetComponent<GlassFill>();
        List<Drink> allRecipes = glassFill.recipes;
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
                string ingredientCode = drink.ingredients[i];
                temp += ingredientNames[ingredientCode];
                if (i < drink.ingredients.Count - 1) temp += ", ";
            }
            temp += "\n";
        }
        recipeText.text = temp;
    }

    
    // populate the map with ingredient codes that refer to names
    private void PopulateIngredientNames()
    {
        ingredientNames = new SortedDictionary<string, string>();
        ingredientNames.Add("ginBottle", "gin");
        ingredientNames.Add("krakenTentacles", "kraken tentacles");
        ingredientNames.Add("vodkaBottle", "vodka");
        ingredientNames.Add("whiskeyBottle", "whiskey");
        ingredientNames.Add("pixieBottle", "pixie dust");
        ingredientNames.Add("angelTears", "angel tears");
        ingredientNames.Add("simpleSyrup", "simple syrup");
        ingredientNames.Add("pumpkinJuice", "pumpkin juice");
        ingredientNames.Add("vialOfBlood", "blood");
        ingredientNames.Add("giantsToe", "a giant's toe");
        ingredientNames.Add("zombieFlesh", "zombie flesh");
    }
}
