using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public Slider slider;
    public Toggle toggle;
   

    // Use this for initialization
    void Start()
    {
        // Every time the game starts, load the preferences of the player for volume and fullscreen
        slider.value = PlayerPrefs.GetFloat("VolumeLevel");
        if(PlayerPrefs.GetString("FullScreen") == "true")
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }


    // If the player clicks on play button then the next scene has to load
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
