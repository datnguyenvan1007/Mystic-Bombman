using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldShopController : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private MoneyData moneyData;
    [SerializeField] private GameObject goldShopPrefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject objectContainer;
    [SerializeField] private Sprite backActiveSprite;
    [SerializeField] private Sprite backInActiveSprite;
    [Header("Comfirmation")]
    [SerializeField] private GameObject purchaseConfirmation;
    [SerializeField] private Text priceConfirmation;
    [SerializeField] private Text quantityConfirmation;
    [SerializeField] private Image avatarConfirmation;
    [SerializeField] private GameObject bonus;
    [SerializeField] private Text amountBonus;
    [Header("Notification")]
    [SerializeField] private GameObject notification;
    [SerializeField] private Text messageNotification;
    [SerializeField] private string failedMessage;
    [SerializeField] private string failedPurchaseByCardMessage;
    [SerializeField] private string successfulMessage;
    [Header("BestDeal")]
    [SerializeField] private HorizontalLayoutGroup quantityBestDealLayout;
    [SerializeField] private HorizontalLayoutGroup bonusBestDealLayout;
    [SerializeField] private Text numberOfQuantityBestDeal;
    [SerializeField] private Text numberOfBonusBestDeal;
    [SerializeField] private Text priceOfBestDeal;
    [SerializeField] private Image avatarBestDeal;
    private List<GameObject> instances = new List<GameObject>();
    private GridLayoutGroup grid;
    private int idOfSelectedObject;
    private Text priceFreeGold = null;
    private Image backFreeGold = null;
    private int remainingMinutes = 0;
    private float remainingSeconds = 0f;
    private Transform container_purchaseConfirm;
    private Transform container_notification;
    private void Awake()
    {
        container_purchaseConfirm = purchaseConfirmation.transform.GetChild(1);
        container_notification = notification.transform.GetChild(1);
        grid = objectContainer.GetComponent<GridLayoutGroup>();
        int rowCount = Mathf.CeilToInt(moneyData.Count / (grid.constraintCount * 1.0f));
        content.sizeDelta += new Vector2(0, (rowCount) * (grid.cellSize.y + grid.spacing.y));
        for (int i = 0; i < moneyData.Count - 1; i++)
        {
            instances.Add(Instantiate(goldShopPrefab, objectContainer.transform));
            instances[i].transform.GetChild(1).GetComponent<Text>().text
            = moneyData.GetMoney(i).name.ToString();
            instances[i].transform.GetChild(2).GetComponent<Text>().text
            = "+" + moneyData.GetMoney(i).amount.ToString();
            instances[i].transform.GetChild(3).GetComponent<Image>().sprite
            = moneyData.GetMoney(i).avatar;
            if (moneyData.GetMoney(i).price != 0)
            {
                instances[i].transform.GetChild(4).GetComponent<Text>().text
                = "$" + moneyData.GetMoney(i).price.ToString();
            }
            else
            {
                instances[i].transform.GetChild(4).GetComponent<Text>().text
                = "FREE";
            }
            instances[i].transform.GetChild(5).GetComponent<Text>().text
            = moneyData.GetMoney(i).id.ToString();
            int x = i;
            instances[i].GetComponent<Button>().onClick.AddListener(delegate { Purchase(moneyData.GetMoney(x).id); });
        }
        //Best Deal
        numberOfBonusBestDeal.text = "+" + moneyData.GetMoney(moneyData.Count - 1).bonus.ToString();
        numberOfQuantityBestDeal.text = "+" + moneyData.GetMoney(moneyData.Count - 1).amount.ToString();
        avatarBestDeal.sprite = moneyData.GetMoney(moneyData.Count - 1).avatar;
        priceOfBestDeal.text = "$" + moneyData.GetMoney(moneyData.Count - 1).price.ToString();
        Canvas.ForceUpdateCanvases();
        quantityBestDealLayout.enabled = false;
        quantityBestDealLayout.enabled = true;
        bonusBestDealLayout.enabled = false;
        bonusBestDealLayout.enabled = true;
        if (moneyData.Count > 0)
        {
            priceFreeGold = instances[0].transform.GetChild(4).GetComponent<Text>();
            backFreeGold = instances[0].transform.GetChild(0).GetComponent<Image>();
        }
    }
    void OnEnable()
    {
        moneyText.text = GameData.gold.ToString("#,0").Replace(",", ".");
        if (instances.Count > 0)
        {
            if (!PlayerPrefs.HasKey("LastReceivedTime"))
                return;
            DateTime lastReceivedTime = Convert.ToDateTime(PlayerPrefs.GetString("LastReceivedTime"));
            DateTime time = lastReceivedTime.AddMinutes(15);
            if (DateTime.Now >= time)
                return;
            remainingMinutes = (time - DateTime.Now).Minutes;
            remainingSeconds = (time - DateTime.Now).Seconds;
            priceFreeGold.text = remainingMinutes + ":" + Mathf.RoundToInt(remainingSeconds);
            backFreeGold.sprite = backInActiveSprite;
            instances[0].GetComponent<Button>().interactable = false;
        }
    }
    private void FixedUpdate()
    {
        if (remainingMinutes == 0 && remainingSeconds < 0.3f)
            return;
        priceFreeGold.text = remainingMinutes + ":" + Mathf.RoundToInt(remainingSeconds);
        remainingSeconds -= Time.fixedDeltaTime;
        if (Mathf.RoundToInt(remainingSeconds) == 0)
        {
            if (remainingMinutes == 0)
            {
                priceFreeGold.text = "FREE";
                backFreeGold.sprite = backActiveSprite;
                instances[0].GetComponent<Button>().interactable = true;
            }
            else
            {
                remainingMinutes--;
                remainingSeconds = 59;
            }
        }
    }
    private void Purchase(int id)
    {
        idOfSelectedObject = id;
        quantityConfirmation.text = "+" + (moneyData.GetMoney(id).amount + moneyData.GetMoney(id).bonus).ToString();
        if (moneyData.GetMoney(id).price == 0f)
            priceConfirmation.text = "WATCH ADS";
        else
            priceConfirmation.text = "$" + moneyData.GetMoney(id).price.ToString();
        avatarConfirmation.sprite = moneyData.GetMoney(id).avatar;
        bonus.SetActive(false);
        purchaseConfirmation.SetActive(true);
        container_purchaseConfirm.DOScale(Vector3.one, 0.3f);
    }
    public void PurchaseBestDeal()
    {
        idOfSelectedObject = moneyData.GetMoney(moneyData.Count - 1).id;
        quantityConfirmation.text = "+" + moneyData.GetMoney(moneyData.Count - 1).amount.ToString();
        priceConfirmation.text = "$" + moneyData.GetMoney(idOfSelectedObject).price.ToString();
        avatarConfirmation.sprite = moneyData.GetMoney(idOfSelectedObject).avatar;
        amountBonus.text = "+" + moneyData.GetMoney(moneyData.Count - 1).bonus.ToString();
        bonus.SetActive(true);
        purchaseConfirmation.SetActive(true);
        container_purchaseConfirm.DOScale(Vector3.one, 0.3f);
    }
    public void ConfirmPurchase()
    {
        if (moneyData.GetMoney(idOfSelectedObject).price == 0f)
        {
            MG_Interface.Current.Reward_Show((bool isSucceed) =>
            {
                if (isSucceed)
                {
                    GameData.gold += moneyData.GetMoney(idOfSelectedObject).amount + moneyData.GetMoney(idOfSelectedObject).bonus;
                    moneyText.text = GameData.gold.ToString("#,0").Replace(",", ".");
                    PlayerPrefs.SetInt("Gold", GameData.gold);
                    PlayerPrefs.SetString("LastReceivedTime", DateTime.Now.ToString());
                    remainingMinutes = 15;
                    remainingSeconds = 0;
                    priceFreeGold.text = remainingMinutes + ":" + Mathf.RoundToInt(remainingSeconds);
                    backFreeGold.sprite = backInActiveSprite;
                    instances[0].GetComponent<Button>().interactable = false;
                }
            });
        }
        else
        {
            MG_Interface.Current.Purchase_Item(idOfSelectedObject.ToString(), (bool result, bool onIAP, string productId) =>
            {
                purchaseConfirmation.SetActive(false);
                if (result)
                {
                    GameData.gold += moneyData.GetMoney(idOfSelectedObject).amount + moneyData.GetMoney(idOfSelectedObject).bonus;
                    moneyText.text = GameData.gold.ToString("#,0").Replace(",", ".");
                    PlayerPrefs.SetInt("Gold", GameData.gold);
                    messageNotification.text = successfulMessage;
                    notification.SetActive(true);
                    container_notification.DOScale(Vector3.one, 0.3f);
                }
                else
                {
                    messageNotification.text = failedPurchaseByCardMessage;
                    notification.SetActive(true);
                    container_notification.GetChild(1).DOScale(Vector3.one, 0.3f);
                }
            });
        }
        purchaseConfirmation.SetActive(false);
        container_purchaseConfirm.DOScale(Vector3.zero, 0.3f);
    }
    public void CancelPurchase()
    {
        container_purchaseConfirm.DOScale(Vector3.zero, 0.3f);
        purchaseConfirmation.SetActive(false);
    }
    public void Confirm()
    {
        notification.SetActive(false);
        container_notification.DOScale(Vector3.zero, 0.3f);
    }
}
