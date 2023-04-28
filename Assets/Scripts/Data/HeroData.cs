using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Hero")]
public class HeroData : ScriptableObject
{
    public List<Hero> heroes = new List<Hero>();
    public int Count {get => heroes.Count;}
    public Hero GetHero(int index) {
        return heroes[index];
    }
    public List<Hero> GetPurchasedHero() {
        List<Hero> list = new List<Hero>();
        for (int i = 0; i < Count; i++) {
            if (heroes[i].isPurchased) {
                list.Add(heroes[i]);
            }
        }
        return list;
    }
}
[System.Serializable]
public class Hero{
    public int id;
    public string name;
    public Sprite avatar;
    public Currency currency;
    public int price;
    public bool isPurchased;
    public RuntimeAnimatorController anim;
}
