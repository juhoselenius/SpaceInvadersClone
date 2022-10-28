using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    //public string playerName;

    public int currentScore;
    public int currentLevel;
    public int currentLives;

    public bool paused;

    private void Awake()
    {
        if(manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
            currentLevel = 0;
            currentScore = 0;
            currentLives = 3;

            paused = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        Debug.Log("Game Saved");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData();
        //data.playerName = playerName;
        data.currentScore = currentScore;
        data.currentLevel = currentLevel;
        data.currentLives = currentLives;

        bf.Serialize(file, data);
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            Debug.Log("Game loaded");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);

            //playerName = data.playerName;
            currentLevel = data.currentLevel;
            currentScore = data.currentScore;
            currentLives = data.currentLives;
        }
    }
}

class PlayerData
{
    public string playerName;

    public int currentScore;
    public int currentLevel;
    public int currentLives;
}
