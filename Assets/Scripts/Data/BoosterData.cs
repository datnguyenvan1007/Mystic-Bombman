using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New BoosterData", menuName = "Data/Booster")]
public class BoosterData : ScriptableObject
{
    public List<Booster> boosters = new List<Booster>();
    public int Count {get => boosters.Count;}
    public Booster GetBooster(int index) {
        return boosters[index];
    }
}
[System.Serializable]
public class Booster {
    public BoosterName name;
    public int price;
    public Sprite avatar;
}
public enum BoosterName {
    Bomb,
    Flame,
    Detonator,
    WallPass,
    BombPass,
    FlamePass,
    Mystery
}