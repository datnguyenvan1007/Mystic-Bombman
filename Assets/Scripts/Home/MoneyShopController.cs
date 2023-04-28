using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyShopController : ShopBase
{
    [Header("Quantity")]
    [SerializeField] private Image currency;
    [SerializeField] private Text numberOfQuantity;
    [SerializeField] private MoneyData moneyData;
    protected override void Start()
    {
        base.Start();
        int rowCount = Mathf.CeilToInt(moneyData.Count / (grid.constraintCount * 1.0f));
        content.sizeDelta += new Vector2(0, (rowCount - 1) * (grid.cellSize.y + grid.spacing.y));
        for (int i = 0; i < moneyData.Count; i++)
        {
            instances.Add(Instantiate(objectPrefab, objectContainer.transform));
            instances[i].transform.GetChild(2).GetComponent<Image>().sprite
            = moneyData.GetMoney(i).avatar;
            if (moneyData.GetMoney(i).price != 0) {
                instances[i].transform.GetChild(3).GetChild(0).GetComponent<Text>().text
                = moneyData.GetMoney(i).price + " đ";
            }
            else {
                instances[i].transform.GetChild(3).GetChild(0).GetComponent<Text>().text
                = "Free";
            }
            instances[i].transform.GetChild(4).GetComponent<Text>().text
            = moneyData.GetMoney(i).name.ToString();
            instances[i].transform.GetChild(5).GetChild(1).GetComponent<Text>().text
            = "+" + moneyData.GetMoney(i).quantity.ToString();
            instances[i].transform.GetChild(6).GetComponent<Text>().text
            = moneyData.GetMoney(i).id.ToString();
            int index = i;
            instances[index].GetComponent<Button>().onClick.AddListener(delegate { ShowInfo(index, instances[index].transform); });

        }
    }
    public override void ShowInfo(int index, Transform tran)
    {
        Money data = moneyData.GetMoney(index);
        if (oldHighLight != null)
        {
            oldHighLight.SetActive(false);
        }
        tran.GetChild(0).gameObject.SetActive(true);
        oldHighLight = tran.GetChild(0).gameObject;
        if (!info.activeSelf)
            info.SetActive(true);
        indexOfSelectedObject = index;
        name.text = data.name;
        image.sprite = data.avatar;
        numberOfQuantity.text = "+" + data.quantity.ToString();
        if (data.price == 0)
        {
            numberOfPrice.text = "Watch Ads";
        }
        else
        {
            numberOfPrice.text = data.price.ToString() + " đ";
        }
    }
    public override void Confirm()
    {
        GameData.gold += moneyData.GetMoney(indexOfSelectedObject).quantity;
        gold.text = GameData.gold.ToString();
    }
}
