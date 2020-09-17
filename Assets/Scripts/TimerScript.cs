using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
{
    [SerializeField] private TMP_Text uiText;

    private float timer;
    public bool runTimer;

    void Start()
    {
        timer = 0;
        runTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (runTimer)
        {
            timer += Time.deltaTime;
            uiText.text = timer.ToString("F");
        }

        else
        {
            addTime();
            timer = 0;
            runTimer = true;
        }
        
    }

    public void addTime()
    {
        
    }}
