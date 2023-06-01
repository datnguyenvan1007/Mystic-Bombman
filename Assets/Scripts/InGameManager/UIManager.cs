using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem.LowLevel;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text time;
    [SerializeField] private Text score;
    [SerializeField] private Text left;
    [SerializeField] private Text stage;

    [SerializeField] private GameObject startingScene;
    [SerializeField] private GameObject playingScene;
    [SerializeField] private GameObject winScene;
    [SerializeField] private GameObject loseScene;
    [SerializeField] private Transform pause;
    [SerializeField] private Transform prompt;

    [SerializeField] private GameObject joystickControl;
    [SerializeField] private GameObject dpadControl;
    [SerializeField] private GameObject buttonExplode;

    [SerializeField] private RectTransform uiControlMovement;
    [SerializeField] private RectTransform uiControlBomb;
    [SerializeField] private List<Image> controllers;

    [SerializeField] private GameObject respawnPopup;
    [SerializeField] private Button respawnByGoldButton;
    [SerializeField] private Button respawnByWatchAdsButton;
    [SerializeField] private Button cancleRespawnButton;

    [SerializeField] private Text controlsText;
    [SerializeField] private Text flipControlText;
    [SerializeField] private Text soundText;

    [SerializeField] private Text respawnFee;
    [SerializeField] private Text respawnLeftText;
    [SerializeField] private Text contentCancleRespawn;
    [SerializeField] private GameObject reward;
    [SerializeField] private Text rewardText;

    [SerializeField] private GameObject pressUp;
    [SerializeField] private GameObject pressDown;
    [SerializeField] private GameObject pressLeft;
    [SerializeField] private GameObject pressRight;

    [SerializeField] private GameObject devButton;

    private List<Vector2> controllersPosition = new List<Vector2>();
    private float timer = 0f;
    private Vector2 oldMove = Vector2.zero;
    public static UIManager instance;
    private void Awake()
    {
        UIManager.instance = this;
        devButton.SetActive(MG_Interface.Current.devMode);
    }
    private void Start()
    {
        controllersPosition.Add(uiControlMovement.localPosition);
        controllersPosition.Add(uiControlBomb.localPosition);
        SetControllerOpacity(PlayerPrefs.GetFloat("ControllerOpacity", 45) / 100);
        SetControllerType(PlayerPrefs.GetInt("ControllerType", 2));
        SetActiveButtonDetonator(GameData.detonator);
        SetFlipControls(PlayerPrefs.GetInt("FlipControls", 0));
        if (PlayerPrefs.GetInt("Sound", 0) == 1)
        {
            soundText.text = "ON";
        }
        else
        {
            soundText.text = "OFF";
        }
    }
    private void FixedUpdate()
    {
        if (timer < 0)
            return;
        if (timer >= 0)
        {
            timer -= Time.fixedDeltaTime;
            contentCancleRespawn.text = "NO(" + Mathf.RoundToInt(timer) + ")";
        }
        if (timer < 0)
        {
            contentCancleRespawn.text = "NO";
            cancleRespawnButton.interactable = true;
        }
    }
    public void SetControllerOpacity(float a)
    {
        foreach (var c in controllers)
        {
            c.color = new Color(c.color.r, c.color.g, c.color.b, a);
        }
    }
    public void SetControllerType(int type)
    {
        if (type == 1)
        {
            dpadControl.SetActive(false);
            joystickControl.SetActive(true);
            controlsText.text = "JOYSTICK";
        }
        else
        {
            dpadControl.SetActive(true);
            joystickControl.SetActive(false);
            controlsText.text = "DPAD";
        }
    }
    public void SetFlipControls(int type)
    {
        if (type == 1)
        {
            uiControlMovement.localPosition = controllersPosition[1];
            uiControlBomb.localPosition = controllersPosition[0];
            flipControlText.text = "ON";
        }
        else
        {
            uiControlMovement.localPosition = controllersPosition[0];
            uiControlBomb.localPosition = controllersPosition[1];
            flipControlText.text = "OFF";
        }
    }
    public void SetActiveButtonDetonator(int type)
    {
        if (uiControlBomb.gameObject.activeSelf)
        {
            if (type == 1)
            {
                buttonExplode.SetActive(true);
            }
            else
            {
                buttonExplode.SetActive(false);
            }
        }
    }
    public void SetGameScore(int s)
    {
        GameData.score += s;
        score.text = GameData.score.ToString("#,0").Replace(",", ".");
    }
    public void SetTimeGame(int t)
    {
        time.text = t.ToString();
    }
    public void SetValueStageAndLeft(int stageValue, int leftValue)
    {
        stage.text = "STAGE " + stageValue;
        left.text = leftValue.ToString();
    }
    public void SetActivePlayingScene(bool isActive)
    {
        playingScene.SetActive(isActive);
    }
    public void SetActiveStartingScene(bool isActive)
    {
        startingScene.SetActive(isActive);
    }
    public void ActiveWinScene()
    {
        winScene.SetActive(true);
    }
    public void ActiveLoseScene()
    {
        loseScene.SetActive(true);
    }
    public void Pause()
    {
        pause.parent.gameObject.SetActive(true);
        AudioManager.instance.Mute();
        Time.timeScale = 0;
        pause.DOScale(Vector3.one, 0.3f).SetUpdate(true);
    }
    public void SelectControllerType()
    {
        if (controlsText.text == "JOYSTICK")
        {
            PlayerPrefs.SetInt("ControllerType", 2);
            controlsText.text = "DPAD";
            dpadControl.SetActive(true);
            joystickControl.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("ControllerType", 1);
            controlsText.text = "JOYSTICK";
            dpadControl.SetActive(false);
            joystickControl.SetActive(true);
        }
    }
    public void SelectFlipControls()
    {
        if (flipControlText.text == "ON")
        {
            PlayerPrefs.SetInt("FlipControls", 0);
            flipControlText.text = "OFF";
            uiControlMovement.localPosition = controllersPosition[0];
            uiControlBomb.localPosition = controllersPosition[1];
        }
        else
        {
            PlayerPrefs.SetInt("FlipControls", 1);
            flipControlText.text = "ON";
            uiControlMovement.localPosition = controllersPosition[1];
            uiControlBomb.localPosition = controllersPosition[0];
        }
    }
    public void SelectSound()
    {
        if (soundText.text == "ON")
        {
            PlayerPrefs.SetInt("Sound", 0);
            soundText.text = "OFF";
            AudioManager.instance.Mute();
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            soundText.text = "ON";
            AudioManager.instance.UnMute();
        }
    }
    public void DisplayHighLightDpad(Vector2 move)
    {
        if (oldMove == move)
            return;
        if (move == Vector2.down)
            pressDown.SetActive(true);
        if (move == Vector2.left)
            pressLeft.SetActive(true);
        if (move == Vector2.right)
            pressRight.SetActive(true);
        if (move == Vector2.up)
            pressUp.SetActive(true);

        if (oldMove == Vector2.down)
            pressDown.SetActive(false);
        if (oldMove == Vector2.left)
            pressLeft.SetActive(false);
        if (oldMove == Vector2.right)
            pressRight.SetActive(false);
        if (oldMove == Vector2.up)
            pressUp.SetActive(false);
        oldMove = move;
    }
    public void Continue()
    {
        Time.timeScale = 1;
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
            AudioManager.instance.UnMute();
        pause.parent.gameObject.SetActive(false);
        pause.DOScale(Vector3.zero, 0.3f);
    }
    public void MainMenu()
    {
        pause.gameObject.SetActive(false);
        /*pause.DOScale(Vector3.zero, 0.3f).SetUpdate(true);*/
        prompt.gameObject.SetActive(true);
        /*prompt.DOScale(Vector3.one, 0.3f).SetUpdate(true);*/
    }
    public void ReturnHome()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("RespawnLeft", GameData.respawnLeft);
        PlayerPrefs.SetInt("Gold", GameData.gold);
        SceneManager.LoadScene(0);
    }
    public void CancelReturn()
    {
        prompt.gameObject.SetActive(false);
        /*prompt.DOScale(Vector3.zero, 0.3f).SetUpdate(true);*/
        pause.gameObject.SetActive(true);
        /*pause.DOScale(Vector3.one, 0.3f).SetUpdate(true);*/
    }
    public bool GetActiveJoystick()
    {
        return joystickControl.activeSelf;
    }
    public void ShowPopupRespawn()
    {
        respawnFee.text = GameManager.instance.feeToRespawn.ToString();
        respawnLeftText.text = "(x" + GameData.respawnLeft + ")";
        if (GameData.gold < GameManager.instance.feeToRespawn)
        {
            respawnByGoldButton.interactable = false;
        }
        else
        {
            respawnByGoldButton.interactable = true;
        }
        cancleRespawnButton.interactable = false;
        respawnPopup.SetActive(true);
        timer = 10;
    }
    public void RespawnByGold()
    {
        GameData.gold -= GameManager.instance.feeToRespawn;
        Respawn();
    }
    public void RespawnByWatchAds()
    {
        MG_Interface.Current.Reward_Show(Respawn);
    }
    public void Respawn(bool isSucceed = true)
    {
        if (isSucceed)
        {
            GameData.respawnLeft--;
            GameManager.instance.IsPlayingLevel = false;
            PoolBrick.instance.DespawnAll();
            Destroy(GameManager.instance.EnemiesAndItemOfCurrentLevel);
            PlayerPrefs.SetInt("Left", 0);
            StartCoroutine(GameManager.instance.LoadLevel());
            respawnPopup.SetActive(false);
            PlayerPrefs.SetInt("RespawnLeft", GameData.respawnLeft);
        }
        else
        {
            GameManager.instance.GoToPreviousLevel();
        }
    }
    public void CancleRespawn()
    {
        respawnPopup.SetActive(false);
        StartCoroutine(GameManager.instance.GoToPreviousLevel());
    }
    public IEnumerator ShowReward()
    {
        rewardText.text = "+" + GameManager.instance.reward;
        reward.SetActive(true);
        reward.transform.DOScale(Vector3.one, 0.3f);
        yield return new WaitForSeconds(0.3f);
        reward.transform.DOScale(new Vector3(0.95f, 0.95f, 0.95f), 0.1f);
        yield return new WaitForSeconds(0.1f);
        reward.transform.DOScale(Vector3.one, 0.1f);
    }
    public void HideReward()
    {
        reward.transform.DOScale(Vector3.zero, 0f);
        reward.SetActive(false);
    }
}
