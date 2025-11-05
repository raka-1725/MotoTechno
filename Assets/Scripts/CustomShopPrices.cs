using UnityEngine;


[CreateAssetMenu(fileName = "ShopPrice", menuName = "ScriptableObject_ShopCustom")]
public class CustomShopPrices : ScriptableObject
{
    public int motorUpgradePrice = 10;
    public int brakeUpgradePrice = 10;
    public int batteryUpgradePrice = 10;
    public int frontWingletPrice = 50;
    public int rearWingPrice = 70;

    public int defaultPrice = 10;
}
