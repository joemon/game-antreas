using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {
    public Text[] scores;
    public Text totalScore;
    int total;

    // Use this for initialization
    void Start () {
        total = 0;
    }


    /*
     * Get the values of the highscores for each level from the PlayerPrefs and then update 
     * the appropriate text field in the scoreboard with these values.
     * Also, calculates the total score by adding together the highscores from all the levels
     * and update the totalscore text field in the scoreboard.
     */
    public void UpdateScoreboard()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].text = PlayerPrefs.GetInt("HighScore" + (i + 2).ToString()).ToString() + " points";
            total = total + PlayerPrefs.GetInt("HighScore" + (i + 2).ToString());
        }

        PlayerPrefs.SetInt("TotalScore", total);
        totalScore.text = total.ToString() + " points";
    }

}
