using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIngredient : MonoBehaviour
{

    public GameObject emptyGlass;
    public GameObject fullGlass;
    private bool instanceCreated = false;
    private bool dragging = false;
    private Vector2 mousePos;
    private bool full = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnMouseDown()
    {
        dragging = true;
        //Create an instance of the object to remain while you drap the ingredient
        if (!instanceCreated)
        {
            Instantiate(gameObject);
        }
        instanceCreated = true;

    }

    public void OnMouseUp()
    {
        dragging = false;
        Destroy(gameObject);
    }

    //Detects if the ingredient comes into contact with a glass
    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        //the full boolean separates what happens when the glass is full versus empty, currently, if it's empty, it just repalces it with the full sprite
        if (collisionInfo.gameObject.tag == "Glass" && full == false)
        {
            full = true;
            //makes sure that the glass isn't in the customer's hands (colliding errors otherwise)
            if(collisionInfo.gameObject.GetComponent<GlassFill>().purchased == false)
            {
                GameObject tempGlass = Instantiate(fullGlass);
                tempGlass.GetComponent<GlassFill>().full = true;
                Destroy(collisionInfo.gameObject);
                Destroy(gameObject);
            }
        }
        if (collisionInfo.gameObject.tag == "Glass" && full == true)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        //need to use ScreenToWorldPoint in order to put the mouse position into terms of scene position instead of screen pixel position
        //to get vector from x to y:
        //XY = y-pos - x-pos
        if (dragging == true)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30.0f)) - transform.position;
            transform.Translate(point);
        }
    }
}
