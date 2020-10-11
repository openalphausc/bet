using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearGlassButton : MonoBehaviour
{
    public void Clear()
    {
        GlassFill glass = FindObjectOfType<GlassFill>();
        glass.clearIngredients();
    }
}
