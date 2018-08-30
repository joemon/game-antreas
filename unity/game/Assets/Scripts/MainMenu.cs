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
        slider.value = PlayerPrefs.GetFloat("VolumeLevel");
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" + PlayerPrefs.GetString("FullScreen"));
        if(PlayerPrefs.GetString("FullScreen") == "true")
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }
    

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
