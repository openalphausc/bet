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

        if (MonsterSpawner.inTutorial && !clearButtonEnabled)
        {
            YarnBarTending.EnableDialogueFunctions();
            clearButtonEnabled = true;
        }
    }
}
