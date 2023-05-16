using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterShopController : ShopBase
{
    [SerializeField] private Text quantity;

    [SerializeField] private BoosterData boosterData;

    private int priceOfSelectedObject;
    private int quantityOfSelectedObject;
    protected override void Start()
    {
        base.Start();
        int rowCount = Mathf.CeilToInt(boosterData.Count / (grid.constraintCount * 1.0f));
        content.sizeDelta += new Vector2(0, (rowCount - 1) * (grid.cellSize.y + grid.spacing.y));
        for (int i = 0; i < boosterData.Count; i++)
        {
            instances.Add(Instantiate(objectPrefab, objectContainer.transform));
            instances[i].transform.GetChild(2).GetComponent<Image>().sprite
            = boosterData.GetBooster(i).avatar;
            instances[i].transform.GetChild(3).GetChild(0).GetComponent<Text>().text
            = boosterData.GetBooster(i).price.ToString();
            instances[i].transform.GetChild(4).GetComponent<Text>().text
            = boosterData.GetBooster(i).name.ToString();
            int index = i;
            instances[index].GetComponent<Button>().onClick.AddListener(delegate { ShowInfo(index, instances[index].transform); });
        }
    }
    public override void ShowInfo(int index, Transform tran)
    {
        Booster data = boosterData.GetBooster(index);
        quantityOfSelectedObject = 1;
        indexOfSelectedObject = index;
        priceOfSelectedObject = data.price;
        if (oldHighLight != null)
        {
            oldHighLight.SetActive(false);
        }
        tran.GetChild(0).gameObject.SetActive(true);
        oldHighLight = tran.GetChild(0).gameObject;
        if (!info.activeSelf)
            info.SetActive(true);
        name.text = data.name.ToString();
        image.sprite = data.avatar;
        numberOfPrice.text = data.price.ToString();
        quantity.text = "1";
    }
    public override void Confirm()
    {
        if (GameData.gold >= boosterData.GetBooster(indexOfSelectedObject).price * quantityOfSelectedObject)
        {
            PlayerPrefs.SetInt("Owned" + boosterData.GetBooster(indexOfSelectedObject).name + "Booster",
                PlayerPrefs.GetInt("Owned" + boosterData.GetBooster(indexOfSelectedObject).name + "Booster", 0) + quantityOfSelectedObject);
            GameData.gold -= boosterData.GetBooster(indexOfSelectedObject).price * quantityOfSelectedObject;
            gold.text = GameData.gold.ToString();
            PlayerPrefs.SetInt("Gold", GameData.gold);
        }
        else
        {
            confirm.SetActive(false);
            lackOfGold.SetActive(true);
        }
    }
    public void IncreaseQuantity()
    {
        quantityOfSelectedObject++;
        quantity.text = quantityOfSelectedObject.ToString();
        numberOfPrice.text = (priceOfSelectedObject * quantityOfSelectedObject).ToString();
    }
    public void DecreaseQuantity()
    {
        if (quantityOfSelectedObject == 1)
            return;
        quantityOfSelectedObject--;
        quantity.text = quantityOfSelectedObject.ToString();
        numberOfPrice.text = (priceOfSelectedObject * quantityOfSelectedObject).ToString();
    }
}
