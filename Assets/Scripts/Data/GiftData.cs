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
    public void ResetStatus() {
        if (dailyGifts.Count == 0)
            return;
        dailyGifts[0].status = GiftStatus.Active;
        for (int i = 1; i < Count; i++) {
            dailyGifts[i].status = GiftStatus.Lock;
        }
    }
}
[System.Serializable]
public class Gift {
    public int id;
    public int quantity;
    public GiftStatus status;
    public string claimedTime;
}
public enum GiftStatus {
    Claimed,
    Active,
    Lock
}