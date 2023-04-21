using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public List<Item> items = new List<Item>();
    public int Count {get => items.Count;}
    public Item GetItem(int index) {
        return items[index];
    }
}

[System.Serializable]
public class Item{
    public string id;
    public string name;
    public Sprite avatar;
    public Currency currency;
    public int price;
    public int quantity;
}