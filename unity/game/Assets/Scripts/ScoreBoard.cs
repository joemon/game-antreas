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
     * 
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
