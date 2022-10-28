using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  Used code from John French
 *  (https://gamedevbeginner.com/how-to-keep-score-in-unity-with-loading-and-saving/)
 */

public class HighScoreDisplay : MonoBehaviour
{
    public Text nameText;
    public Text scoreText;
    
    public void DisplayHighScore(string name, int score)
    {
        nameText.text = name;
        scoreText.text = string.Format("{0:00000}", score);
    }

    public void HideEntryDisplay()
    {
        nameText.text = "";
        scoreText.text = "";
    }
}
