using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<Image> buttonsMenu;
    [SerializeField] private List<GameObject> views;
    [SerializeField] private Color colorOfSelectedButton;
    [SerializeField] private Color colorOfUnselectedButton;
    private int oldIndex = 0;
    private void Start() {
        for (int i = 0; i < buttonsMenu.Count; i++) {
            int x = i;
            buttonsMenu[i].GetComponent<Button>().onClick.AddListener(delegate {SelectMenu(x);});
        }
    }
    public void SelectMenu(int index)
    {
        if (oldIndex == index)
            return;
        buttonsMenu[oldIndex].color = colorOfUnselectedButton;
        views[oldIndex].SetActive(false);
        buttonsMenu[index].color = colorOfSelectedButton;
        views[index].SetActive(true);
        oldIndex = index;
    }
}
