using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text versionText;
    public string BarTendingSceneName;
    public string PlaytestingContextSceneName;
    public bool LoadPlaytestingContext;

    // Start is called before the first frame update
    void Start()
    {
        versionText.text = "v" + Application.version;
    }

    // TODO: Don't hardcode load scene. Create some sort of global variable
    public void Play()
    {
        if (LoadPlaytestingContext)
        {
            Debug.Log("Loading Playtesting Context Menu");
            SceneManager.LoadScene(PlaytestingContextSceneName);
        }
        else
        {
            SceneManager.LoadScene(BarTendingSceneName);
        }
    }

    public void Quit()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
