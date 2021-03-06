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

    // sound
    public AudioSource setDown;

    public static bool cupCanMove = true;

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

        // if holding, position = mouse position
        if (holding) transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30.0f));

        // make sure rotation = 0
        // TODO: why is this necessary? what is changing the glass' rotation when it's handed to the monster
        transform.rotation = Quaternion.identity;

        // if offscreen, delete object
        float offscreenXright = 80.0f;
        float offscreenXleft = -80.0f;
        if (transform.position.x > offscreenXright || transform.position.x < offscreenXleft)
        {
            Destroy(gameObject);
        }
    }

    public void OnMouseDown()
    {
        if (GlassMove.cupCanMove) mouseDown = true;
    }

    public void OnMouseExit()
    {
        mouseDown = false;
    }

    public void OnMouseUp()
    {
        // only let the player hold the glass if they aren't already holding an ingredient AND if the game isn't paused
        if (!PauseMenu.isPaused && equipIngredient.equippedObject == null && mouseDown && !holding && !glassFill.purchased)
        {
            holding = true;
        }
        else if (holding)
        {
            holding = false;
            setDown.Play();
        }
        mouseDown = false;
    }
}
