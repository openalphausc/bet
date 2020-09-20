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

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.name == "EmptyGlass" && full == false)
        {
            full = true;
            Instantiate(fullGlass);
            Destroy(collisionInfo.gameObject);
            Destroy(gameObject);
        }
        if (collisionInfo.gameObject.name == "FullGlass" && full == true)
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
