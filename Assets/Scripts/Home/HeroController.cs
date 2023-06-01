using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject purchaseConfirmation;
    [SerializeField] private GameObject failedPurchaseByGold;
    [SerializeField] private GameObject failedPurchaseByCard;
    [SerializeField] private GameObject successfulPurchase;
    [Header("Data")]
    [SerializeField] private HeroData heroData;
    [Header("View")]
    [SerializeField] private Sprite selectedHero;
    [SerializeField] private Sprite unselectedHero;
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject objectContainer;
    [Header("Info")]
    [SerializeField] private new Text name;
    [SerializeField] private Text numberOfPriceInfo;
    [SerializeField] private Image avatarInfo;
    [SerializeField] private GameObject gold;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private HorizontalLayoutGroup priceHorizontalLayoutInfo;
    [Header("Purchase Comfirmation")]
    [SerializeField] private Text numberOfPricePurchase;
    [SerializeField] private Image avatarPurchase;
    [SerializeField] private GameObject goldPurchase;
    [SerializeField] private HorizontalLayoutGroup priceHorizontalLayoutPurchase;
    private GameObject oldHighLight = null;
    private Transform oldSelected = null;
    private Transform clickedGameObject = null;
    private Transform container_purchaseConfirmation;
    private Transform container_failedPurchaseByGold;
    private Transform container_failedPurchaseByCard;
    private Transform container_successfulPurchase;
    private int idOfSelectedObject;
    private GridLayoutGroup grid;
    List<GameObject> instancesHero = new List<GameObject>();
    Vector2 sizeDelta;
    List<int> purchasedHeroes;
    int idHeroGift = -1;
    int idHeroSkin;
    DateTime receivedDate;
    int daysLeft;
    private void CalculateContentSize()
    {
        sizeDelta = content.sizeDelta;
        grid = objectContainer.GetComponent<GridLayoutGroup>();
        int rowCount = Mathf.CeilToInt(heroData.Count / (grid.constraintCount * 1.0f));
        content.sizeDelta += new Vector2(0, rowCount * grid.cellSize.y + (rowCount - 1) * grid.spacing.y + grid.padding.top * 2);
    }
    private void LoadDataHeroes()
    {
        idHeroSkin = PlayerPrefs.GetInt("IdHero", 0);
        purchasedHeroes = Array.ConvertAll(PlayerPrefs.GetString("PurchasedHeroes", "0").Split(','), int.Parse).ToList();
        for (int i = 0; i < heroData.Count; i++)
        {
            instancesHero.Add(Instantiate(heroPrefab, objectContainer.transform));
            instancesHero[i].transform.GetChild(2).GetComponent<Text>().text
            = heroData.GetHero(i).name.ToString();
            instancesHero[i].transform.GetChild(3).GetComponent<Image>().sprite
            = heroData.GetHero(i).avatar;
            if (heroData.GetHero(i).currency != Currency.Gold)
            {
                instancesHero[i].transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
                instancesHero[i].transform.GetChild(4).GetChild(1).GetComponent<Text>().text = "$" + heroData.GetHero(i).price.ToString();
            }
            else
            {
                instancesHero[i].transform.GetChild(4).GetChild(1).GetComponent<Text>().text = heroData.GetHero(i).price.ToString();
            }
            Canvas.ForceUpdateCanvases();
            instancesHero[i].transform.GetChild(4).GetComponent<HorizontalLayoutGroup>().enabled = false;
            instancesHero[i].transform.GetChild(4).GetComponent<HorizontalLayoutGroup>().enabled = true;
            instancesHero[i].transform.GetChild(5).GetComponent<Text>().text
            = heroData.GetHero(i).id.ToString();
            int x = i;
            instancesHero[i].GetComponent<Button>().onClick.AddListener(delegate { ShowInfo(heroData.GetHero(x), instancesHero[x].transform); });
        }
        for (int i = 0; i < purchasedHeroes.Count; i++)
        {
            instancesHero[purchasedHeroes[i]].transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
            instancesHero[purchasedHeroes[i]].transform.GetChild(4).GetChild(1).GetComponent<Text>().text = "OWNED";
        }
    }
    private void Awake()
    {
        CalculateContentSize();
        LoadDataHeroes();
        container_purchaseConfirmation = purchaseConfirmation.transform.GetChild(1);
        container_successfulPurchase = successfulPurchase.transform.GetChild(1);
        container_failedPurchaseByGold = failedPurchaseByGold.transform.GetChild(1);
        container_failedPurchaseByCard = failedPurchaseByCard.transform.GetChild(1);
    }
    void OnEnable()
    {
        moneyText.text = GameData.gold.ToString("#,0").Replace(",", ".");
        if (PlayerPrefs.HasKey("HeroGift"))
        {
            string heroGiftInfo = PlayerPrefs.GetString("HeroGift");
            idHeroGift = Convert.ToInt32(heroGiftInfo.Substring(0, heroGiftInfo.IndexOf('/')));
            receivedDate = Convert.ToDateTime(heroGiftInfo.Substring(heroGiftInfo.IndexOf('/') + 1));
            daysLeft = (receivedDate - DateTime.Now).Days;
            if (daysLeft <= 0)
            {
                if (idHeroGift == idHeroSkin)
                {
                    PlayerPrefs.SetInt("IdHero", 0);
                    idHeroSkin = 0;
                    oldSelected.GetChild(1).GetComponent<Image>().sprite = unselectedHero;
                    instancesHero[0].transform.GetChild(1).GetComponent<Image>().sprite = selectedHero;
                    instancesHero[0].transform.GetChild(4).GetChild(1).GetComponent<Text>().text = "OWNED";
                    ShowInfo(heroData.GetHero(0), instancesHero[0].transform);
                    oldSelected = instancesHero[0].transform;
                }
                idHeroGift = -1;
            }
        }
        if (idHeroGift != -1)
        {
            instancesHero[idHeroGift].transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
            instancesHero[idHeroGift].transform.GetChild(4).GetChild(1).GetComponent<Text>().text =
            daysLeft + " " + (daysLeft == 1 ? "DAY" : "DAYS");
        }
        oldSelected = instancesHero[idHeroSkin].transform;
        instancesHero[idHeroSkin].transform.GetChild(1).GetComponent<Image>().sprite = selectedHero;
        ShowInfo(heroData.GetHero(idHeroSkin), instancesHero[idHeroSkin].transform);
        instancesHero[idHeroSkin].transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ShowInfo(Hero data, Transform tran)
    {
        if (oldHighLight != null)
        {
            oldHighLight.SetActive(false);
        }
        tran.GetChild(0).gameObject.SetActive(true);
        oldHighLight = tran.GetChild(0).gameObject;
        idOfSelectedObject = data.id;
        name.text = data.name;
        avatarInfo.sprite = data.avatar;
        if (data.id == idHeroGift)
        {
            gold.SetActive(false);
            numberOfPriceInfo.text = daysLeft + " " + (daysLeft == 1 ? "DAY" : "DAYS"); ;
            purchaseButton.gameObject.SetActive(false);
            selectButton.interactable = true;
            selectButton.gameObject.SetActive(true);
        }
        else
        {
            if (purchasedHeroes.Contains(idOfSelectedObject))
            {
                gold.SetActive(false);
                numberOfPriceInfo.text = "OWNED";
                if (data.id == idHeroSkin)
                {
                    selectButton.interactable = false;
                    purchaseButton.gameObject.SetActive(false);
                    selectButton.gameObject.SetActive(true);
                }
                else
                {
                    selectButton.interactable = true;
                    purchaseButton.gameObject.SetActive(false);
                    selectButton.gameObject.SetActive(true);
                }
            }
            else
            {
                if (data.currency != Currency.Gold)
                {
                    gold.SetActive(false);
                    numberOfPriceInfo.text = "$" + data.price.ToString();
                }
                else
                {
                    gold.SetActive(true);
                    numberOfPriceInfo.text = data.price.ToString();
                }
                purchaseButton.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
            }
        }
        Canvas.ForceUpdateCanvases();
        priceHorizontalLayoutInfo.enabled = false;
        priceHorizontalLayoutInfo.enabled = true;
        clickedGameObject = tran;
    }
    private void ShowSelectedHero()
    {
        oldSelected.GetChild(1).GetComponent<Image>().sprite = unselectedHero;
        clickedGameObject.GetChild(1).GetComponent<Image>().sprite = selectedHero;
        selectButton.interactable = false;
        clickedGameObject.GetChild(4).gameObject.SetActive(true);
    }
    public void Select()
    {
        PlayerPrefs.SetInt("IdHero", idOfSelectedObject);
        ShowSelectedHero();
        idHeroSkin = idOfSelectedObject;
        oldSelected = clickedGameObject;
    }
    public void Purchase()
    {
        avatarPurchase.sprite = heroData.GetHero(idOfSelectedObject).avatar;
        if (heroData.GetHero(idOfSelectedObject).currency == Currency.Gold)
        {
            numberOfPricePurchase.text = heroData.GetHero(idOfSelectedObject).price.ToString();
            goldPurchase.SetActive(true);
        }
        else
        {
            goldPurchase.SetActive(false);
            numberOfPricePurchase.text = "$" + heroData.GetHero(idOfSelectedObject).price.ToString();
        }
        Canvas.ForceUpdateCanvases();
        priceHorizontalLayoutPurchase.enabled = false;
        priceHorizontalLayoutPurchase.enabled = true;
        purchaseConfirmation.SetActive(true);
        container_purchaseConfirmation.DOScale(Vector3.one, 0.3f);
    }
    public void Cancel(Transform container)
    {
        container.DOScale(Vector3.zero, 0.3f);
        container.parent.gameObject.SetActive(false);
    }
    public void ConfirmPurchase()
    {
        if (heroData.GetHero(idOfSelectedObject).currency == Currency.Gold)
        {
            if (GameData.gold >= heroData.GetHero(idOfSelectedObject).price)
            {
                GameData.gold -= (int)heroData.GetHero(idOfSelectedObject).price;
                moneyText.text = GameData.gold.ToString("#,0").Replace(",", ".");
                PlayerPrefs.SetInt("Gold", GameData.gold);
                PurchaseSucceed();
            }
            else
            {
                purchaseConfirmation.SetActive(false);
                container_purchaseConfirmation.DOScale(Vector3.zero, 0.3f);
                failedPurchaseByGold.SetActive(true);
                container_failedPurchaseByGold.DOScale(Vector3.one, 0.3f);
            }
        }
        else
        {
            MG_Interface.Current.Purchase_Item(idOfSelectedObject.ToString(), (bool result, bool onIAP, string productId) =>
            {
                if (result)
                {
                    PurchaseSucceed();
                }
                else
                {
                    purchaseConfirmation.SetActive(false);
                    container_purchaseConfirmation.DOScale(Vector3.zero, 0.3f);
                    failedPurchaseByCard.SetActive(true);
                    container_failedPurchaseByCard.DOScale(Vector3.one, 0.3f);
                }
            });
        }
    }
    public void PurchaseSucceed()
    {
        if (!PlayerPrefs.HasKey("PurchasedHeroes"))
            PlayerPrefs.SetString("PurchasedHeroes", "0");
        PlayerPrefs.SetString("PurchasedHeroes", PlayerPrefs.GetString("PurchasedHeroes") + "," + idOfSelectedObject);
        purchasedHeroes.Add(idOfSelectedObject);
        purchaseConfirmation.SetActive(false);
        successfulPurchase.SetActive(true);
        clickedGameObject.GetChild(4).GetChild(0).gameObject.SetActive(false);
        clickedGameObject.GetChild(4).GetChild(1).GetComponent<Text>().text = "OWNED";
        gold.SetActive(false);
        numberOfPriceInfo.text = "OWNED";
        selectButton.interactable = true;
        selectButton.gameObject.SetActive(true);
        purchaseButton.gameObject.SetActive(false);
        container_successfulPurchase.DOScale(Vector3.one, 0.3f);
    }
}
