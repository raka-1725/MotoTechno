using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class ShopMaster : MonoBehaviour
{
    
    [Header("GeneratingSpec")]
    [SerializeField] MotorCycleCusomization mCustomSpec;

    [Header("CustomPriceSheet")]
    [SerializeField] CustomShopPrices mCustomPrices;

    [Header("DisplayMoto")]
    [SerializeField] MotorCycleCusomization mDisplaySpec;
    [SerializeField] GameObject mMotoDisplay;

    [SerializeField] private List<StatusUpgrade> mUpgrades;
    [SerializeField] private StatusUpgradeColor mUpgradeColorScript;

    [Header("ShopBranches")]
    [SerializeField] private GameObject mMain;
    [SerializeField] private GameObject mComponents;
    [SerializeField] private GameObject mColor;
    [SerializeField] private GameObject mCustomize;


    private void Start()
    {
        mCustomSpec = new MotorCycleCusomization();

        SetUpgrades();

        foreach (StatusUpgrade mUpgrades in mUpgrades) 
        {
            mUpgrades.upgradePurchased += OnUpgradePurchased;
        }
        mUpgradeColorScript.upgradePurchased += OnUpgradeColorPurchased;
    }

    private void OnUpgradeColorPurchased(StatusUpgradeColor color, bool defaultcolor, int priceToSubtract, Color customColor)
    {
        mCustomSpec.DefaultBodyColor = defaultcolor;
        mCustomSpec.mCustomBodyColor = customColor;
        PlayerCredits.Instance.subtractMoney(priceToSubtract);

        mDisplaySpec.DefaultBodyColor = defaultcolor;
        mDisplaySpec.mCustomBodyColor = customColor;
        UnityEngine.Debug.Log(customColor);
        UpdateDisplayMoto();
    }

    private void UpdateDisplayMoto()
    {
        mMotoDisplay.SetActive(false);
        mMotoDisplay.SetActive(true);
    }

    private void OnUpgradePurchased(StatusUpgrade upgrade, string nameOfComponent, int priceToSubtract)
    {
        ApplyUpgrades(upgrade);
        PlayerCredits.Instance.subtractMoney(priceToSubtract);
    }

    private void ApplyUpgrades(StatusUpgrade upgrade)
    {
        switch (upgrade.NameOfComponent)
        {
            case "Battery":
                switch (upgrade.currentLevel)
                {
                    case 1:
                        mCustomSpec.EnergyUseIndex = 2;
                        break;
                    case 2:
                        mCustomSpec.EnergyUseIndex = 1.7f;
                        break;
                    case 3:
                        mCustomSpec.EnergyUseIndex = 1.4f;
                        break;
                    case 4:
                        mCustomSpec.EnergyUseIndex = 1.2f;
                        break;
                    case 5:
                        mCustomSpec.EnergyUseIndex = 1.0f;
                        break;
                    default:
                        break;
                }
                break;
            case "Motor":
                switch (upgrade.currentLevel)
                {
                    case 1:
                        mCustomSpec.MaxPower = 5000;
                        break;
                    case 2:
                        mCustomSpec.MaxPower = 5500;
                        break;
                    case 3:
                        mCustomSpec.MaxPower = 7000;
                        break;
                    case 4:
                        mCustomSpec.MaxPower = 8500;
                        break;
                    case 5:
                        mCustomSpec.MaxPower = 10000;
                        break;
                    default:
                        break;
                }
                break;
            case "Brake":
                switch (upgrade.currentLevel)
                {
                    case 1:
                        mCustomSpec.BrakeTorque = 500;
                        break;
                    case 2:
                        mCustomSpec.BrakeTorque = 600;
                        break;
                    case 3:
                        mCustomSpec.BrakeTorque = 700;
                        break;
                    case 4:
                        mCustomSpec.BrakeTorque = 850;
                        break;
                    case 5:
                        mCustomSpec.BrakeTorque = 1000;
                        break;
                    default:
                        break;
                }
                break;
            case "Front Winglet":
                switch (upgrade.currentLevel)
                {
                    case 0:
                        mCustomSpec.FrontWinglet = false;
                        mDisplaySpec.FrontWinglet = false;
                        UpdateDisplayMoto();
                        break;
                    case 1:
                        mCustomSpec.FrontWinglet = true;
                        mDisplaySpec.FrontWinglet = true;
                        UpdateDisplayMoto();
                        break;
                    default:
                        break;
                }
                break;
            case "Rear Wing":
                switch (upgrade.currentLevel)
                {
                    case 0:
                        mCustomSpec.RearWing = false;
                        mDisplaySpec.RearWing = false;
                        UpdateDisplayMoto();
                        break;
                    case 1:
                        mCustomSpec.RearWing = true;
                        mDisplaySpec.RearWing = true;
                        UpdateDisplayMoto();
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
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

    //Generating Spec inPROGRESS

    //public void GenerateSpecSheet()
    //{
    //    MotorCycleCusomization newSpec = ScriptableObject.CreateInstance<MotorCycleCusomization>();

    //    newSpec.EnergyUseIndex = mCustomSpec.EnergyUseIndex;
    //    newSpec.MaxPower = mCustomSpec.MaxPower;
    //    newSpec.BrakeTorque = mCustomSpec.BrakeTorque;
    //    newSpec.FrontWinglet = mCustomSpec.FrontWinglet;
    //    newSpec.RearWing = mCustomSpec.RearWing;
    //    newSpec.DefaultBodyColor = mCustomSpec.DefaultBodyColor;
    //    newSpec.mCustomBodyColor = mCustomSpec.mCustomBodyColor;


    //    string assetPath = "Assets/GeneratedSpecs/";
    //    if (!System.IO.Directory.Exists(assetPath))
    //    {
    //        System.IO.Directory.CreateDirectory(assetPath);
    //    }

    //    string fileName = $"MotorcycleSpec_{DateTime.Now:yyyyMMdd_HHmmss}.asset";
    //    UnityEditor.AssetDatabase.CreateAsset(newSpec, assetPath + fileName);
    //    UnityEditor.AssetDatabase.SaveAssets();
    //    UnityEditor.AssetDatabase.Refresh();
    //}




}
