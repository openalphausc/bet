using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    private float startPosX;
    private float startPosY;
    private bool dragging = false;
    private Vector2 mousePos;

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

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //Debug.Log(Camera.main.nearClipPlane);
        if (dragging == true)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30.0f)) - transform.position;
            transform.Translate(point);
        }
    }
}
