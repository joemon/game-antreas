using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TestScript {
    Rocket rocket;
    LevelsManager levelsManager;
    GameManager gameManager;
    MovingObstacles movingObstacles;

    [SetUp]
    public void SetUp()
    {
        rocket = new GameObject().AddComponent<Rocket>();
        levelsManager = new GameObject().AddComponent<LevelsManager>();
        gameManager = new GameObject().AddComponent<GameManager>();
        movingObstacles = new GameObject().AddComponent<MovingObstacles>();
    }


    //------------------------------------ LevelManager --------------------------------------------

    // Checks if all the correct inputs for level selection, return true.
    [Test]
    public void TestSelectedLevelIsCorrect()
    {
        bool isCorrect;
        for (int i=2; i<=11; i++)
        {
            isCorrect = levelsManager.LevelIsCorrect(i);
            Assert.AreEqual(true, isCorrect);
        }
    }


    // Checks that a level cannot be a negative number.
    [Test]
    public void TestSelectedLevelIsNegative()
    {
        bool isWrong = levelsManager.LevelIsCorrect(-5);
        Assert.AreEqual(false, isWrong);
    }


    // Checks if an input for level selection is out of range (<2 or >11)
    [Test]
    public void TestSelectedLevelIsOutOfRange()
    {
        bool isWrong1 = levelsManager.LevelIsCorrect(1);
        bool isWrong2 = levelsManager.LevelIsCorrect(12);
        Assert.AreEqual(false, isWrong1);
        Assert.AreEqual(false, isWrong2);
    }
    
    
    // Checks if the argument of unlockLevels method is in the correct range.
    [Test]
    public void TestUnlockLevelsStateIsCorrect()
    {
        bool isCorrect;
        for (int i = 0; i <= 10; i++)
        {
            isCorrect = levelsManager.LevelStateIsCorrect(i);
            Assert.AreEqual(true, isCorrect);
        }
    }

    
    // Checks that the state of unlock levels cannot be negative.
    [Test]
    public void TestUnlockLevelsStateIsNegative()
    {
        bool isWrong = levelsManager.LevelStateIsCorrect(-2);
        Assert.AreEqual(false, isWrong);
    }


    // Checks that the state of unlock levels cannot be greater than 10.
    [Test]
    public void TestUnlockLevelsStateIsOutOfRange()
    {
        bool isWrong = levelsManager.LevelStateIsCorrect(11);
        Assert.AreEqual(false, isWrong);
    }


    //------------------------------------ GameManager --------------------------------------------

    // Checks if the score updates correctly when the player picks a correct answer.
    [Test]
    public void TestUpdateScoreOnCorrectAnswer()
    {
        Text scoreTxt = new GameObject().AddComponent<Text>();
        gameManager.Construct(scoreTxt, 400);

        int points = 100;
        int expectedScore = gameManager.GetScore() + points;

        int realScore = gameManager.UpdateScore(points);

        Assert.AreEqual(expectedScore, realScore);
    }


    // Checks if the score updates correctly when the player picks a wrong answer.
    [Test]
    public void TestUpdateScoreOnWrongAnswer()
    {
        Text scoreTxt = new GameObject().AddComponent<Text>();
        gameManager.Construct(scoreTxt, 400);

        int points = -50;
        int expectedScore = gameManager.GetScore() + points;

        int realScore = gameManager.UpdateScore(points);

        Assert.AreEqual(expectedScore, realScore);
    }


    // Checks that the level is completed (equations score >= 150)
    [Test]
    public void TestLevelCompleted()
    {   
        gameManager.completeLevelUI = new GameObject();
        gameManager.totalScoreText = new GameObject().AddComponent<Text>();
        gameManager.highScore = new GameObject().AddComponent<Text>();
        gameManager.Construct(250.0f);

        gameManager.SetScore(300);

        string result = gameManager.CompleteLevel();

        Assert.AreEqual("Level Completed", result);
    }


    // Checks that the level is failed (equations score < 150)
    [Test]
    public void TestLevelFailed()
    {
        gameManager.failedLevelUI = new GameObject();
        gameManager.SetScore(50);

        string result = gameManager.CompleteLevel();

        Assert.AreEqual("Level Failed", result);
    }


    //------------------------------------ MovingObstacles --------------------------------------------

    // Checks that the argument for period can be possitive.
    [Test]
    public void TestMovingObstaclesPeriodIsPossitive()
    {
        Vector3 moveVector = new Vector3(12f, 0f, 0f);
        float p = 3f;

        movingObstacles.Construct(moveVector, p);

        bool result = movingObstacles.MakeTransition();

        Assert.AreEqual(true, result);
    }

    // Checks that the argument for period cannot be negative.
    [Test]
    public void TestMovingObstaclesPeriodIsNegative()
    {
        Vector3 moveVector = new Vector3(12f, 0f, 0f);
        float p = -1f;

        movingObstacles.Construct(moveVector, p);

        bool result = movingObstacles.MakeTransition();

        Assert.AreEqual(false, result);
    }

    // Checks that the argument for period cannot be zero.
    [Test]
    public void TestMovingObstaclesPeriodIsZero()
    {
        Vector3 moveVector = new Vector3(12f, 0f, 0f);
        float p = 0f;

        movingObstacles.Construct(moveVector, p);

        bool result = movingObstacles.MakeTransition();

        Assert.AreEqual(false, result);
    }


    //------------------------------------ Rocket --------------------------------------------

    // Checks if the file with the equations is loaded and read correctly.
    [Test]
    public void TestLoadEquations()
    {
        TextAsset textfile = (TextAsset)Resources.Load("equations");
        Dictionary<string, string> allEquations = new Dictionary<string, string>();
        rocket.Construct(allEquations);


        bool result = rocket.LoadEquations(textfile);

        Assert.AreEqual(true, result);
    }

    // Checks if the file with the equations is null.
    [Test]
    public void TestLoadEquationsFail()
    {
        TextAsset textfile = null;
        Dictionary<string, string> allEquations = new Dictionary<string, string>();
        rocket.Construct(allEquations);


        bool result = rocket.LoadEquations(textfile);

        Assert.AreEqual(false, result);
    }
    

}
