using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ControlsPanel;

    public Button ReturnControls;
    public Button StartButton;

    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Shader Test Scene");
        print("It's Gaming Time");
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        MainMenu.SetActive(false);
        ControlsPanel.SetActive(false);
    }

    public void ControlsButton()
    {
        MainMenu.SetActive(false);
        ControlsPanel.SetActive(true);
        //ReturnControls.Select();
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        MainMenu.SetActive(true);
        ControlsPanel.SetActive(false);
        //StartButton.Select();
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }

}