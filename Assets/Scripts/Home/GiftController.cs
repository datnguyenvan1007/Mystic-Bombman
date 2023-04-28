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
    private int lastClaimedIndex = 0;
    private List<GameObject> instances = new List<GameObject>();
    private void Awake()
    {
        try
        {
            DateTime time = Convert.ToDateTime(giftData.GetGift(0).claimedTime);
            if (DateTime.Now.Year > time.Year)
            {
                giftData.ResetStatus();
            }
            if (DateTime.Now.Year == time.Year)
            {
                if (DateTime.Now.DayOfYear > time.DayOfYear)
                {
                    giftData.ResetStatus();
                }
            }
        }
        catch (Exception)
        {
            giftData.ResetStatus();
        }
        grid = content.GetComponent<GridLayoutGroup>();
        int rowCount = Mathf.CeilToInt(giftData.Count / (grid.constraintCount * 1.0f));
        content.sizeDelta += new Vector2(0, (rowCount) * (grid.cellSize.y + grid.spacing.y));
        for (int i = 0; i < giftData.Count; i++)
        {
            instances.Add(Instantiate(giftPrefab, content.transform));
            instances[i].transform.GetChild(0).GetComponent<Text>().text
            = giftData.GetGift(i).id.ToString();
            instances[i].transform.GetChild(3).GetComponent<Text>().text
            = "+" + giftData.GetGift(i).quantity;
            instances[i].transform.GetChild(4).GetChild(0).GetComponent<Text>().text = giftData.GetGift(i).status.ToString();
            if (giftData.GetGift(i).status == GiftStatus.Active)
                instances[i].transform.GetChild(4).GetComponent<Image>().color = activeColor;
            else
            {
                instances[i].transform.GetChild(4).GetComponent<Image>().color = inactiveColor;
                instances[i].transform.GetChild(4).GetComponent<Button>().interactable = false;
            }
            int index = i;
            instances[i].transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { Claim(index); });
        }
    }
    void Claim(int index)
    {
        giftData.GetGift(index).claimedTime = DateTime.Now.ToString();
        giftData.GetGift(index).status = GiftStatus.Claimed;
        instances[index].transform.GetChild(4).GetComponent<Image>().color = inactiveColor;
        instances[index].transform.GetChild(4).GetComponent<Button>().interactable = false;
        instances[index].transform.GetChild(4).GetChild(0).GetComponent<Text>().text = giftData.GetGift(index).status.ToString();
        confirm.SetActive(true);
        confirm.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Text>().text = "+" + giftData.GetGift(index).quantity;
        GameData.gold += giftData.GetGift(index).quantity;
        minute = 15;
        second = 0;
        if (index == instances.Count - 1)
            return;
        timerText = instances[index + 1].transform.GetChild(4).GetChild(0).GetComponent<Text>();
        lastClaimedIndex = index;
    }
    void OnEnable()
    {
        for (int i = 0; i < giftData.Count; i++)
        {
            if (giftData.GetGift(i).status != GiftStatus.Claimed)
            {
                lastClaimedIndex = i - 1;
                if (lastClaimedIndex == -1)
                    return;
                DateTime previousTime = Convert.ToDateTime(giftData.GetGift(lastClaimedIndex).claimedTime).AddMinutes(15);
                if (previousTime <= DateTime.Now)
                {
                    ActiveNextGift(lastClaimedIndex);
                }
                else
                {
                    timerText = instances[lastClaimedIndex + 1].transform.GetChild(4).GetChild(0).GetComponent<Text>();
                    minute = previousTime.Minute - DateTime.Now.Minute;
                    second = previousTime.Second - DateTime.Now.Second;
                    timerText.text = minute + ":" + second;
                }
                break;
            }
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
        giftData.GetGift(index + 1).status = GiftStatus.Active;
        instances[index + 1].transform.GetChild(4).GetComponent<Image>().color = activeColor;
        instances[index + 1].transform.GetChild(4).GetComponent<Button>().interactable = true;
        instances[index + 1].transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "Active";
    }
}
