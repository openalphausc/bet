using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassFill : MonoBehaviour
{
    private bool full = false;
    private ArrayList ingredients = new ArrayList();
    private bool dragging = false;
    private bool purchased = false;
    private GameObject monsterCol;
    public GameObject emptyGlass;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnMouseDown()
    {
        dragging = true;
    }

    public void OnMouseUp()
    {
        dragging = false;
    }

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ingredient")
        {
            full = true;
            ingredients.Add(collisionInfo.gameObject.name);
        }
        if (collisionInfo.gameObject.tag == "Monster")
        {
            purchased = true;
            monsterCol = collisionInfo.gameObject;
            Instantiate(emptyGlass);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (purchased == true)
        {
            gameObject.transform.position = monsterCol.gameObject.transform.position;
        }
        else if (dragging == true)
        {
            Debug.Log("Here");
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30.0f)) - transform.position;
            transform.Translate(point);
        }
    }
}
