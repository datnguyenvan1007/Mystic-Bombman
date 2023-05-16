using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnedHeroController : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private GameObject ownedHeroPrefab;
    [SerializeField] private GameObject info;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject objectContainer;
    [SerializeField] private Image avatarCostumeSkin_PlayerInfo;
    [Header("Selection")]
    [SerializeField] private Text id;
    [SerializeField] private new Text name;
    [SerializeField] private Image avatar;
    private GameObject oldHighLight = null;
    private GameObject oldSelected = null;
    private Transform clickedGameObject = null;
    private GridLayoutGroup grid;
    List<GameObject> instancesHero = new List<GameObject>();
    Vector2 sizeDelta;
    private void Awake()
    {
        sizeDelta = content.sizeDelta;
    }

    void OnEnable()
    {
        grid = objectContainer.GetComponent<GridLayoutGroup>();
        int rowCount = Mathf.CeilToInt(heroData.Count / (grid.constraintCount * 1.0f));
        content.sizeDelta += new Vector2(0, (rowCount) * (grid.cellSize.y + grid.spacing.y));
        int idHeroSkin = PlayerPrefs.GetInt("IdHero", 0);
        string[] purchasedHeroes = PlayerPrefs.GetString("PurchasedHeroes", "0,").Split(",");
        for (int i = 0; i < purchasedHeroes.Length - 1; i++)
        {
            int index = Convert.ToInt32(purchasedHeroes[i]);
            instancesHero.Add(Instantiate(ownedHeroPrefab, objectContainer.transform));
            instancesHero[i].transform.GetChild(2).GetChild(0).GetComponent<Text>().text
            = heroData.GetHero(index).name.ToString();
            instancesHero[i].transform.GetChild(3).GetComponent<Image>().sprite
            = heroData.GetHero(index).avatar;
            instancesHero[i].transform.GetChild(4).GetComponent<Text>().text
            = heroData.GetHero(index).id.ToString();
            int x = index;
            int j = i;
            instancesHero[i].GetComponent<Button>().onClick.AddListener(delegate { ShowInfo(heroData.GetHero(x), instancesHero[j].transform); });
            if (idHeroSkin == heroData.GetHero(index).id)
            {
                ShowInfo(heroData.GetHero(index), instancesHero[i].transform);
                ShowSelectedHero();
            }
        }
    }
    private void OnDisable()
    {
        foreach (GameObject instance in instancesHero)
        {
            Destroy(instance);
        }
        instancesHero.Clear();
        content.sizeDelta = sizeDelta;
    }
    public void ShowInfo(Hero data, Transform tran)
    {
        if (oldHighLight != null)
        {
            oldHighLight.SetActive(false);
        }
        tran.GetChild(0).gameObject.SetActive(true);
        oldHighLight = tran.GetChild(0).gameObject;
        if (!info.activeSelf)
            info.SetActive(true);
        id.text = data.id.ToString();
        name.text = data.name;
        avatar.sprite = data.avatar;
        clickedGameObject = tran;
    }
    public void Select()
    {
        PlayerPrefs.SetInt("IdHero", Convert.ToInt32(id.text));
        avatarCostumeSkin_PlayerInfo.sprite = heroData.GetHero(Convert.ToInt32(id.text)).avatar;
        ShowSelectedHero();
    }
    private void ShowSelectedHero()
    {
        if (oldSelected != null)
        {
            oldSelected.SetActive(false);
        }
        oldSelected = clickedGameObject.GetChild(5).gameObject;
        clickedGameObject.GetChild(5).gameObject.SetActive(true);
    }
}
