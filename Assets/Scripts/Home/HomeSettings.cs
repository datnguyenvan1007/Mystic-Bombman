using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeSettings : MonoBehaviour
{
    [SerializeField] private Slider controllerOpacitySlider;
    [SerializeField] private Text controlsText;
    [SerializeField] private Text flipControlText;
    [SerializeField] private Text soundText;
    [SerializeField] private Text percentText;
    void Start()
    {
        controllerOpacitySlider.value = PlayerPrefs.GetFloat("ControllerOpacity", 45f);
        percentText.text = controllerOpacitySlider.value + "%";
        if (PlayerPrefs.GetInt("ControllerType", 2) == 1)
        {
            controlsText.text = "JOYSTICK";
        }
        else
        {
            controlsText.text = "DPAD";
        }
        if (PlayerPrefs.GetInt("FlipControls", 0) == 1)
        {
            flipControlText.text = "ON";
        }
        else
        {
            flipControlText.text = "OFF";
        }
        if (PlayerPrefs.GetInt("Sound", 0) == 1)
        {
            soundText.text = "ON";
        }
        else
        {
            soundText.text = "OFF";
        }
    }
    public void ChangeControllerOpacity()
    {
        PlayerPrefs.SetFloat("ControllerOpacity", controllerOpacitySlider.value);
        percentText.text = controllerOpacitySlider.value + "%";
    }
    public void SelectControllerType()
    {
        if (controlsText.text == "JOYSTICK")
        {
            PlayerPrefs.SetInt("ControllerType", 2);
            controlsText.text = "DPAD";
        }
        else
        {
            PlayerPrefs.SetInt("ControllerType", 1);
            controlsText.text = "JOYSTICK";
        }
    }
    public void SelectFlipControls()
    {
        if (flipControlText.text == "ON")
        {
            PlayerPrefs.SetInt("FlipControls", 0);
            flipControlText.text = "OFF";
        }
        else
        {
            PlayerPrefs.SetInt("FlipControls", 1);
            flipControlText.text = "ON";
        }
    }
    public void SelectSound()
    {
        if (soundText.text == "ON")
        {
            PlayerPrefs.SetInt("Sound", 0);
            soundText.text = "OFF";
            HomeManager.instance.Mute();
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            soundText.text = "ON";
            HomeManager.instance.UnMute();
        }
    }
}