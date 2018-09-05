using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour {
    public Button[] levels;
    int levelsState;
    int lev;
    int firstLevelScene = 2, lastLevelScene = 11;

    // Use this for initialization
    void Start() {
        //PlayerPrefs.DeleteKey("LevelsState");

        levelsState = PlayerPrefs.GetInt("LevelsState");
        unlockLevels(levelsState);
    }

    public void unlockLevels(int lState)
    {
        if (LevelStateIsCorrect(lState) == true)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                if (i <= lState)
                {
                    levels[i].interactable = true;
                }
                else
                {
                    levels[i].interactable = false;
                }
            }
        }
        else
        {
            throw new ArgumentException("Levels State is not valid");
        }
    }

    public bool LevelStateIsCorrect(int lState)
    {
        if ((lState < 0))
        {
            return false;
        }
        else if (lState > 10)
        {
            return false;
        }

        return true;
    }

    // Loads the appropriete level 
    /// <summary>
    /// This function loads the level that is selected by the player.
    /// </summary>
    /// <param name="level">The number of the scene that corresponds to the selected level</param>
    /// <returns></returns>
    public void loadSelectedLevel(int level)
    {
        if(LevelIsCorrect(level) == true)
        {
            Invoke("loadLevel", 0.5f);
        }
        else
        {
            throw new ArgumentException("Selected level not found");
        }
    }

    public bool LevelIsCorrect(int level)
    {
        if ((level >= firstLevelScene) && (level <= lastLevelScene))
        {
            lev = level;
            return true;
        }

        return false;

    }

    public void loadLevel()
    {
        SceneManager.LoadScene(lev);
    } 

    public void Exit(){
        SceneManager.LoadScene(0);
    }

}
