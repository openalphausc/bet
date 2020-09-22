using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
{
    [SerializeField] public TMP_Text uiText;

    private float timer;
    public bool runTimer = true;
    public float offscreenX = 79.0f;
    public float timerMultiplier = 1.0f;
    public Slider slider;
    public GameObject currentMonster;
    public GameObject dialogueBox;
    void Start()
    {
        timer = 0.0f;
        uiText.text = "";
        slider.value = 100.0f;
        currentMonster = GameObject.FindWithTag("Monster");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMonster == null)
        {
            currentMonster = GameObject.FindWithTag("Monster");
        }
        //Only run the timer if box is checked
        if (runTimer)
        {
            timer += Time.deltaTime;
            uiText.text = timer.ToString("F");
        }
        if (dialogueBox.activeSelf == true)
        {
            slider.value -= (Time.deltaTime*timerMultiplier);
        }
        
        if (currentMonster != null)
        {
            if (currentMonster.transform.position.x > offscreenX)
            {
                slider.value = 100.0f;
            }
        }
        
    }
}