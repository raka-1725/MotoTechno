using System;
using UnityEngine;

[Serializable]
public class MotorCycleCusomizationData
{
    public float EnergyUseIndex;
    public int MaxPower;
    public int BrakeTorque;
    public bool FrontWinglet;
    public bool RearWing;
    public bool DefaultBodyColor;
    public Color mCustomBodyColor;
    public float RearWingGripMultiplier;
    public float FrontWingletGripMultiplier;
    public int BatteryCapacity;
    public int RegenStrength;
    public float OverTakeIndex;
}