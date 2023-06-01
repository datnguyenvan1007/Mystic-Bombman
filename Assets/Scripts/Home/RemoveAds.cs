using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveAds : MonoBehaviour
{
    [SerializeField] private float priceToRemoveAds;
    [SerializeField] private GameObject purchasePanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject removeAdsButton;
    [SerializeField] private string succeedMessage;
    [SerializeField] private string failedMessage;
    [SerializeField] private Text messageConfirmText;
    [SerializeField] private Text priceText;
    private Transform panel;
    private Transform container_confirmPanel;
    private void Awake()
    {
        panel = transform.GetChild(1);
        container_confirmPanel = confirmPanel.transform.GetChild(1);
        priceText.text = "$" + priceToRemoveAds.ToString();
    }
    private void OnEnable()
    {
        panel.DOScale(Vector3.one, 0.3f);
    }
    private void OnDisable()
    {
        panel.DOScale(Vector3.zero, 0.3f);
    }
    public void Purchase()
    {
        MG_Interface.Current.Purchase_Item("RemoveAds", (bool result, bool onIAP, string productId) =>
        {
            if (result)
            {
                messageConfirmText.text = succeedMessage;
                removeAdsButton.SetActive(false);
                MG_PlayerPrefs.SetBool("RemoveAds", true);
            }
            else
            {
                messageConfirmText.text = failedMessage;
            }
            purchasePanel.SetActive(false);
            confirmPanel.SetActive(true);
            container_confirmPanel.DOScale(Vector3.one, 0.3f);
        });
    }
    public void Confirm()
    {
        confirmPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}
