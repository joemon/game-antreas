using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    // Variable for update and display the current score
    public GameObject scoreObject;
    Text scoreText;
    int score;

    // Variable for update and display the timer
    public GameObject timerObject;
    Text timerText;
    float timeLeft = 300.0f;

    void Update()
    {
        timeLeft = timeLeft - Time.deltaTime;
        if (timeLeft < 0)
        {
            // First load a Game Over screen
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            timerText = timerObject.GetComponent<Text>();
            timerText.text = "Timer: " + timeLeft.ToString("0");
        }
    }

    void Awake()
    {
        if (instance == null){ instance = this; }
        else if (instance != null) { Destroy(gameObject); }

        scoreText = scoreObject.GetComponent<Text>();
        scoreText.text = score.ToString();

        timerText = timerObject.GetComponent<Text>();
        timerText.text = "Timer: " + timeLeft.ToString("0");

    }

    public void UpdateScore(int points)
    {
        score = score + points;
        scoreText.text = score.ToString();
    }

    public int GetTimeLeft()
    {
        return Mathf.RoundToInt(timeLeft);
    }

    public int getScore()
    {
        return score;
    }


}
