using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blender : MonoBehaviour
{
    private GameObject glass;
    private GlassMove glassMove;
    private GlassFill glassFill;

    private bool blending;
    private float blendTimer;
    private float maxBlendTime = 1.0f;

    private float glassXPosition = -4.5f;
    private float glassYPosition = -10.67f;

    public AudioSource mixSound;
    
    // Start is called before the first frame update
    void Start()
    {
        blending = false;
        blendTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        glass = GameObject.FindWithTag("Glass");
        if (glass != null)
        {
            glassMove = glass.GetComponent<GlassMove>();
            glassFill = glass.GetComponent<GlassFill>();
        }

        // check if currently blending
        if (blending)
        {
            blendTimer += Time.deltaTime;
            if (blendTimer >= maxBlendTime)
            {
                StopBlending();
            }
        }
    }

    private void StartBlending()
    {
        // start blend timer
        blending = true;
        blendTimer = 0.0f;
        
        // disappear the glass by teleporting it way offscreen lol
        glassMove.gameObject.transform.position = new Vector3(0, 10000, 0);
        
        // start sound
        mixSound.Play();
    }

    private void StopBlending()
    {
        blending = false;
        
        // teleport cup back to center
        glassMove.gameObject.transform.position = new Vector3(glassXPosition, glassYPosition, 0);
        
        // blend drink
        glassFill.currentDrink.BlendToppings();
        
        // update sprite
        glassFill.UpdateDrinkSprite(false);
        
        // stop sound
        mixSound.Stop();
        
        // if in tutorial, then reenable dialogue functions
        if (MonsterSpawner.inTutorial && YarnBarTending.readyToBlend)
        {
            YarnBarTending.EnableDialogueFunctions();
            YarnBarTending.readyToBlend = false;
            GlassMove.cupCanMove = false;
        }
    }

    public void OnMouseUp()
    {
        if (glassMove.holding && glassFill.currentDrink.GetAmount() > 0)
        {
            glassMove.holding = false;
            StartBlending();
        }
    }
}
