using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject purchaseConfirmation;
    [SerializeField] private GameObject failedPurchase;
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
    private int idOfSeletedObject;
    private GridLayoutGroup grid;
    List<GameObject> instancesHero = new List<GameObject>();
    Vector2 sizeDelta;
    int[] purchasedHeroes;
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
    private void LoadComponents()
    {
        idHeroSkin = PlayerPrefs.GetInt("IdHero", 0);
        purchasedHeroes = Array.ConvertAll(PlayerPrefs.GetString("PurchasedHeroes", "0").Split(','), int.Parse);
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
        for (int i = 0; i < purchasedHeroes.Length; i++)
        {
            instancesHero[purchasedHeroes[i]].transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
            instancesHero[purchasedHeroes[i]].transform.GetChild(4).GetChild(1).GetComponent<Text>().text = "OWNED";
        }
        oldSelected = instancesHero[idHeroSkin].transform;
        instancesHero[idHeroSkin].transform.GetChild(1).GetComponent<Image>().sprite = selectedHero;
        ShowInfo(heroData.GetHero(idHeroSkin), instancesHero[idHeroSkin].transform);
        instancesHero[idHeroSkin].transform.GetChild(0).gameObject.SetActive(true);
    }
    private void Awake()
    {
        CalculateContentSize();
        LoadComponents();
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
    }
    public void ShowInfo(Hero data, Transform tran)
    {
        gold.SetActive(true);
        if (oldHighLight != null)
        {
            oldHighLight.SetActive(false);
        }
        tran.GetChild(0).gameObject.SetActive(true);
        oldHighLight = tran.GetChild(0).gameObject;
        idOfSeletedObject = data.id;
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
            if (tran.GetChild(4).GetChild(1).GetComponent<Text>().text == "OWNED")
            {
                gold.SetActive(false);
                numberOfPriceInfo.text = "OWNED";
                if (data.id == idHeroSkin)
                {
                    purchaseButton.gameObject.SetActive(false);
                    selectButton.interactable = false;
                    selectButton.gameObject.SetActive(true);
                }
                else
                {
                    purchaseButton.gameObject.SetActive(false);
                    selectButton.interactable = true;
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
        PlayerPrefs.SetInt("IdHero", idOfSeletedObject);
        ShowSelectedHero();
        idHeroSkin = idOfSeletedObject;
        oldSelected = clickedGameObject;
    }
    public void Purchase()
    {
        avatarPurchase.sprite = heroData.GetHero(idOfSeletedObject).avatar;
        if (heroData.GetHero(idOfSeletedObject).currency == Currency.Gold)
        {
            numberOfPricePurchase.text = heroData.GetHero(idOfSeletedObject).price.ToString();
            goldPurchase.SetActive(true);
        }
        else
        {
            goldPurchase.SetActive(false);
            numberOfPricePurchase.text = "$" + heroData.GetHero(idOfSeletedObject).price.ToString();
        }
        Canvas.ForceUpdateCanvases();
        priceHorizontalLayoutPurchase.enabled = false;
        priceHorizontalLayoutPurchase.enabled = true;
    }
    public void ConfirmPurchase()
    {
        if (heroData.GetHero(idOfSeletedObject).currency == Currency.Gold)
        {
            if (GameData.gold >= heroData.GetHero(idOfSeletedObject).price)
            {
                if (!PlayerPrefs.HasKey("PurchasedHeroes"))
                    PlayerPrefs.SetString("PurchasedHeroes", "0");
                PlayerPrefs.SetString("PurchasedHeroes", PlayerPrefs.GetString("PurchasedHeroes") + "," + idOfSeletedObject);
                GameData.gold -= (int)heroData.GetHero(idOfSeletedObject).price;
                moneyText.text = GameData.gold.ToString("#,0").Replace(",", ".");
                PlayerPrefs.SetInt("Gold", GameData.gold);
                purchaseConfirmation.SetActive(false);
                successfulPurchase.SetActive(true);
                clickedGameObject.GetChild(4).GetChild(0).gameObject.SetActive(false);
                clickedGameObject.GetChild(4).GetChild(1).GetComponent<Text>().text = "OWNED";
            }
            else
            {
                purchaseConfirmation.SetActive(false);
                failedPurchase.SetActive(true);
            }
        }
        else
        {

        }
    }
}
