using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public string previousScene;
    public GameObject pauseMenu;

    private void Start()
    {
        if (pauseMenu != null)
        {
            if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void LoadPlayerMenu()
    {
        SceneManager.LoadScene("PlayerMenu");
    }

    public void LoadHighScoreScene()
    {
        SceneManager.LoadScene("HighScore");
    }

    public void LoadSinglePlayerModes()
    {
        SceneManager.LoadScene("SingleModes");
    }

    public void LoadClassicInstructions()
    {
        SceneManager.LoadScene("ClassicInstructions");
    }

    public void LoadClassic()
    {
        SceneManager.LoadScene("ClassicMode");
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void Back()
    {
        SceneManager.LoadScene(previousScene);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void TogglePauseMenu()
    {
        if(pauseMenu != null)
        {
            if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
        }
    }
}
