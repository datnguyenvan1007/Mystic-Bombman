using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private BoosterData boosterData;
    [SerializeField] private Image avatarHeroSkin;
    [SerializeField] private Transform boosterContainer;
    [SerializeField] private Text numberOfBomb;
    [SerializeField] private Text numberOfFlame;
    private List<int> listIndexSelectedBooster = new List<int>();
    void Start()
    {
        numberOfBomb.text = PlayerPrefs.GetInt("NumberOfBombs", 1).ToString();
        numberOfFlame.text = PlayerPrefs.GetInt("Flame", 1).ToString();
    }
    void OnEnable()
    {
        avatarHeroSkin.sprite = heroData.GetHero(PlayerPrefs.GetInt("IdHero", 0)).avatar;
        for (int i = 0; i < boosterData.Count; i++) {
            if (boosterData.GetBooster(i).quantiy == 0)
                continue;
            boosterContainer.GetChild(i).gameObject.SetActive(true);
            int index = i;
            boosterContainer.GetChild(index).GetComponent<Button>().onClick
            .AddListener(delegate {SelectBooster(index, boosterContainer.GetChild(index));});
            boosterContainer.GetChild(i).GetChild(2).GetComponent<Image>().sprite
            = boosterData.GetBooster(i).avatar;
            boosterContainer.GetChild(i).GetChild(4).GetComponent<Text>().text
            = boosterData.GetBooster(i).quantiy.ToString();
        }
    }
    void SelectBooster(int index, Transform trans) {
        if (trans.GetChild(0).gameObject.activeSelf) {
            trans.GetChild(0).gameObject.SetActive(false);
            listIndexSelectedBooster.Remove(index);
        }
        else {
            trans.GetChild(0).gameObject.SetActive(true);
            listIndexSelectedBooster.Add(index);
        }
    }
    public List<string> GetNameSelectedBooster() {
        List<string> names = new List<string>();
        foreach (int index in listIndexSelectedBooster) {
            boosterData.GetBooster(index).quantiy -= 1;
            names.Add(boosterData.GetBooster(index).name.ToString());
        }
        return names;
    }
    void OnDisable()
    {
        foreach (Transform booster in boosterContainer) {
            if (booster.gameObject.activeSelf) {
                booster.GetChild(0).gameObject.SetActive(false);
                booster.gameObject.SetActive(false);
                booster.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
        listIndexSelectedBooster.Clear();
    }
}
