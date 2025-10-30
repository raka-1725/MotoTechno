using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUpgrade : MonoBehaviour
{
    public string NameOfComponent;
    public int currentLevel;
    public int price;

    public Image Thumbnail;

    private TextMeshProUGUI mCurrentLevel;
    private TextMeshProUGUI mPrice;


    private void Start()
    {
        mCurrentLevel = GetComponentInChildren<TextMeshProUGUI>();
        mPrice = GetComponentInChildren<TextMeshProUGUI>();

    }

    public void SetUpgrade(int currentlevel, int price) 
    {
        mCurrentLevel.SetText(currentlevel.ToString());
        mPrice.SetText(price.ToString());
    }
}
