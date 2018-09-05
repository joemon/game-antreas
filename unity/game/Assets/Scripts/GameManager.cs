using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    // Variable for the AudioMixet to handle the volume of the sounds.
    public AudioMixer audioMixer;

    // Variables for update and display the current score on the screen
    public GameObject scoreObject;
    private Text scoreText;
    private int score;

    // Variables for update and display the timer on the screen
    public GameObject timerObject;
    private Text timerText;
    private float timeLeft = 300.0f;
    private bool isTimerOn = false;
    

    // Variables for the winning screen and loosing screen
    public GameObject completeLevelUI;
    public GameObject failedLevelUI;

    // Variables for total score and highscore of the player
    public Text totalScoreText;
    public Text highScore;

    // For the unlock of the new levels
    int levelPassed, currentScene;


    public void Construct(Text scoreTxt, int scoreNum)
    {
        scoreText = scoreTxt;
        score = scoreNum;
    }

    public void Construct(float timeLft)
    {
        timeLeft = timeLft;

        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

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

            // If time is over then restart the level otherwise update the countdown.
            if (timeLeft < 0)
            {
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

    public int UpdateScore(int points)
    {
        score = score + points;
        scoreText.text = score.ToString();
        return score;

    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int s)
    {
        score = s;
    }

    public Text GetScoreText()
    {
        return scoreText;
    }

    public void SetScoreText(Text txt)
    {
        scoreText = txt;
    }

    public string CompleteLevel()
    {
        int score = GetScore();
        
        // if the player answer at least 3 out of 6 questions correct then level complete
        if (score >= 150)
        {
            completeLevelUI.SetActive(true);

            int timeLeft = GetTimeLeft();
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

            return "Level Completed";
        } // Else the level failed
        else
        {
            failedLevelUI.SetActive(true);
            return "Level Failed";
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
