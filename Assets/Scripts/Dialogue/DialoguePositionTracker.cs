using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class DialoguePositionTracker : MonoBehaviour
{
    public RectTransform dialogueContainer;

    public RectTransform drinkIcon;

    public Light2D backgroundLight;
    public Light2D leftMonsterLight;
    public Light2D rightMonsterLight;
    public Light2D centerMonsterLight;

    private float maxBackgroundIntensity;
    private float originalMonsterLightIntensity;
    
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
        maxBackgroundIntensity = backgroundLight.intensity;
        backgroundLight.intensity = 0.05f;
        float soloIntensity = 0.7f;
        if (positionName == "centerSeat")
        {
            left = 767;
            right = 60;
            posY = -42;
            leftMonsterLight.enabled = false;
            rightMonsterLight.enabled = false;
            centerMonsterLight.enabled = true;
            originalMonsterLightIntensity = centerMonsterLight.intensity;
            centerMonsterLight.intensity = soloIntensity;
        }
        else if (positionName == "leftSeat")
        {
            left = 366;
            right = 417;
            posY = -42;
            leftMonsterLight.enabled = true;
            rightMonsterLight.enabled = false;
            centerMonsterLight.enabled = false;
            originalMonsterLightIntensity = leftMonsterLight.intensity;
            leftMonsterLight.intensity = soloIntensity;
        }
        else if (positionName == "rightSeat")
        {
            left = 417;
            right = 363;
            posY = -42;
            leftMonsterLight.enabled = false;
            rightMonsterLight.enabled = true;
            centerMonsterLight.enabled = false;
            originalMonsterLightIntensity = rightMonsterLight.intensity;
            rightMonsterLight.intensity = soloIntensity;
        }

        dialogueContainer.offsetMin = new Vector2(left, dialogueContainer.offsetMin.y);
        dialogueContainer.offsetMax = new Vector2(-right, posY);
        if (drinkIcon != null)
        {
            drinkIcon.offsetMin = new Vector2(744 + 287, drinkIcon.offsetMin.y);
            drinkIcon.offsetMax = new Vector2(-(218 - 69), -93);
        }
    }

    public void HideDialogueSystem()
    {
        if (drinkIcon != null)
        {
            // hide the drink icon
            drinkIcon.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (leftMonsterLight != null)
        {
            backgroundLight.intensity = maxBackgroundIntensity;
            leftMonsterLight.enabled = true;
            leftMonsterLight.intensity = originalMonsterLightIntensity;
            rightMonsterLight.enabled = true;
            rightMonsterLight.intensity = originalMonsterLightIntensity;
            centerMonsterLight.enabled = true;
            centerMonsterLight.intensity = originalMonsterLightIntensity;
        }
    }

    public void SetDrinkIconColor(Color color)
    {
        if (drinkIcon != null)
        {
            // show the drink icon
            GameObject liquidIcon = drinkIcon.gameObject.transform.GetChild(0).gameObject;
            liquidIcon.SetActive(true);
            liquidIcon.GetComponent<Image>().color = color;
        }
    }
}
