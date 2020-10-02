using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassMove : MonoBehaviour
{
    private bool mouseDown = false;
    public bool holding = false;

    // reference to EquipIngredient
    private EquipIngredient equipIngredient;
    private GlassFill glassFill;

    // Start is called before the first frame update
    void Start()
    {
        equipIngredient = GameObject.FindWithTag("EquipIngredient").GetComponent<EquipIngredient>();
        glassFill = gameObject.GetComponent<GlassFill>();
    }

    // Update is called once per frame
    void Update()
    {
        if (glassFill.purchased) holding = false;

        if (holding)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30.0f)) - transform.position;
            transform.Translate(point);
        }

        float offscreenX = 80.0f;
        if (transform.position.x > offscreenX)
        {
            Destroy(gameObject);
        }
    }

    public void OnMouseDown()
    {
        mouseDown = true;
    }

    public void OnMouseExit()
    {
        mouseDown = false;
    }

    public void OnMouseUp()
    {
        // only let the player hold the glass if they aren't already holding an ingredient
        if (equipIngredient.equippedObject == null && mouseDown && !holding && !glassFill.purchased)
        {
            holding = true;
        }
        else if (holding)
        {
            holding = false;
        }
        mouseDown = false;
    }
}
