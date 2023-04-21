using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDataItems : MonoBehaviour
{
    [SerializeField] private GameObject info;
    [SerializeField] private Text id;
    [Header("Name")]
    [SerializeField] private new Text name;
    [SerializeField] private Image image;
    [Header("Quantity")]
    [SerializeField] private Image currency;
    [SerializeField] private Text numberOfQuantity;
    [Header("Price")]
    [SerializeField] private Image type;
    [SerializeField] private Text numberOfPrice;
    [SerializeField] private GameObject watchAds;
    [SerializeField] private ItemData gems;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private RectTransform content;
    private GameObject oldHighLight = null;
    private GridLayoutGroup grid;
    List<GameObject> instancesItem = new List<GameObject>();
    void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
        int rowCount = Mathf.CeilToInt(gems.Count / (grid.constraintCount * 1.0f));
        content.sizeDelta += new Vector2(0, (rowCount - 1) * (grid.cellSize.y + grid.spacing.y));
        for (int i = 0; i < gems.Count; i++)
        {
            instancesItem.Add(Instantiate(itemPrefab, transform));
            instancesItem[i].transform.GetChild(2).GetComponent<Image>().sprite
            = gems.GetItem(i).avatar;
            instancesItem[i].transform.GetChild(3).GetChild(0).GetComponent<Text>().text
            = gems.GetItem(i).price.ToString();
            instancesItem[i].transform.GetChild(4).GetComponent<Text>().text
            = gems.GetItem(i).name.ToString();
            instancesItem[i].transform.GetChild(5).GetChild(1).GetComponent<Text>().text
            = "+" + gems.GetItem(i).quantity.ToString();
            instancesItem[i].transform.GetChild(6).GetComponent<Text>().text
            = gems.GetItem(i).id.ToString();
            int index = i;
            instancesItem[index].GetComponent<Button>().onClick.AddListener(delegate { ShowInfo(gems.GetItem(index), instancesItem[index].transform); });

        }
    }
    public void ShowInfo(Item data, Transform tran)
    {
        if (oldHighLight != null)
        {
            oldHighLight.SetActive(false);
        }
        tran.GetChild(0).gameObject.SetActive(true);
        oldHighLight = tran.GetChild(0).gameObject;
        if (!info.activeSelf)
            info.SetActive(true);
        id.text = data.id;
        name.text = data.name;
        image.sprite = data.avatar;
        numberOfQuantity.text = "+" + data.quantity.ToString();
        if (data.price == 0)
        {
            watchAds.SetActive(true);
            type.gameObject.SetActive(false);
            numberOfPrice.gameObject.SetActive(false);
        }
        else
        {
            watchAds.SetActive(false);
            type.gameObject.SetActive(true);
            numberOfPrice.gameObject.SetActive(true);
            numberOfPrice.text = data.price.ToString();
        }
    }
}
