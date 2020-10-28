using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePositionTracker : MonoBehaviour
{
    public RectTransform dialogueContainer;

    public RectTransform drinkIcon;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // positionName can be any of ("leftSeat", "rightSeat", "centerSeat") ... will add after hours positions here later
    public void SetDialoguePosition(string positionName)
    {
        float left = 0, right = 0, posY = 0;
        if (positionName == "centerSeat")
        {
            left = 744;
            right = 218;
            posY = -42;
        }
        else if (positionName == "leftSeat")
        {
            left = 366;
            right = 599;
            posY = -42;
        }
        else if (positionName == "rightSeat")
        {
            left = 599;
            right = 363;
            posY = -42;
        }

        dialogueContainer.offsetMin = new Vector2(left, dialogueContainer.offsetMin.y);
        dialogueContainer.offsetMax = new Vector2(-right, posY);
        if (drinkIcon != null)
        {
            drinkIcon.offsetMin = new Vector2(left + 287, drinkIcon.offsetMin.y);
            drinkIcon.offsetMax = new Vector2(-(right - 69), -93);
        }
    }

    public void HideDrinkIcon()
    {
        Debug.Log("Setting dialogue system to invisible.");
        if (drinkIcon != null)
        {
            // hide the drink icon
            drinkIcon.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            drinkIcon.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void SetDrinkIconColor(Color color)
    {
        if (drinkIcon != null)
        {
            // show the drink icon
            drinkIcon.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            GameObject liquidIcon = drinkIcon.gameObject.transform.GetChild(1).gameObject;
            liquidIcon.SetActive(true);
            liquidIcon.GetComponent<Image>().color = color;
        }
    }
}
