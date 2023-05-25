using System;
using System.Collections.Generic;
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
    [Header("Comfirmation")]
    [SerializeField] private HorizontalLayoutGroup goldGiftConfirmLayout;
    [SerializeField] private HorizontalLayoutGroup heroGiftConfirmLayout;
    [SerializeField] private Text quantityGoldConfirm;
    [SerializeField] private Image avatarHeroConfirm;
    [SerializeField] private Text lifeTimeHeroConfirm;
    int timesReceived;
    bool canIncreaseTimesReceived = false;
    private void Awake()
    {
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
                PlayerPrefs.SetInt("ReceivedGift", 0);
                claimed.SetActive(false);
                PlayerPrefs.SetString("FirstOpeningInDay", DateTime.Now.ToString());
                canIncreaseTimesReceived = true;
            }
            else
            {
                if (PlayerPrefs.GetInt("ReceivedGift", 0) == 1)
                {
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
        if (timesReceived != 27)
        {
            timesReceived %= 7;
            goldGift.SetActive(true);
            infoGolfGift.text = "+" + giftData.GetGift(timesReceived).amountOfGolf;
        }
        else
        {
            heroGift.SetActive(true);
            infoHeroGift.text = "+" + giftData.GetGift(timesReceived).lifeTime + " DAYS";
            avatarHeroGift.sprite = heroData.GetHero(giftData.GetGift(timesReceived).idHero).avatar;
        }
    }
    public void Claim()
    {
        Canvas.ForceUpdateCanvases();
        if (giftData.GetGift(timesReceived).isGold)
        {
            GameData.gold += giftData.GetGift(timesReceived).amountOfGolf;
            PlayerPrefs.SetInt("Gold", GameData.gold);
            quantityGoldConfirm.text = "+" + giftData.GetGift(timesReceived).amountOfGolf;
            goldGiftConfirmLayout.enabled = false;
            goldGiftConfirmLayout.enabled = true;
            goldGiftConfirmLayout.gameObject.SetActive(true);
        }
        else
        {
            int idHero = giftData.GetGift(timesReceived).idHero;
            avatarHeroConfirm.sprite = heroData.GetHero(idHero).avatar;
            lifeTimeHeroConfirm.text = "+" + giftData.GetGift(timesReceived).lifeTime + " DAYS";
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
                + DateTime.Now.AddDays(giftData.GetGift(timesReceived).lifeTime));
        }
        PlayerPrefs.SetInt("ReceivedGift", 1);
        claimed.SetActive(true);
        notification.SetActive(false);
        if (canIncreaseTimesReceived)
            PlayerPrefs.SetInt("TimesReceived", PlayerPrefs.GetInt("TimesReceived", 0) == 28 ? 0 : PlayerPrefs.GetInt("TimesReceived", 0) + 1);
        canIncreaseTimesReceived = false;
    }
}
