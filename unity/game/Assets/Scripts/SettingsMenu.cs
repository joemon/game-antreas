using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {
    public AudioMixer audioMixer;
    public Slider slider;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        slider.value = volume;
    }

    public void SaveVolumeStatus()
    {
        PlayerPrefs.SetFloat("VolumeLevel", slider.value);
    }

    public void SetFullScreen(bool isOn)
    {
        Screen.fullScreen = isOn;
    }

    public void SaveFullScreenStatus()
    {
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
