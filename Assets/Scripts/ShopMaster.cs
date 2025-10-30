using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShopMaster : MonoBehaviour
{
    [Header("GeneratingSpec")]
    [SerializeField] MotorCycleCusomization mCustomSpec;

    [Header("CustomPriceSheet")]
    [SerializeField] CustomShopPrices mCustomPrices;

    [Header("Stats")]
    [SerializeField] private int currentPoints;

    [SerializeField] private List<StatusUpgrade> mUpgrades;

    [Header("ShopBranches")]
    [SerializeField] private GameObject mMain;
    [SerializeField] private GameObject mComponents;
    [SerializeField] private GameObject mColor;
    [SerializeField] private GameObject mCustomize;

    private void Start()
    {
        mCustomSpec = new MotorCycleCusomization();
        mUpgrades = new List<StatusUpgrade>(GetComponentsInChildren<StatusUpgrade>());

        SetUpgrades();
    }

    public void SetUpgrades() 
    {

        foreach (StatusUpgrade statusupgrades in mUpgrades) 
        {
            switch (statusupgrades.NameOfComponent) 
            {
                case "Battery":
                    statusupgrades.SetUpgrade(1,mCustomPrices.batteryUpgradePrice);
                    break;
                case "Motor":
                    statusupgrades.SetUpgrade(1, mCustomPrices.motorUpgradePrice);
                    break;
                case "Brake":
                    statusupgrades.SetUpgrade(1, mCustomPrices.brakeUpgradePrice);
                    break;
                case "Front Winglet":
                    statusupgrades.SetUpgrade(0, mCustomPrices.frontWingletPrice);
                    break;
                case "Rear Wing":
                    statusupgrades.SetUpgrade(0,mCustomPrices.rearWingPrice);
                    break;
                default:
                    break;
            }
        }

        SetMainMenu();
    }

    private void SetMainMenu()
    {
        mMain.SetActive(true);
        mComponents.SetActive(false);
        mColor.SetActive(false);
        mCustomize.SetActive(false);
    }

    public void SetBodyColor(bool isCustom, Color customedcolor) 
    {
        mCustomSpec.DefaultBodyColor = !isCustom;
        if (isCustom) 
        {
            mCustomSpec.mCustomBodyColor = customedcolor;
        }
    }
}
