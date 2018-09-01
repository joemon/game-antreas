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

    // Use this for initialization
    void Start() {
        //PlayerPrefs.DeleteKey("LevelsState");

        levelsState = PlayerPrefs.GetInt("LevelsState");
        unlockLevels(levelsState);
    }

    private void unlockLevels(int lState)
    {
        int i = 0;
        for (i=0; i < levels.Length; i++)
        {
            if (i <= lState){
                levels[i].interactable = true;
            }
            else{
                levels[i].interactable = false;
            }
        }
    }

    public void loadSelectedLevel(int level){
        lev = level;
        Invoke("loadLevel", 0.5f);
    }

    public void loadLevel()
    {
        SceneManager.LoadScene(lev);
    } 

    public void Exit(){
        SceneManager.LoadScene(0);
    }

}
