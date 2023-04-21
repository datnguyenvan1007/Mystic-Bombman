using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    // [SerializeField] private Sprite[] spritesOfButtonContinue;
    // [SerializeField] private Sprite[] spritesOfButtonNewGame;
    // [SerializeField] private Sprite[] spritesOfButtonSettings;
    // [SerializeField] private Sprite[] spritesOfButtonExit;
    // [SerializeField] private Image buttonContinue;
    // [SerializeField] private Image buttonNewGame;
    // [SerializeField] private Image buttonSettings;
    // [SerializeField] private Image buttonExit;
    [SerializeField] private Image buttonYes;
    [SerializeField] private Image buttonNo;
    [SerializeField] private Slider controllerOpacitySlider;
    // [SerializeField] private Text controlerOpacityPercent;
    // [SerializeField] private Image controllerTypeJoystick;
    // [SerializeField] private Image controllerTypeDpad;
    // [SerializeField] private Image flipControls;
    [SerializeField] private Image soundOn;
    [SerializeField] private Image soundOff;
    [SerializeField] private Image flipControlsOn;
    [SerializeField] private Image flipControlsOff;
    // [SerializeField] private Sprite[] spritesOfJoystick;
    // [SerializeField] private Sprite[] spritesOfDpad;
    // [SerializeField] private Sprite[] spritesOfFlipControl;
    [SerializeField] private Sprite[] spritesOn;
    [SerializeField] private Sprite[] spritesOff;
    [SerializeField] private Sprite[] spritesOfButtonYes;
    [SerializeField] private Sprite[] spritesOfButtonNo;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject btnContinue;
    [SerializeField] private GameObject prompt;
    [SerializeField] private GameObject dpad;
    [SerializeField] private GameObject joystick;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        controllerOpacitySlider.value = PlayerPrefs.GetFloat("ControllerOpacity", 45f);
        // controlerOpacityPercent.text = controllerOpacitySlider.value + "%";
        // btnContinue.SetActive(PlayerPrefs.HasKey("Left"));

        if (PlayerPrefs.GetInt("ControllerType", 2) == 1)
        {
            // controllerTypeJoystick.sprite = spritesOfJoystick[1];
            // controllerTypeDpad.sprite = spritesOfDpad[0];
            joystick.SetActive(true);
            dpad.SetActive(false);
        }
        else
        {
            // controllerTypeJoystick.sprite = spritesOfJoystick[0];
            // controllerTypeDpad.sprite = spritesOfDpad[1];
            joystick.SetActive(false);
            dpad.SetActive(true);
        }

        // if (PlayerPrefs.GetInt("FlipControls", 0) == 1)
        // {
        //     flipControls.sprite = spritesOfFlipControl[1];
        // }
        // else
        // {
        //     flipControls.sprite = spritesOfFlipControl[0];
        // }
        if (PlayerPrefs.GetInt("FlipControls", 0) == 1)
        {
            flipControlsOn.sprite = spritesOn[1];
            flipControlsOff.sprite = spritesOff[0];
        }
        else
        {
            flipControlsOn.sprite = spritesOn[0];
            flipControlsOff.sprite = spritesOff[1];
        }

        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            soundOn.sprite = spritesOn[1];
            soundOff.sprite = spritesOff[0];
            audioSource.mute = false;
        }
        else
        {
            soundOn.sprite = spritesOn[0];
            soundOff.sprite = spritesOff[1];
            audioSource.mute = true;
        }
    }
    public void ChangeControllerOpacity()
    {
        PlayerPrefs.SetFloat("ControllerOpacity", controllerOpacitySlider.value);
        // controlerOpacityPercent.text = controllerOpacitySlider.value + "%";
    }
    // public void SelectControlTypeJoystick()
    // {
    //     PlayerPrefs.SetInt("ControllerType", 1);
    //     controllerTypeJoystick.sprite = spritesOfJoystick[1];
    //     controllerTypeDpad.sprite = spritesOfDpad[0];
    // }
    // public void SelectControleTypeDPad()
    // {
    //     PlayerPrefs.SetInt("ControllerType", 2);
    //     controllerTypeJoystick.sprite = spritesOfJoystick[0];
    //     controllerTypeDpad.sprite = spritesOfDpad[1];
    // }
    // public void SelectFlipControls()
    // {
    //     int flip = PlayerPrefs.GetInt("FlipControls") == 1 ? 0 : 1;
    //     PlayerPrefs.SetInt("FlipControls", flip);
    //     if (flip == 1)
    //     {
    //         flipControls.sprite = spritesOfFlipControl[1];
    //     }
    //     else
    //     {
    //         flipControls.sprite = spritesOfFlipControl[0];
    //     }
    // }
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
    public void SelectFlipControlsOn()
    {
        if (PlayerPrefs.GetInt("FlipControls", 0) == 1)
            return;
        PlayerPrefs.SetInt("FlipControls", 1);
        flipControlsOn.sprite = spritesOn[1];
        flipControlsOff.sprite = spritesOff[0];
    }
    public void SelectFlipControlsOff()
    {
        if (PlayerPrefs.GetInt("FlipControls", 0) == 0)
            return;
        PlayerPrefs.SetInt("FlipControls", 0);
        flipControlsOn.sprite = spritesOn[0];
        flipControlsOff.sprite = spritesOff[1];
    }
    public void SelectSoundOn()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
            return;
        PlayerPrefs.SetInt("Sound", 1);
        soundOn.sprite = spritesOn[1];
        soundOff.sprite = spritesOff[0];
        audioSource.mute = false;
    }
    public void SelectSoundOff()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 0)
            return;
        PlayerPrefs.SetInt("Sound", 0);
        soundOn.sprite = spritesOn[0];
        soundOff.sprite = spritesOff[1];
        audioSource.mute = true;
    }
    // public void OnPointerDownContinue()
    // {
    //     buttonContinue.sprite = spritesOfButtonContinue[1];
    // }
    public void Continue()
    {
        // buttonContinue.sprite = spritesOfButtonContinue[0];
        SceneManager.LoadScene(1);
    }
    // public void OnPointerDownNewGame()
    // {
    //     buttonNewGame.sprite = spritesOfButtonNewGame[1];
    // }
    public void NewGame()
    {
        // buttonNewGame.sprite = spritesOfButtonNewGame[0];
        if (PlayerPrefs.HasKey("Left"))
        {
            buttons.SetActive(false);
            prompt.SetActive(true);
        }
        else
        {
            PlayerPrefs.DeleteKey("Score");
            PlayerPrefs.DeleteKey("NumberOfBombs");
            PlayerPrefs.DeleteKey("Flame");
            PlayerPrefs.DeleteKey("WallPass");
            PlayerPrefs.DeleteKey("BombPass");
            PlayerPrefs.DeleteKey("FlamePass");
            PlayerPrefs.DeleteKey("Speed");
            PlayerPrefs.DeleteKey("Stage");
            PlayerPrefs.DeleteKey("Left");
            PlayerPrefs.DeleteKey("Detonator");
            SceneManager.LoadScene(1);
        }
    }
    public void OnPointerDownButtonYes()
    {
        buttonYes.sprite = spritesOfButtonYes[1];
        PlayerPrefs.DeleteKey("Score");
        PlayerPrefs.DeleteKey("NumberOfBombs");
        PlayerPrefs.DeleteKey("Flame");
        PlayerPrefs.DeleteKey("WallPass");
        PlayerPrefs.DeleteKey("BombPass");
        PlayerPrefs.DeleteKey("FlamePass");
        PlayerPrefs.DeleteKey("Speed");
        PlayerPrefs.DeleteKey("Stage");
        PlayerPrefs.DeleteKey("Left");
        PlayerPrefs.DeleteKey("Detonator");
    }
    public void OnPointerUpButtonYes()
    {
        buttonYes.sprite = spritesOfButtonYes[0];
        SceneManager.LoadScene(1);
    }
    public void OnPointerDownButtonNo()
    {
        buttonNo.sprite = spritesOfButtonNo[1];
    }
    public void OnPointerUpButtonNo()
    {
        buttonNo.sprite = spritesOfButtonNo[0];
    }
    // public void OnPointerDownSettings()
    // {
    //     buttonSettings.sprite = spritesOfButtonSettings[1];
    // }
    // public void OnPointerUpSettings()
    // {
    //     buttonSettings.sprite = spritesOfButtonSettings[0];
    // }
    // public void OnPointerDownExit()
    // {
    //     buttonExit.sprite = spritesOfButtonExit[1];
    // }
    // public void OnPointerUpExit()
    // {
    //     buttonExit.sprite = spritesOfButtonExit[0];
    //     Application.Quit();
    // }

}
