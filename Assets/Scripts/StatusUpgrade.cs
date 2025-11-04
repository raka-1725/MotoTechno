using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUpgrade : MonoBehaviour
{
    public string NameOfComponent;
    public int currentLevel;
    public int price;
    public int maxLevel;
    private int purchasedLevel;
    private bool bCanPurchaseUpgrade = true;

    public Image ThumbnailImage;
    public Sprite Thumbnail;

    [SerializeField] private TextMeshProUGUI mNameText;
    [SerializeField] private TextMeshProUGUI mCurrentLevel;
    [SerializeField] private TextMeshProUGUI mPrice;
    [SerializeField] private Button mPurchaseButton;


    public Action<StatusUpgrade, string, int> upgradePurchased;

    


    private void Start()
    {
        mNameText.SetText(NameOfComponent);
        mPurchaseButton.onClick.AddListener(PurchaceUpgrade);
        ThumbnailImage.sprite = Thumbnail;
    }

    public void SetUpgrade(int currentlevel, int pricevalue) 
    {
        currentLevel = currentlevel;
        price = pricevalue;
        mCurrentLevel.SetText(currentlevel.ToString());
        mPrice.SetText($"Price : {price.ToString()}");
    }

    private void Update()
    {
        CheckPurchaseable();
    }

    private void CheckPurchaseable()
    {
        bCanPurchaseUpgrade = currentLevel < maxLevel;
        if (PlayerCredits.Instance.credit <= price) 
        {
            bCanPurchaseUpgrade = false;
        }
        mPurchaseButton.interactable = bCanPurchaseUpgrade;
    }

    public void PurchaceUpgrade() 
    {
        if (!bCanPurchaseUpgrade) return;

        currentLevel++;
        purchasedLevel = currentLevel;
        int pricesubtract = price;
        int newPrice = price * 2;

        SetUpgrade(currentLevel, newPrice);
        upgradePurchased?.Invoke(this, NameOfComponent, pricesubtract);
    }
}
