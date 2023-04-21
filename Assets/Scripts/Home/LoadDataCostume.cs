using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDataCostume : MonoBehaviour
{
    [SerializeField] private GameObject info;
    [SerializeField] private Text id;
    [Header("Name")]
    [SerializeField] private new Text name;
    [SerializeField] private Image image;
    [Header("Price")]
    [SerializeField] private Image type;
    [SerializeField] private Text numberOfPrice;
    [SerializeField] private GameObject watchAds;
    [SerializeField] private CustomeData heroData;
    [SerializeField] private CustomeData bombData;
    [SerializeField] private GameObject customePrefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject hero;
    [SerializeField] private GameObject bomb;
    [SerializeField] private GameObject flame;
    private RectTransform rectTransform;
    List<GameObject> instancesItem = new List<GameObject>();
    private GridLayoutGroup grid;
    private GameObject oldHighLight = null;
    void Start()
    {
        LoadData(hero, heroData);
        LoadData(bomb, bombData);
    }
    void LoadData(GameObject obj, CustomeData data)
    {
        int start = instancesItem.Count;
        int end = start + data.Count;
        grid = obj.GetComponent<GridLayoutGroup>();
        rectTransform = obj.GetComponent<RectTransform>();
        int rowCount = Mathf.CeilToInt(data.Count / (grid.constraintCount * 1.0f));
        rectTransform.sizeDelta += new Vector2(0, rowCount * (grid.cellSize.y + grid.spacing.y));
        content.sizeDelta += new Vector2(0, rowCount * (grid.cellSize.y + grid.spacing.y));
        for (int i = start; i < end; i++)
        {
            instancesItem.Add(Instantiate(customePrefab, obj.transform));
            instancesItem[i].transform.GetChild(2).GetComponent<Image>().sprite
            = data.GetCustome(i - start).avatar;
            instancesItem[i].transform.GetChild(3).GetComponent<Text>().text
            = data.GetCustome(i - start).name;
            instancesItem[i].transform.GetChild(4).GetComponent<Text>().text
            = data.GetCustome(i - start).price.ToString();
            instancesItem[i].transform.GetChild(5).GetComponent<Text>().text
            = data.GetCustome(i - start).currency.ToString();
            instancesItem[i].transform.GetChild(6).GetComponent<Text>().text
            = data.GetCustome(i - start).id.ToString();
            int x = i;
            instancesItem[x].GetComponent<Button>().onClick.AddListener(delegate { ShowInfo(data.GetCustome(x  - start), instancesItem[x].transform); });
        }
    }
    public void ShowInfo(Custome data, Transform tran)
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
