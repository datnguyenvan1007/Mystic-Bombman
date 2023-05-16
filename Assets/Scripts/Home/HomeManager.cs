using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private Text highScore;
    private AudioSource audioSource;
    public static bool IsContinue {get; set;} = true;
    void Awake()
    {
        GameData.gold = PlayerPrefs.GetInt("Gold", 0);
        GameData.respawnLeft = PlayerPrefs.GetInt("RespawnLeft", 3);
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    
    public void Continue()
    {
        IsContinue = true;
    }
    public void NewGame()
    {
        IsContinue = false;
    }
    public void PlayGame() {
        if (!IsContinue) {
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
        }
        List<string> names = playerInfo.GetNameSelectedBooster();
        foreach (string name in names) {
            PlayerPrefs.SetInt(name + "Booster", 1);
        }
        SceneManager.LoadScene(1);
    }
}
