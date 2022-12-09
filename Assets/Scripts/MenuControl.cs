using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public string goBackScene;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject highScoreInput;
    public GameObject highScoreSceneTryAgainButton;
    public GameObject loadText;

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

        if (optionsMenu != null)
        {
            if (optionsMenu.activeInHierarchy)
            {
                optionsMenu.SetActive(false);
            }
        }

        if (highScoreInput != null)
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
            
            if (optionsMenu.activeInHierarchy)
            {
                optionsMenu.SetActive(false);
            }
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
        AudioManager.aManager.StopAll();
        AudioManager.aManager.Play("Macgyver");
        SceneManager.LoadScene("HighScores");
    }

    public void LoadClassicInstructions()
    {
        SceneManager.LoadScene("ClassicInstructions");
    }

    public void LoadCMultiplayerInstructions()
    {
        SceneManager.LoadScene("MultiplayerInstructions");
    }

    public void LoadClassic()
    {
        AudioManager.aManager.StopAll();
        AudioManager.aManager.Play("LevelMusic");
        SceneManager.LoadScene("ClassicMode");
    }

    public void LoadClassicWithPlayerReset()
    {
        ResetGameManager();
        AudioManager.aManager.StopAll();
        AudioManager.aManager.Play("LevelMusic");
        SceneManager.LoadScene("ClassicMode");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
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
        Time.timeScale = 1;
        AudioManager.aManager.StopAll();
        ResetGameManager();
        GameManager.manager.highScoresFromMainMenu = false;
        AudioManager.aManager.Play("MainTheme");
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

    public void ToggleOptionsMenu()
    {
        if (optionsMenu != null)
        {
            if (optionsMenu.activeInHierarchy)
            {
                optionsMenu.SetActive(false);
            }
            else
            {
                optionsMenu.SetActive(true);
            }
        }
    }

    public void ResetGameManager()
    {
        GameManager.manager.currentScore = 0;
        GameManager.manager.currentLevel = 0;
        GameManager.manager.currentLives = 3;
        GameManager.manager.playerShots = 0;
    }

    public void SubmitHighScore(string input)
    {
        GameManager.manager.AddNewScore(input, GameManager.manager.currentScore);
        GameManager.manager.SaveHighScores();
        AudioManager.aManager.StopAll();
        AudioManager.aManager.Play("Macgyver");
        SceneManager.LoadScene("HighScores");
    }

    public void SaveFromMenu()
    {
        GameManager.manager.Save();
    }

    public void LoadSavedFromMenu()
    {
        GameManager.manager.Load();
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            if(loadText != null)
            {
                loadText.SetActive(true);
            }
        }
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
