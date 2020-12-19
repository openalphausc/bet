using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public string MainMenuSceneName;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if game is paused, resume
            if (isPaused)
            {
                Resume();
            }
            // otherwise, pause
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // if game is paused, go back to main menu
            if (isPaused)
            {
                LoadMenu();
            }
        }
    }

    // TODO: Pitch down music while in pause menu?
    public void Resume()
    {
        // close the pause menu, resume time, update isPaused
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        // open the pause menu, pause time, update isPaused
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(MainMenuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
