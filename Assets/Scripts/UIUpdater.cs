using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    public Text level;
    public Text score;
    public Text highScore;
    public Text lives;
    
    // Start is called before the first frame update
    void Start()
    {
        level.text = "Level " + (GameManager.manager.currentLevel).ToString();
        score.text = GameManager.manager.currentScore.ToString();
        lives.text = GameManager.manager.currentLives.ToString();

        if(GameManager.manager.highScoreList.Count == 0)
        {
            highScore.text = "0";
        } else
        {
            highScore.text = GameManager.manager.highScoreList[0].score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
