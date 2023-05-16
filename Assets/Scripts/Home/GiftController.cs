using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftController : MonoBehaviour
{
    [SerializeField] private GiftData giftData;
    [SerializeField] private GameObject giftPrefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject confirm;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;
    private GridLayoutGroup grid;
    private int minute = 0;
    private float second = 0f;
    private Text timerText;
    private int lastClaimedIndex = -1;
    private List<GameObject> instances = new List<GameObject>();
    private void Awake()
    {
        // PlayerPrefs.SetInt("IndexGiftActive", 0);
        //             PlayerPrefs.DeleteKey("ClaimedTime");
        try
        {
            DateTime time = Convert.ToDateTime(PlayerPrefs.GetString("ClaimedTime"));
            if (DateTime.Now.Year > time.Year)
            {
                PlayerPrefs.SetInt("IndexGiftActive", 0);
                PlayerPrefs.DeleteKey("ClaimedTime");
            }
            if (DateTime.Now.Year == time.Year)
            {
                if (DateTime.Now.DayOfYear > time.DayOfYear)
                {
                    PlayerPrefs.SetInt("IndexGiftActive", 0);
                    PlayerPrefs.DeleteKey("ClaimedTime");
                }
            }
        }
        catch (Exception)
        {
        }
        grid = content.GetComponent<GridLayoutGroup>();
        int rowCount = Mathf.CeilToInt(giftData.Count / (grid.constraintCount * 1.0f));
        content.sizeDelta += new Vector2(0, (rowCount) * (grid.cellSize.y + grid.spacing.y));
        int indexGiftActive = PlayerPrefs.GetInt("IndexGiftActive", 0);
        for (int i = 0; i < giftData.Count; i++)
        {
            instances.Add(Instantiate(giftPrefab, content.transform));
            instances[i].transform.GetChild(0).GetComponent<Text>().text
            = giftData.GetGift(i).id.ToString();
            instances[i].transform.GetChild(3).GetComponent<Text>().text
            = "+" + giftData.GetGift(i).quantity;
            if (i == indexGiftActive && indexGiftActive >= 0)
            {
                instances[i].transform.GetChild(4).GetComponent<Image>().color = activeColor;
                instances[i].transform.GetChild(4).GetChild(0).GetComponent<Text>().text = GiftStatus.Active.ToString();
            }
            else
            {
                if (i < Mathf.Abs(indexGiftActive))
                    instances[i].transform.GetChild(4).GetChild(0).GetComponent<Text>().text = GiftStatus.Claimed.ToString();
                if (i > Mathf.Abs(indexGiftActive))
                    instances[i].transform.GetChild(4).GetChild(0).GetComponent<Text>().text = GiftStatus.Lock.ToString();
                instances[i].transform.GetChild(4).GetComponent<Image>().color = inactiveColor;
                instances[i].transform.GetChild(4).GetComponent<Button>().interactable = false;
            }
            int index = i;
            instances[i].transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { Claim(index); });
        }
        if (Mathf.Abs(indexGiftActive) > 0)
            lastClaimedIndex = Mathf.Abs(indexGiftActive) - 1;
        try
        {
            DateTime time = Convert.ToDateTime(PlayerPrefs.GetString("FirstOpening"));
            if (DateTime.Now.Year == time.Year)
            {
                if (DateTime.Now.DayOfYear == time.DayOfYear)
                {
                    gameObject.SetActive(false);
                    PlayerPrefs.SetString("FirstOpening", DateTime.Now.ToString());
                }
            }
        }
        catch (Exception)
        {
            PlayerPrefs.SetString("FirstOpening", DateTime.Now.ToString());
        }
    }
    void Claim(int index)
    {
        PlayerPrefs.SetString("ClaimedTime", DateTime.Now.ToString());
        PlayerPrefs.SetInt("IndexGiftActive", -index - 1);
        instances[index].transform.GetChild(4).GetComponent<Image>().color = inactiveColor;
        instances[index].transform.GetChild(4).GetComponent<Button>().interactable = false;
        instances[index].transform.GetChild(4).GetChild(0).GetComponent<Text>().text = GiftStatus.Claimed.ToString();
        confirm.SetActive(true);
        confirm.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Text>().text = "+" + giftData.GetGift(index).quantity;
        GameData.gold += giftData.GetGift(index).quantity;
        PlayerPrefs.SetInt("Gold", GameData.gold);
        if (index == instances.Count - 1)
            return;
        minute = 15;
        second = 0;
        timerText = instances[index + 1].transform.GetChild(4).GetChild(0).GetComponent<Text>();
        lastClaimedIndex = index;
    }
    void OnEnable()
    {
        // Debug.Log(PlayerPrefs.GetInt("IndexGiftActive", 0));
        // Debug.Log(PlayerPrefs.GetString("ClaimedTime"));
        if (lastClaimedIndex == -1 || lastClaimedIndex == giftData.Count - 1 || PlayerPrefs.GetInt("IndexGiftActive", 0) >= 0)
            return;
        try
        {
            DateTime previousTime = Convert.ToDateTime(PlayerPrefs.GetString("ClaimedTime")).AddMinutes(15);
            if (previousTime <= DateTime.Now)
            {
                ActiveNextGift(lastClaimedIndex);
            }
            else
            {
                instances[lastClaimedIndex + 1].transform.GetChild(4).GetComponent<Image>().color = inactiveColor;
                instances[lastClaimedIndex + 1].transform.GetChild(4).GetComponent<Button>().interactable = false;
                timerText = instances[lastClaimedIndex + 1].transform.GetChild(4).GetChild(0).GetComponent<Text>();
                TimeSpan substract = previousTime - DateTime.Now;
                minute = substract.Minutes;
                second = substract.Seconds;
                timerText.text = minute + ":" + second;
            }
        }
        catch (Exception)
        {
            // Debug.Log(e.Message);
            minute = 0;
            second = 0;
        }

    }
    void FixedUpdate()
    {
        if (minute <= 0 && second <= 0f)
            return;
        if (second <= 0)
        {
            second = 59;
            minute--;
        }
        timerText.text = minute + ":" + Mathf.RoundToInt(second);
        second -= Time.fixedDeltaTime;
        if (minute <= 0 && second <= 0f)
            ActiveNextGift(lastClaimedIndex);
    }
    void ActiveNextGift(int index)
    {
        PlayerPrefs.SetInt("IndexGiftActive", Mathf.Abs(PlayerPrefs.GetInt("IndexGiftActive", 0)));
        instances[index + 1].transform.GetChild(4).GetComponent<Image>().color = activeColor;
        instances[index + 1].transform.GetChild(4).GetComponent<Button>().interactable = true;
        instances[index + 1].transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "Active";
    }
}
