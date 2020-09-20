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

    void Start()
    {
        timer = 0.0f;
        uiText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        //Only run the timer if box is checked
        if (runTimer)
        {
            timer += Time.deltaTime;
            uiText.text = timer.ToString("F");
        }

    }
}