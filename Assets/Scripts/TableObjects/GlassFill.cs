using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassFill : MonoBehaviour
{
    private ArrayList ingredients = new ArrayList();
    private bool dragging = false;
    public bool purchased = false;
    private GameObject monsterCol;
    public GameObject emptyGlass;
    public bool full = false;

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

    //determines what occurs when a glass comes into contact with an ingredient versus a monster, since the glass only moves when it is full, we don't need to check for that
    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ingredient")
        {
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
        if (purchased)
        {
            gameObject.transform.position = monsterCol.gameObject.transform.position;
        }
        else if (dragging && full)
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
}
