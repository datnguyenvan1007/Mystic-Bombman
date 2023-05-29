using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private Text highScore;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject removeAdsButton;
    public static HomeManager instance;
    private AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        instance = this;
        GameData.gold = PlayerPrefs.GetInt("Gold", 0);
        GameData.respawnLeft = PlayerPrefs.GetInt("RespawnLeft", 3);
        removeAdsButton.SetActive(!MG_PlayerPrefs.GetBool("RemoveAds", false));
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("Left"))
        {
            continueButton.SetActive(false);
        }
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString("#,0").Replace(",", ".");
        if (PlayerPrefs.GetInt("Sound", 0) == 0)
        {
            Mute();
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene(1);
    }
    public void NewGame()
    {
        if (PlayerPrefs.GetInt("HighScore", 0) < PlayerPrefs.GetInt("Score", 0))
            PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("Score", 0));
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
        GameData.respawnLeft = 3;
        SceneManager.LoadScene(1);
    }
    public void Mute()
    {
        audioSource.mute = true;
    }
    public void UnMute()
    {
        audioSource.mute = false;
    }
}
