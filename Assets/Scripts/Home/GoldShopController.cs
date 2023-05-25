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
    [SerializeField] private GameObject purchaseConfirmation;
    [SerializeField] private Text price;
    [SerializeField] private Image avatar;
    [SerializeField] private HorizontalLayoutGroup quantityBestDealLayout;
    [SerializeField] private HorizontalLayoutGroup bonusBestDealLayout;
    [SerializeField] private Text numberOfQuantityBestDeal;
    [SerializeField] private Text numberOfBonusBestDeal;
    private List<GameObject> instances = new List<GameObject>();
    private GridLayoutGroup grid;
    private int idOfSelectedObject;
    void OnEnable()
    {
        moneyText.text = GameData.gold.ToString("#,0").Replace(",", ".");
    }
    private void Start()
    {
        grid = objectContainer.GetComponent<GridLayoutGroup>();
        int rowCount = Mathf.CeilToInt(moneyData.Count / (grid.constraintCount * 1.0f));
        content.sizeDelta += new Vector2(0, (rowCount) * (grid.cellSize.y + grid.spacing.y));
        for (int i = 0; i < moneyData.Count - 1; i++)
        {
            instances.Add(Instantiate(goldShopPrefab, objectContainer.transform));
            instances[i].transform.GetChild(1).GetComponent<Text>().text
            = moneyData.GetMoney(i).name.ToString();
            instances[i].transform.GetChild(2).GetComponent<Text>().text
            = "+" + moneyData.GetMoney(i).quantity.ToString();
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
        numberOfBonusBestDeal.text = "+" + moneyData.GetMoney(moneyData.Count - 1).bonus.ToString();
        numberOfQuantityBestDeal.text = "+" + moneyData.GetMoney(moneyData.Count - 1).quantity.ToString();
        Canvas.ForceUpdateCanvases();
        quantityBestDealLayout.enabled = false;
        quantityBestDealLayout.enabled = true;
        bonusBestDealLayout.enabled = false;
        bonusBestDealLayout.enabled = true;
    }
    private void Purchase(int id)
    {
        idOfSelectedObject = id;
        purchaseConfirmation.SetActive(true);
        if (moneyData.GetMoney(id).price == 0)
            price.text = "WATCH ADS";
        else
            price.text = "$" + moneyData.GetMoney(id).price.ToString();
    }
    public void ConfirmPurchase()
    {
        GameData.gold += moneyData.GetMoney(idOfSelectedObject).quantity;
        moneyText.text = GameData.gold.ToString("#,0").Replace(",", ".");
        PlayerPrefs.SetInt("Gold", GameData.gold);
    }
}
