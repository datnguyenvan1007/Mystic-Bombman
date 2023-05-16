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
}
[System.Serializable]
public class Hero{
    public int id;
    public string name;
    public Sprite avatar;
    public Currency currency;
    public int price;
    public RuntimeAnimatorController anim;
}
