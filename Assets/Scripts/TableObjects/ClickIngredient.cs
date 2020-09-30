using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickIngredient : MonoBehaviour
{
    private EquipIngredient equipIngredient;
    
    private bool mouseDown = false;

    // Start is called before the first frame update
    void Start()
    {
        equipIngredient = GameObject.FindWithTag("EquipIngredient").GetComponent<EquipIngredient>();
    }

    void OnMouseDown()
    {
        mouseDown = true;
    }

    void OnMouseExit()
    {
        mouseDown = false;
    }

    void OnMouseUp()
    {
        if(mouseDown) {
            equipIngredient.ClickOnObject(gameObject);
            mouseDown = false;
        }
    }

}
