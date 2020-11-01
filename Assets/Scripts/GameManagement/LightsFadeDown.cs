using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LightsFadeDown : MonoBehaviour
{
    public List<GameObject> lights;
    private List<float> maxIntensities;
    
    public float fadeDownTime;

    private float timer;
    private bool fadingDown;

    public string sceneToChangeTo;
    
    // Start is called before the first frame update
    void Start()
    {
        fadingDown = false;
        timer = fadeDownTime;
        maxIntensities = new List<float>();
        foreach (GameObject light in lights) maxIntensities.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingDown)
        {
            timer -= Time.deltaTime;
            // intensity of each light fades up, proportional with the time it takes
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].GetComponent<Light2D>().intensity = Mathf.Lerp(0, maxIntensities[i], timer / fadeDownTime);
            }
            // when timer hits 0, stop and change scene
            if (timer <= 0)
            {
                fadingDown = false;
                // change scene here
                SceneManager.LoadScene(sceneToChangeTo);
            }
        }
    }

    public void StartFadeDown()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            maxIntensities[i] = lights[i].GetComponent<Light2D>().intensity;
        }
        fadingDown = true;
    }
}