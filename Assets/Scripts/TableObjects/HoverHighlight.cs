using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class HoverHighlight : MonoBehaviour
{
    private EquipIngredient equipIngredient;
    private Light2D light;

    public bool isEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        equipIngredient = GameObject.FindWithTag("EquipIngredient").GetComponent<EquipIngredient>();
        light = GetComponent<Light2D>();
    }

    void OnMouseOver()
    {
        if (equipIngredient.equippedObject != gameObject && !PauseMenu.isPaused && isEnabled) light.enabled = true;
    }

    void OnMouseExit()
    {
        light.enabled = false;
    }

    void OnMouseUp()
    {
        light.enabled = false;
    }
}
