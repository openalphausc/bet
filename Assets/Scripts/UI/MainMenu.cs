using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // TODO: Don't hardcode load scene. Create some sort of global variable
    public void Play()
    {
        SceneManager.LoadScene("BarTendingScene");
    }

    public void Quit()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
