using UnityEngine;
using UnityEngine.Rendering;

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

    private void Awake()
    {
        mColorPicker = FindAnyObjectByType<ColorPicker>();
        mShopMaster = FindAnyObjectByType<ShopMaster>();
    }

    public void SetColorSelection(bool isDefaultcolor) 
    {
        if (!isDefaultcolor) 
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
        }

    }

    public void ApplyColor() 
    {
        mShopMaster.SetBodyColor(!bDefaultSelected, bDefaultSelected ? mDefaultBodyColor : customedColor);
    }
}
