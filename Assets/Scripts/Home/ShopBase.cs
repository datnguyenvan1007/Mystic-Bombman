using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShopBase : MonoBehaviour
{
    [SerializeField] protected Text gold;
    [SerializeField] protected GameObject info;
    [SerializeField] protected GameObject confirm;
    [SerializeField] protected GameObject lackOfGold;
    [SerializeField] protected new Text name;
    [SerializeField] protected Image image;
    [SerializeField] protected Text numberOfPrice;
    [SerializeField] protected GameObject objectPrefab;
    [SerializeField] protected RectTransform content;
    [SerializeField] protected GameObject objectContainer;
    protected List<GameObject> instances = new List<GameObject>();
    protected GridLayoutGroup grid;
    protected GameObject oldHighLight = null;
    protected int indexOfSelectedObject;
    protected virtual void Start()
    {
        grid = objectContainer.GetComponent<GridLayoutGroup>();
    }
    public abstract void ShowInfo(int index, Transform trans);
    public abstract void Confirm();
    protected virtual void OnEnable()
    {
        gold.text = GameData.gold.ToString();
        confirm.transform.GetChild(1).GetChild(4).GetComponent<Button>()
        .onClick.AddListener(Confirm);
    }
    protected virtual void OnDisable()
    {
        info.SetActive(false);
        if (oldHighLight != null)
        {
            oldHighLight.SetActive(false);
        }
        confirm.transform.GetChild(1).GetChild(4).GetComponent<Button>()
        .onClick.RemoveListener(Confirm);
    }
}
