using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  With the help of code from John French
 *  (https://gamedevbeginner.com/how-to-keep-score-in-unity-with-loading-and-saving/)
 */

public class HighScore : MonoBehaviour
{
    public HighScoreDisplay[] highScoreDisplayArray;

    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        GameManager.manager.highScoreList.Sort((HighScoreEntry x, HighScoreEntry y) => y.score.CompareTo(x.score));
        for (int i = 0; i < highScoreDisplayArray.Length; i++)
        {
            if (i < GameManager.manager.highScoreList.Count)
            {
                highScoreDisplayArray[i].DisplayHighScore(GameManager.manager.highScoreList[i].name, GameManager.manager.highScoreList[i].score);
            }
            else
            {
                highScoreDisplayArray[i].HideEntryDisplay();
            }
        }
    }
}
