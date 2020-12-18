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

    public void Play()
    {
        SceneManager.LoadScene("Tabsheet");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
