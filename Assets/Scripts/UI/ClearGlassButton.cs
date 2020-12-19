using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearGlassButton : MonoBehaviour
{

    private bool clearButtonEnabled = false;
    
    public void Clear()
    {
        GlassFill glass = FindObjectOfType<GlassFill>();
        glass.clearIngredients();
        
        foreach (GameObject t in glass.toppings) {
            Destroy(t);
        }
        glass.toppings.Clear();

        if (MonsterSpawner.inTutorial && !clearButtonEnabled)
        {
            YarnBarTending.EnableDialogueFunctions();
            clearButtonEnabled = true;
        }
    }
}
