using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeSheet : MonoBehaviour
{
    private string currentDrinkName;
    private string currentNotes;

    public GameObject recipeSheetWindow;
    public TextMeshProUGUI recipeText;

    // Start is called before the first frame update
    void Start()
    {
        currentDrinkName = "";
        currentNotes = "";
        recipeText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUp() {
        if(!recipeSheetWindow.activeSelf) ShowOrder();
    }

    public void AddOrderNotes(string drinkName, string orderNotes)
    {
        currentDrinkName = drinkName;
        currentNotes = orderNotes;
    }

    public void ClearOrderNotes()
    {
        currentDrinkName = "";
        currentNotes = "";
    }

    void ShowOrder() {
        recipeSheetWindow.SetActive(true);
        if (currentDrinkName == "")
        {
            recipeText.text = "";
        }
        else
        {
            recipeText.text = currentDrinkName + ": " + currentNotes;
        }
    }
}
