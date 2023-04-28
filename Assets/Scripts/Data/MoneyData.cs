using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
public class MoneyData : ScriptableObject
{
    public List<Money> money = new List<Money>();
    public int Count { get => money.Count; }
    public Money GetMoney(int index)
    {
        return money[index];
    }
}

[System.Serializable]
public class Money
{
    public int id;
    public string name;
    public Sprite avatar;
    public int price;
    public int quantity;
}
