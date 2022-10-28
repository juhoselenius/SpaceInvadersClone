using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public string goBackScene;
    public GameObject pauseMenu;
    public GameObject highScoreInput;
    public GameObject highScoreSceneTryAgainButton;

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

        if(highScoreInput != null)
        {
            if(ScoredTopFive())
            {
                highScoreInput.SetActive(true);
            }
            else
            {
                highScoreInput.SetActive(false);
            }
        }

        if(highScoreSceneTryAgainButton != null)
        {
            if(GameManager.manager.highScoresFromMainMenu)
            {
                highScoreSceneTryAgainButton.SetActive(false);
            }
            else
            {
                highScoreSceneTryAgainButton.SetActive(true);
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
        if(SceneManager.GetActiveScene().name.CompareTo("MainMenu") == 0)
        {
            GameManager.manager.highScoresFromMainMenu = true;
        }
        else
        {
            GameManager.manager.highScoresFromMainMenu = false;
        }

        SceneManager.LoadScene("HighScores");
    }

    public void LoadClassicInstructions()
    {
        SceneManager.LoadScene("ClassicInstructions");
    }

    public void LoadClassic()
    {
        ResetGameManager();
        SceneManager.LoadScene("ClassicMode");
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void Back()
    {
        SceneManager.LoadScene(goBackScene);
    }

    public void ToMainMenu()
    {
        ResetGameManager();
        GameManager.manager.highScoresFromMainMenu = false;
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

    public void ResetGameManager()
    {
        GameManager.manager.currentScore = 0;
        GameManager.manager.currentLevel = 0;
        GameManager.manager.currentLives = 3;
    }

    public void SubmitHighScore(string input)
    {
        GameManager.manager.AddNewScore(input, GameManager.manager.currentScore);
        SceneManager.LoadScene("HighScores");
    }

    public bool ScoredTopFive()
    {
        //Debug.Log("Count: " + GameManager.manager.highScoreList.Count);
        if(GameManager.manager.highScoreList.Count >= 5)
        {
            if (GameManager.manager.currentScore > GameManager.manager.highScoreList[4].score)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
}
