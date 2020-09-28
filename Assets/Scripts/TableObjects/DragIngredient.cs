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
    private bool colliding = true;
    private Collider2D collisionInfoCopy;

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
        colliding = false;

    }

    public void OnMouseUp()
    {
        dragging = false;

        //if we are currently colliding with something at the frame of OnMouseUp(), we enter
        if (colliding)
        {
            if (collisionInfoCopy.gameObject.tag == "Glass")
            {
                //makes sure that the glass isn't in the customer's hands (colliding errors otherwise)
                if (collisionInfoCopy.gameObject.GetComponent<GlassFill>().purchased == false && !dragging)
                {
                    GameObject tempGlass = Instantiate(fullGlass);
                    tempGlass.GetComponent<GlassFill>().full = true;
                    Destroy(collisionInfoCopy.gameObject);
                    Destroy(gameObject);
                }
            }
        }

        Destroy(gameObject);
    }

    //Detects if the ingredient comes into contact with a glass
    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        //moved original code to onMouseUp
        //We just assign collisionInfoCopy here and colliding
        collisionInfoCopy = collisionInfo;
        colliding = true;
    }

    void OnTriggerExit2D(Collider2D collisionInfo)
    {
        //adjusting data members for non-collision
        colliding = false;
        collisionInfoCopy = null;
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
