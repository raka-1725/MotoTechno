using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUpgrade : MonoBehaviour
{
    public string NameOfComponent;
    public int currentLevel;
    public int price;

    public Image Thumbnail;

    [SerializeField] private TextMeshProUGUI mNameText;
    [SerializeField] private TextMeshProUGUI mCurrentLevel;
    [SerializeField] private TextMeshProUGUI mPrice;


    private void Start()
    {
        mCurrentLevel = GetComponentInChildren<TextMeshProUGUI>();
        mPrice = GetComponentInChildren<TextMeshProUGUI>();
        mNameText.SetText(NameOfComponent);
    }

    public void SetUpgrade(int currentlevel, int pricevalue) 
    {
        currentLevel = currentlevel;
        price = pricevalue;
        mCurrentLevel.SetText(currentlevel.ToString());
        mPrice.SetText($"Price : {price.ToString()}");
    }
}
