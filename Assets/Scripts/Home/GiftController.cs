using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GiftController : MonoBehaviour
{
    [SerializeField] private GiftData giftData;
    [SerializeField] private HeroData heroData;
    [SerializeField] private GameObject heroGift;
    [SerializeField] private GameObject goldGift;
    [SerializeField] private GameObject claimed;
    [SerializeField] private GameObject notification;
    [SerializeField] private Image avatarHeroGift;
    [SerializeField] private Text infoGolfGift;
    [SerializeField] private Text infoHeroGift;
    [SerializeField] private Animator x2;
    [Header("Comfirmation")]
    [SerializeField] private GameObject confirm;
    [SerializeField] private HorizontalLayoutGroup goldGiftConfirmLayout;
    [SerializeField] private HorizontalLayoutGroup heroGiftConfirmLayout;
    [SerializeField] private Text quantityGoldConfirm;
    [SerializeField] private Image avatarHeroConfirm;
    [SerializeField] private Text lifeTimeHeroConfirm;
    int timesReceived;
    bool canIncreaseTimesReceived = false;
    private Transform panel;
    private Transform panelConfirm;
    private void Awake()
    {
        panel = transform.GetChild(1);
        panelConfirm = confirm.transform.GetChild(1);
        if (PlayerPrefs.GetInt("IsOpenedApp", 0) == 1)
        {
            DateTime time;
            if (PlayerPrefs.HasKey("FirstOpeningInDay"))
            {
                time = Convert.ToDateTime(PlayerPrefs.GetString("FirstOpeningInDay"));
            }
            else
            {
                time = DateTime.Now.AddDays(-1);
            }
            if (DateTime.Now.Year > time.Year || DateTime.Now.DayOfYear > time.DayOfYear)
            {
                panel.DOScale(Vector3.one, 0f);
                PlayerPrefs.SetInt("ReceivedGift", 0);
                claimed.SetActive(false);
                PlayerPrefs.SetString("FirstOpeningInDay", DateTime.Now.ToString());
                canIncreaseTimesReceived = true;
            }
            else
            {
                if (PlayerPrefs.GetInt("ReceivedGift", 0) == 1)
                {
                    x2.enabled = false;
                    claimed.SetActive(true);
                    notification.SetActive(false);
                }
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
            PlayerPrefs.SetInt("IsOpenedApp", 1);
        }
        timesReceived = PlayerPrefs.GetInt("TimesReceived", 0);
        if (timesReceived != 27 && timesReceived != 13)
        {
            timesReceived %= 7;
            goldGift.SetActive(true);
            infoGolfGift.text = "+" + giftData.GetGift(timesReceived).amountOfGolf;
        }
        else
        {
            if (timesReceived == 13)
                timesReceived = 7;
            else
                timesReceived = 8;
            heroGift.SetActive(true);
            infoHeroGift.text = "+" + giftData.GetGift(timesReceived).lifeTime + " DAYS";
            avatarHeroGift.sprite = heroData.GetHero(giftData.GetGift(timesReceived).idHero).avatar;
        }
    }
    private void OnEnable()
    {
        panel.DOScale(Vector3.one, 0.3f);
    }
    private void OnDisable()
    {
        panel.DOScale(Vector3.zero, 0.3f);
    }
    public void Claim(int x = 1)
    {
        Canvas.ForceUpdateCanvases();
        if (giftData.GetGift(timesReceived).isGold)
        {
            GameData.gold += giftData.GetGift(timesReceived).amountOfGolf * x;
            PlayerPrefs.SetInt("Gold", GameData.gold);
            quantityGoldConfirm.text = "+" + giftData.GetGift(timesReceived).amountOfGolf * x;
            goldGiftConfirmLayout.enabled = false;
            goldGiftConfirmLayout.enabled = true;
            goldGiftConfirmLayout.gameObject.SetActive(true);
        }
        else
        {
            int idHero = giftData.GetGift(timesReceived).idHero;
            avatarHeroConfirm.sprite = heroData.GetHero(idHero).avatar;
            lifeTimeHeroConfirm.text = "+" + giftData.GetGift(timesReceived).lifeTime * x + " DAYS";
            heroGiftConfirmLayout.enabled = false;
            heroGiftConfirmLayout.enabled = true;
            heroGiftConfirmLayout.gameObject.SetActive(true);
            bool canReceive = true;
            int[] purchasedHeroes = Array.ConvertAll(PlayerPrefs.GetString("PurchasedHeroes", "0").Split(","), int.Parse);
            for (int i = 0; i < purchasedHeroes.Length; i++)
            {
                if (purchasedHeroes[i] == idHero)
                {
                    canReceive = false;
                }
            }
            if (canReceive)
                PlayerPrefs.SetString("HeroGift", giftData.GetGift(timesReceived).idHero + "/"
                + DateTime.Now.AddDays(giftData.GetGift(timesReceived).lifeTime * x));
        }
        panel.gameObject.SetActive(false);
        confirm.SetActive(true);
        panelConfirm.DOScale(Vector3.one, 0.3f);
        PlayerPrefs.SetInt("ReceivedGift", 1);
        x2.enabled = false;
        claimed.SetActive(true);
        notification.SetActive(false);
        if (canIncreaseTimesReceived)
            PlayerPrefs.SetInt("TimesReceived", PlayerPrefs.GetInt("TimesReceived", 0) == 28 ? 0 : PlayerPrefs.GetInt("TimesReceived", 0) + 1);
        canIncreaseTimesReceived = false;
    }
    public void WatchAds()
    {
        MG_Interface.Current.Reward_Show((bool isSucceed) => { 
            if (isSucceed)
            {
                Claim(2);
            }
        });
    }
    public void Confirm()
    {
        confirm.SetActive(false);
        panelConfirm.DOScale(Vector3.zero, 0.3f);
        gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
    }
}
