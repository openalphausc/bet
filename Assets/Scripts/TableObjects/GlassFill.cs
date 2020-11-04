﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassFill : MonoBehaviour
{
    private Drink targetDrink = new Drink();
    [NonSerializedAttribute] public Drink currentDrink = new Drink();

    public bool purchased = false;

    // Smiley face prefabs
    public GameObject happyFace;
    public GameObject neutralFace;
    public GameObject frownFace;

    // references
    private EquipIngredient equipIngredient;
    private GlassMove glassMove;

    // sprite stuff
    public SpriteRenderer liquidSprite;
    public Sprite oneSixthSprite;
    public Sprite twoSixthSprite;
    public Sprite threeSixthSprite;
    public Sprite fourSixthSprite;
    public Sprite fiveSixthSprite;
    public Sprite sixSixthSprite;
    private Color targetColor;
    private float maxMixTime = 1.0f;

    // sound
    public AudioSource pourDrink;

    private RecipeManager recipeManager;

    // Start is called before the first frame update
    void Start()
    {
        equipIngredient = GameObject.FindWithTag("EquipIngredient").GetComponent<EquipIngredient>();
        glassMove = gameObject.GetComponent<GlassMove>();
        recipeManager = GameObject.FindWithTag("RecipeSheet").GetComponent<RecipeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // if current monster's order has changed, re-get the target drink
        Monster currentMonster = Monster.currentlyOrderingMonster;
        if(currentMonster != null) {
            string newDrinkName = currentMonster.drinkOrder;
            if (newDrinkName.CompareTo(targetDrink.name) != 0)
            {
                targetDrink = recipeManager.GetDrinkByName(newDrinkName);
            }
        }
    }

    IEnumerator LerpDrinkColor()
    {
        Color startColor = liquidSprite.color;
        float mixTimer = 0;

        while (mixTimer < maxMixTime)
        {
            liquidSprite.color = Color.Lerp(startColor, targetColor, mixTimer / maxMixTime);
            mixTimer += Time.deltaTime;

            yield return null;
        }

        liquidSprite.color = targetColor;
    }

    public void OnMouseUp()
    {
        if (equipIngredient.equippedObject != null)
        {
            AddIngredient(equipIngredient.equippedObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Monster" && !purchased && Monster.currentlyOrderingMonster != null && collisionInfo.gameObject.GetComponent<Monster>() == Monster.currentlyOrderingMonster)
        {
            Debug.Log("currentDrink.color = " + currentDrink.color.ToString() + ", targetDrink.color = " + targetDrink.color.ToString());

            // check if current drink = target drink
            GameObject face;
            if (currentDrink.Matches(targetDrink))
            {
                // if drink matches color, happy face
                face = Instantiate(happyFace);
                Debug.Log("Drink matches color");
            }
            else {
                // if doesn't match color, but has same ingredients, neutral face
                if(currentDrink.HasSameIngredients(targetDrink)) {
                    face = Instantiate(neutralFace);
                    Debug.Log("Drink matches ingredients, not color.");
                }
                // if totally wrong, frown face
                else {
                    face = Instantiate(frownFace);
                    Debug.Log("Drink is wrong.");
                }
            }
            face.transform.parent = Monster.currentlyOrderingMonster.transform;
            
            // update monster's happiness
            Monster.currentlyOrderingMonster.GivenDrink(currentDrink.Matches(targetDrink) ? 1 : 0);

            // attach drink to monster so they carry it offscreen
            gameObject.transform.parent = collisionInfo.gameObject.transform;
            purchased = true;
            glassMove.holding = false;

            // spawn new glass
            GameObject.FindWithTag("GlassSpawner").GetComponent<GlassSpawner>().SpawnGlass();
        }
    }

    void AddIngredient(GameObject ingredient)
    {
        // if 6 ingredients already in the drink, don't do anything
        if(currentDrink.GetAmount() >= 6) {
            GlassIsFullAlert(); //alert that tells the user the glass is full
            return;
        }

        // add ingredient to current drink
        currentDrink.AddIngredient(ingredient.name);

        // update drink sprite
        UpdateDrinkSprite();

        // play pouring sound
        pourDrink.Play();
    }

    public void UpdateDrinkSprite()
    {
        //changes the sprite of the glass to the number of ingredients
        //TODO: change the fullSprite to different sprites
        switch (currentDrink.liquids.Count)
        {
            case 0:
                break;
            case 1:
                liquidSprite.sprite = oneSixthSprite;
                break;
            case 2:
                liquidSprite.sprite = twoSixthSprite;
                break;
            case 3:
                liquidSprite.sprite = threeSixthSprite;
                break;
            case 4:
                liquidSprite.sprite = fourSixthSprite;
                break;
            case 5:
                liquidSprite.sprite = fiveSixthSprite;
                break;
            case 6:
                // This might end up being fullSprite - but in case we change from sixths to 
                // 8ths and such, I'll keep it this for now?
                liquidSprite.sprite = sixSixthSprite;
                break;
            default:
                break;
        }

        // set color of liquid to appropriate color
        if (currentDrink.liquids.Count > 1)
        {
            targetColor = currentDrink.GetDisplayColor();
            StartCoroutine(LerpDrinkColor());
        }
        else if (currentDrink.liquids.Count == 1)
        {
            liquidSprite.color = currentDrink.GetDisplayColor();
        }
    }

    void GlassIsFullAlert()
    {
        //TODO Charlie and Helen
    }

    // Clears the drink of ingredients and resets its sprite
    public void clearIngredients()
    {
        currentDrink = new Drink();
        liquidSprite.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
    }
}
