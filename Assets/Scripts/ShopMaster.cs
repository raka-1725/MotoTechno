using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopMaster : MonoBehaviour
{
    
    [Header("GeneratingSpec")]
    [SerializeField] MotorCycleCusomization mCustomSpec;
    [SerializeField] MotorCycleCusomization mDefaultSpec;

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

    [SerializeField] private Button mDefaultSpecButton;

    private void Awake()
    {
        foreach (StatusUpgrade mUpgrades in mUpgrades) 
        {
            mUpgrades.upgradePurchased += OnUpgradePurchased;
        }
        mUpgradeColorScript.upgradePurchased += OnUpgradeColorPurchased;
    }

    private void Start()
    {
        mCustomSpec = new MotorCycleCusomization();

        SetUpgrades();

        mDefaultSpecButton.onClick.AddListener(ResetToDefault);
        LoadPreviousSpec();
    }
    private void Update()
    {
        
        
    }

    private MotorCycleCusomization CloneSpec(MotorCycleCusomization original)
    {
        MotorCycleCusomization clone = ScriptableObject.CreateInstance<MotorCycleCusomization>();

        clone.EnergyUseIndex = original.EnergyUseIndex;
        clone.MaxPower = original.MaxPower;
        clone.BrakeTorque = original.BrakeTorque;
        clone.FrontWinglet = original.FrontWinglet;
        clone.RearWing = original.RearWing;

        clone.DefaultBodyColor = original.DefaultBodyColor;
        clone.mDefaultBodyColor = original.mDefaultBodyColor;
        clone.mCustomBodyColor = original.mCustomBodyColor;

        clone.RearWingGripMultiplier = original.RearWingGripMultiplier;
        clone.FrontWingletGripMultiplier = original.FrontWingletGripMultiplier;
        clone.BatteryCapacity = original.BatteryCapacity;
        clone.RegenStrength = original.RegenStrength;
        clone.OverTakeIndex = original.OverTakeIndex;

        return clone;
    }


    private void OnUpgradeColorPurchased(StatusUpgradeColor color, bool defaultcolor, int priceToSubtract, Color customColor)
    {
        mCustomSpec.DefaultBodyColor = defaultcolor;
        mCustomSpec.mCustomBodyColor = customColor;
        PlayerCredits.Instance.subtractMoney(priceToSubtract);

        mDisplaySpec.DefaultBodyColor = defaultcolor;
        mDisplaySpec.mCustomBodyColor = customColor;
        UpdateDisplayMoto();
    }

    private void UpdateDisplayMoto()
    {
        mDisplaySpec.FrontWinglet = mCustomSpec.FrontWinglet;
        mDisplaySpec.RearWing = mCustomSpec.RearWing;
        mDisplaySpec.DefaultBodyColor = mCustomSpec.DefaultBodyColor;
        mDisplaySpec.mDefaultBodyColor = mCustomSpec.mDefaultBodyColor;
        mDisplaySpec.mCustomBodyColor = mCustomSpec.mCustomBodyColor;
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
                mCustomSpec.EnergyUseIndex = GetEnergyIndex(upgrade.currentLevel);
                mCustomSpec.OverTakeIndex = GetOverTakeIndex(upgrade.currentLevel);
                break;
            case "Motor":
                mCustomSpec.MaxPower = GetMotorPower(upgrade.currentLevel);
                mCustomSpec.RegenStrength = GetRegenStrength(upgrade.currentLevel);
                break;
            case "Brake":
                mCustomSpec.BrakeTorque = GetBrakeTorque(upgrade.currentLevel);
                break;
            case "Front Winglet":
                mCustomSpec.FrontWinglet = upgrade.currentLevel > 0;
                mDisplaySpec.FrontWinglet = upgrade.currentLevel > 0;
                UpdateDisplayMoto();
                break;
            case "Rear Wing":
                mCustomSpec.RearWing = upgrade.currentLevel > 0;
                mDisplaySpec.RearWing = upgrade.currentLevel > 0;
                UpdateDisplayMoto();
                break;
        }
    }
    private float GetOverTakeIndex(int level) => level switch
    {
        1 => 1.1f,
        2 => 1.3f,
        3 => 1.4f,
        4 => 1.6f,
        5 => 1.8f,
        _ => mDefaultSpec.OverTakeIndex,
    };
    private float GetRegenStrength(int level) => level switch
    {
        1 => 10,
        2 => 20,
        3 => 30,
        4 => 40,
        5 => 50,
        _ => mDefaultSpec.RegenStrength
    };

    private int GetBrakeTorque(int level) => level switch
    {
        1 => 500,
        2 => 600,
        3 => 700,
        4 => 850,
        5 => 1000,
        _ => mDefaultSpec.BrakeTorque
    };

    private int GetMotorPower(int level) => level switch
    {
        1 => 5000,
        2 => 5500,
        3 => 7000,
        4 => 8500,
        5 => 10000,
        _ => mDefaultSpec.MaxPower
    };

    private float GetEnergyIndex(int level) => level switch
    {
        1 => 2f,
        2 => 1.7f,
        3 => 1.4f,
        4 => 1.2f,
        5 => 1.0f,
        _ => mDefaultSpec.EnergyUseIndex
    };

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

    private void ResetToDefault()
    {
        mCustomSpec = CloneSpec(mDefaultSpec);
        UpdateDisplayMoto();  
    }

    public void GenerateSpecSheet()
    {
        if (SpecIdentical(mDefaultSpec, mCustomSpec)) 
        {
            return;
        }
        ApplyDefaultIfUnset(mCustomSpec, mDefaultSpec);

        MotorCycleCusomization finalSpec = CloneSpec(mCustomSpec);

        string assetPath = "Assets/GeneratedSpecs/";
        if (!System.IO.Directory.Exists(assetPath))
        {
            System.IO.Directory.CreateDirectory(assetPath);
        }


        //Unity Editor
#if UNITY_EDITOR
        string fileName = $"MotorcycleSpec_{DateTime.Now:yyyyMMdd_HHmmss}.asset";
        UnityEditor.AssetDatabase.CreateAsset(finalSpec, assetPath + fileName);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif
        string jsonPath = System.IO.Path.Combine(Application.persistentDataPath, "MotorcycleSpec.json");
        string jsonData = JsonUtility.ToJson(finalSpec, true);
        System.IO.File.WriteAllText(jsonPath, jsonData);
    }

    private void ApplyDefaultIfUnset(MotorCycleCusomization custom, MotorCycleCusomization defaults)
    {
        if (custom.EnergyUseIndex <= 0f)
            custom.EnergyUseIndex = defaults.EnergyUseIndex;
        if (custom.MaxPower <= 0)
            custom.MaxPower = defaults.MaxPower;
        if (custom.BrakeTorque <= 0)
            custom.BrakeTorque = defaults.BrakeTorque;
        if (!custom.FrontWinglet && !defaults.FrontWinglet)
            custom.FrontWinglet = defaults.FrontWinglet;
        if (!custom.RearWing && !defaults.RearWing)
            custom.RearWing = defaults.RearWing;

        if (custom.mCustomBodyColor == defaults.mCustomBodyColor)
        {
            custom.DefaultBodyColor = true;
        }
        else
        {
            custom.DefaultBodyColor = false;
        }

        custom.RearWingGripMultiplier = defaults.RearWingGripMultiplier;
        custom.FrontWingletGripMultiplier = defaults.FrontWingletGripMultiplier;
        custom.BatteryCapacity = defaults.BatteryCapacity;
        custom.RegenStrength = defaults.RegenStrength;
        custom.OverTakeIndex = defaults.OverTakeIndex;

    }

    private bool SpecIdentical(MotorCycleCusomization defaultspec, MotorCycleCusomization customs) 
    {
        return defaultspec.EnergyUseIndex == customs.EnergyUseIndex &&
              defaultspec.MaxPower == customs.MaxPower &&
              defaultspec.BrakeTorque == customs.BrakeTorque &&
              defaultspec.FrontWinglet == customs.FrontWinglet &&
              defaultspec.RearWing == customs.RearWing &&
              defaultspec.DefaultBodyColor == customs.DefaultBodyColor &&
              defaultspec.mCustomBodyColor == customs.mCustomBodyColor;
    }
    private void LoadPreviousSpec()
    {
        string jsonPath = System.IO.Path.Combine(Application.persistentDataPath, "MotorcycleSpec.json");
        if (System.IO.File.Exists(jsonPath))
        {
            string jsonData = System.IO.File.ReadAllText(jsonPath);
            MotorCycleCusomizationData savedData = JsonUtility.FromJson<MotorCycleCusomizationData>(jsonData);
            if (savedData != null)
            {
                mCustomSpec = ScriptableObject.CreateInstance<MotorCycleCusomization>();
                mCustomSpec.EnergyUseIndex = savedData.EnergyUseIndex;
                mCustomSpec.MaxPower = savedData.MaxPower;
                mCustomSpec.BrakeTorque = savedData.BrakeTorque;
                mCustomSpec.FrontWinglet = savedData.FrontWinglet;
                mCustomSpec.RearWing = savedData.RearWing;
                mCustomSpec.DefaultBodyColor = savedData.DefaultBodyColor;
                mCustomSpec.mCustomBodyColor = savedData.mCustomBodyColor;
                mCustomSpec.RearWingGripMultiplier = savedData.RearWingGripMultiplier;
                mCustomSpec.FrontWingletGripMultiplier = savedData.FrontWingletGripMultiplier;
                mCustomSpec.BatteryCapacity = savedData.BatteryCapacity;
                mCustomSpec.RegenStrength = savedData.RegenStrength;
                mCustomSpec.OverTakeIndex = savedData.OverTakeIndex;
            }
        }
        else
        {
            mCustomSpec = CloneSpec(mDefaultSpec);
        }
        UpdateDisplayMoto();
    }


}
