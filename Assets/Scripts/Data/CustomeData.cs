using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custome", menuName = "Data/Custome")]
public class CustomeData : ScriptableObject
{
    public List<Custome> customes = new List<Custome>();
    public int Count {get => customes.Count;}
    public Custome GetCustome(int index) {
        return customes[index];
    }
    public void Add(Custome custome) {
        customes.Add(custome);
    }
}
[System.Serializable]
public class Custome{
    public string id;
    public string name;
    public Sprite avatar;
    public Currency currency;
    public int price;
}
