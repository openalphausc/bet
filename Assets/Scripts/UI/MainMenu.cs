using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text versionText;
    
    // Start is called before the first frame update
    void Start()
    {
        versionText.text = "v" + Application.version;
    }

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
