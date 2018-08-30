using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {
    public AudioMixer audioMixer;
    public Slider slider;

    private void Start()
    {
        
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        slider.value = volume;
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("VolumeLevel", slider.value);
        Debug.Log(PlayerPrefs.GetFloat("VolumeLevel"));
    }

    public void SetFullScreen(bool isOn)
    {
        Debug.Log(isOn.ToString());
        Screen.fullScreen = isOn;
        if (Screen.fullScreen == true)
        {
            PlayerPrefs.SetString("FullScreen", "true");
        }
        else
        {
            PlayerPrefs.SetString("FullScreen", "false");
        }
        
    }

    

}
