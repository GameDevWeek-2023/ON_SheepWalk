using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public bool isPaused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused == true)
            {
                print("Resume");
                Resume();
                isPaused = false;
                //FindObjectOfType<AudioManager>().Play("Select");
            }

            else
            {
                print("Paused");
                Pause();
                isPaused = true;
                //FindObjectOfType<AudioManager>().Play("Select");
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        isPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        isPaused = true;
    }

    public void LoadOptions()
    {
        print("Loading Options..");
    }

    public void LoadMenu()
    {
        GameIsPaused = false;
        isPaused = false;
        Time.timeScale = 1f;
        print("Quitting to Title...");
        SceneManager.LoadScene("Start Menu");
    }
}
