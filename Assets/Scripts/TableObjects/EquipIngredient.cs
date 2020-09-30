using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipIngredient : MonoBehaviour
{

    public GameObject equippedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(equippedObject != null) {
            equippedObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30.0f));
        }
    }

    public void ClickOnObject(GameObject clicked) {
        // if equippedObject == object, put it down on an empty space
        if(equippedObject == clicked) {
            equippedObject = null;
            return;
        }

        // if there's already an object equipped, swap positions
        if(equippedObject != null) {
            Vector3 temp = clicked.transform.position;
            clicked.transform.position = equippedObject.transform.position;
            equippedObject.transform.position = temp;
        }
        // assign equippedObject
        equippedObject = clicked;
    }
}
