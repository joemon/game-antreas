using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    public AudioMixer audioMixer;

    // Variable for update and display the current score
    public GameObject scoreObject;
    Text scoreText;
    int score;

    // Variable for update and display the timer
    public GameObject timerObject;
    Text timerText;
    float timeLeft = 300.0f;
    bool isTimerOn = false;

    // Variables for the winning screen and total score 
    //public GameObject completeLevelUI;
    //public Text totalScoreText;

    // Variable to keep the high score for each level
    //public Text highScore;

    private void Start()
    {
        audioMixer = GetComponent<AudioMixer>();
    }   
    
    void Update()
    {
        if (isTimerOn == true)
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
    }

    void Awake()
    {
        if (instance == null){ instance = this; }
        else if (instance != null) { Destroy(gameObject); }

        scoreText = scoreObject.GetComponent<Text>();
        scoreText.text = score.ToString();

        timerText = timerObject.GetComponent<Text>();
        timerText.text = "Timer: " + timeLeft.ToString("0");
        
        //totalScoreText = GetComponent<Text>();
    }

    public void StartTimer()
    {
        isTimerOn = true;
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

    /*
    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        int total = getScore() + GetTimeLeft();
        totalScoreText.text = "Score: 650";
    }
    */

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

   

}
