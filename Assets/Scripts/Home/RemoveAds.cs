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
    private void Start()
    {
        priceText.text = "$" + priceToRemoveAds.ToString();
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
        });
    }
}
