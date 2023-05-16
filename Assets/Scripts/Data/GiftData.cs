using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Gift", menuName = "Data/Gift")]
public class GiftData : ScriptableObject
{
    public List<Gift> dailyGifts = new List<Gift>();
    public int Count {get => dailyGifts.Count;}
    public Gift GetGift(int index) {
        return dailyGifts[index];
    }
}
[System.Serializable]
public class Gift {
    public int id;
    public int quantity;
}
public enum GiftStatus {
    Claimed,
    Active,
    Lock
}