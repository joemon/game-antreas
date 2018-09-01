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


    

    // Variable to keep the high score for each level
    //public Text highScore;

    // Variables for the winning screen and loosing screen
    public GameObject completeLevelUI;
    public GameObject failedLevelUI;

    // Variables for total score and highscore of the player
    public Text totalScoreText;
    public Text highScore;

    // For the unlock of the new levels
    int levelPassed, currentScene;


    private void Start()
    {
        audioMixer = GetComponent<AudioMixer>();

        currentScene = SceneManager.GetActiveScene().buildIndex;
        levelPassed = PlayerPrefs.GetInt("LevelsState");

        highScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore" + currentScene.ToString()).ToString();

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

    public int GetTimeLeft()
    {
        return Mathf.RoundToInt(timeLeft);
    }

    public void UpdateScore(int points)
    {
        score = score + points;
        scoreText.text = score.ToString();
    }

    public int getScore()
    {
        return score;
    }


    public void CompleteLevel()
    {
        int score = getScore();

        // if the player answer at least 3 out of 6 questions correct then level complete
        if (score >= 150)
        {
            int timeLeft = GameManager.instance.GetTimeLeft();
            completeLevelUI.SetActive(true);
            totalScoreText.text = "Total Score: " + (score + timeLeft).ToString();

            if ((score + timeLeft) > PlayerPrefs.GetInt("HighScore" + currentScene.ToString()))
            {
                PlayerPrefs.SetInt("HighScore" + currentScene.ToString(), (score + timeLeft));
                highScore.text = "High Score: " + (score + timeLeft).ToString();
            }

            if (levelPassed < (currentScene - 1))
            {
                PlayerPrefs.SetInt("LevelsState", (currentScene - 1));
            }

        } // Else the level failed
        else
        {
            failedLevelUI.SetActive(true);
        }
    }

    
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
