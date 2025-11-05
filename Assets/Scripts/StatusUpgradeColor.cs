using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StatusUpgradeColor : MonoBehaviour
{
    ColorPicker mColorPicker;
    public int priceDefaultColor;
    public int priceCustomColor;
    public bool bDefaultSelected;


    public Color buttonSelectedcolor;

    [SerializeField] private Color mDefaultBodyColor;
    
    public Color customedColor;
    private ShopMaster mShopMaster;

    [SerializeField] private Button mSelectDefaultColorButton;
    [SerializeField] private Button mSelectCustomColorButton;

    [SerializeField] private TextMeshProUGUI mDefaultPrice;
    [SerializeField] private TextMeshProUGUI mCustomPrice;

    public Action<StatusUpgradeColor, bool, int, Color> upgradePurchased;


    private void Awake()
    {
        mColorPicker = FindAnyObjectByType<ColorPicker>();
        mShopMaster = FindAnyObjectByType<ShopMaster>();

        mDefaultPrice.SetText($"Price : {priceDefaultColor}");
        mCustomPrice.SetText($"Price : {priceCustomColor}");
    }

    private void Update()
    {
        if (PlayerCredits.Instance.credit <= priceCustomColor)
        {
            mSelectCustomColorButton.interactable = false;
        }
    }

    public void SelectColorOption(bool isDefalut) 
    {
        if (!isDefalut)
        {
            customedColor = mColorPicker.selectedColor;
            bDefaultSelected = false;
        }
        else
        {
            Color.RGBToHSV(mDefaultBodyColor, out float h, out float s, out float v);
            mColorPicker.currentHue = h;
            mColorPicker.currentSat = s;
            mColorPicker.currentVal = v;
            bDefaultSelected = true;
            ApplyColor();
        }
    }

    public void ApplyColor() 
    {
        int pricesubtract = bDefaultSelected ? priceDefaultColor : priceCustomColor;
        mShopMaster.SetBodyColor(!bDefaultSelected, bDefaultSelected ? mDefaultBodyColor : customedColor);
        upgradePurchased.Invoke(this, bDefaultSelected, pricesubtract, customedColor);
    }

 



}
