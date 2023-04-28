using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeSettings : MonoBehaviour
{
    [SerializeField] private Slider controllerOpacitySlider;
    [SerializeField] private GameObject dpad;
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject flipControlOn;
    [SerializeField] private GameObject flipControlOff;
    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;
    void Start()
    {
        controllerOpacitySlider.value = PlayerPrefs.GetFloat("ControllerOpacity", 45f);
        if (PlayerPrefs.GetInt("ControllerType", 2) == 1)
        {
            joystick.SetActive(true);
            dpad.SetActive(false);
        }
        else
        {
            joystick.SetActive(false);
            dpad.SetActive(true);
        }
        if (PlayerPrefs.GetInt("FlipControls", 0) == 1) {
            flipControlOn.SetActive(true);
            flipControlOff.SetActive(false);
        }
        else {
            flipControlOff.SetActive(true);
            flipControlOn.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Sound", 0) == 1) {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else {
            soundOff.SetActive(true);
            soundOn.SetActive(false);
        }
    }
    public void ChangeControllerOpacity()
    {
        PlayerPrefs.SetFloat("ControllerOpacity", controllerOpacitySlider.value);
    }
    public void SelectControllerType() {
        if (joystick.activeSelf) {
            PlayerPrefs.SetInt("ControllerType", 2);
            dpad.SetActive(true);
            joystick.SetActive(false);
        }
        else {
            PlayerPrefs.SetInt("ControllerType", 1);
            dpad.SetActive(false);
            joystick.SetActive(true);
        }
    }
    public void SelectFlipControls() {
        if (flipControlOn.activeSelf) {
            PlayerPrefs.SetInt("FlipControls", 0);
            flipControlOff.SetActive(true);
            flipControlOn.SetActive(false);
        }
        else {
            PlayerPrefs.SetInt("FlipControls", 1);
            flipControlOff.SetActive(false);
            flipControlOn.SetActive(true);
        }
    }
    public void SelectSound() {
        if (flipControlOn.activeSelf) {
            PlayerPrefs.SetInt("Sound", 0);
            soundOff.SetActive(true);
            soundOn.SetActive(false);
        }
        else {
            PlayerPrefs.SetInt("FlipControls", 1);
            soundOff.SetActive(false);
            soundOn.SetActive(true);
        }
    }
}