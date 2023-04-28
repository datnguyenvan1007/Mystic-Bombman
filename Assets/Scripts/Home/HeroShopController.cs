using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HeroShopController : ShopBase
{
    [SerializeField] private HeroData heroData;

    protected override void Start()
    {
        base.Start();
        int rowCount = Mathf.CeilToInt(heroData.Count / (grid.constraintCount * 1.0f));
        content.sizeDelta += new Vector2(0, rowCount * (grid.cellSize.y + grid.spacing.y));
        for (int i = 0; i < heroData.Count; i++)
        {
            instances.Add(Instantiate(objectPrefab, objectContainer.transform));
            instances[i].transform.GetChild(2).GetComponent<Image>().sprite
            = heroData.GetHero(i).avatar;
            instances[i].transform.GetChild(3).GetComponent<Text>().text
            = heroData.GetHero(i).name;
            instances[i].transform.GetChild(4).GetComponent<Text>().text
            = heroData.GetHero(i).price.ToString();
            instances[i].transform.GetChild(5).GetComponent<Text>().text
            = heroData.GetHero(i).currency.ToString();
            instances[i].transform.GetChild(6).GetComponent<Text>().text
            = heroData.GetHero(i).id.ToString();
            int x = i;
            instances[x].GetComponent<Button>().onClick.AddListener(delegate { ShowInfo(x, instances[x].transform); });
        }
    }
    public override void ShowInfo(int index, Transform tran)
    {
        Hero data = heroData.GetHero(index);
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
        if (data.price == 0)
        {
            numberOfPrice.gameObject.SetActive(false);
        }
        else
        {
            numberOfPrice.gameObject.SetActive(true);
            numberOfPrice.text = data.price.ToString();
        }
    }
    public override void Confirm()
    {
        if (GameData.gold >= heroData.GetHero(indexOfSelectedObject).price)
        {
            heroData.GetHero(indexOfSelectedObject).isPurchased = true;
            GameData.gold -= heroData.GetHero(indexOfSelectedObject).price;
            gold.text = GameData.gold.ToString();
        }
        else
        {
            confirm.SetActive(false);
            lackOfGold.SetActive(true);
        }
    }
}
