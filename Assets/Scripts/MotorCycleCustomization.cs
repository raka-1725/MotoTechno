using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "MotorCycleCusomize", menuName = "ScriptableObjects")  ]
public class MotorCycleCusomization : ScriptableObject
{
    //[SerializeField] private GameObject mMotorCyclePrefab;

    [Header("Power/Brake")]
    public int MaxPower;
    public int BrakeTorque;


    [Header("DriveType")]

    public DriveTypes mDriveTypes = DriveTypes.Rear;
    public enum DriveTypes
    {
        Rear = 1, Front = 2, All = 3
    }

    [Header("Battery")]
    public float BatteryCapacity;
    public float EnergyUseIndex;
    public float RegenStrength;
    public float OverTakeIndex;

    [Header("CustomizeVechicle")]
    public bool RearWing;
    public bool FrontWinglet;
    public bool DefaultBodyColor;
    public Color mDefaultBodyColor;
    public Color mCustomBodyColor;

}
