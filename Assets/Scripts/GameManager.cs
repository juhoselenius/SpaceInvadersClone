using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public int currentScore;
    public int savedScore;
    public int currentLevel;
    public int currentLives;
    public int savedLives;

    public int playerShots;

    public bool paused;

    public List<HighScoreEntry> highScoreList;

    public bool highScoresFromMainMenu;

    public bool gameLoaded;

    private void Awake()
    {
        if(manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
            currentLevel = 0;
            currentScore = 0;
            savedScore = 0;
            currentLives = 3;
            savedLives = 3;

            playerShots = 0;

            highScoreList = new List<HighScoreEntry>();

            paused = false;

            gameLoaded = false;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadHighScores();
    }

    public void Save()
    {
        Debug.Log("Game Saved");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData();
        data.savedScore = savedScore;
        data.currentLevel = currentLevel;
        data.savedLives = savedLives;

        bf.Serialize(file, data);
        file.Close();
    }

    public void SaveHighScores()
    {
        // Saving high scores from John French (https://gamedevbeginner.com/how-to-keep-score-in-unity-with-loading-and-saving/)
        XmlSerializer serializer = new XmlSerializer(typeof(List<HighScoreEntry>));
        FileStream stream = new FileStream(Application.persistentDataPath + "/highscores.xml", FileMode.Create);
        serializer.Serialize(stream, highScoreList);
        stream.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            Debug.Log("Game loaded");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);

            currentLevel = data.currentLevel;
            savedScore = data.savedScore;
            currentScore = data.savedScore;
            savedLives = data.savedLives;
            currentLives = data.savedLives;

            gameLoaded = true;
        }
    }

    public void LoadHighScores()
    {
        // Loading high scores from John French (https://gamedevbeginner.com/how-to-keep-score-in-unity-with-loading-and-saving/)
        if (File.Exists(Application.persistentDataPath + "/highscores.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<HighScoreEntry>));
            FileStream stream = new FileStream(Application.persistentDataPath + "/highscores.xml", FileMode.Open);
            highScoreList = (List<HighScoreEntry>)serializer.Deserialize(stream);
        }
    }

    public void AddNewScore(string playerName, int playerScore)
    {
       highScoreList.Add(new HighScoreEntry { name = playerName, score = playerScore });
    }
}

[Serializable]
class PlayerData
{
    public int savedScore;
    public int currentLevel;
    public int savedLives;
}

[System.Serializable]
public class Leaderboard
{
    public List<HighScoreEntry> list = new List<HighScoreEntry>();
}
