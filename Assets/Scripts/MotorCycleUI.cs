using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MotorCycleUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI mSpeedText;
    [SerializeField] private TextMeshProUGUI mBattery;
    [Header("BattSlider")]
    [SerializeField] private Slider mBattSlider;

    [Header("PowerSlider")]
    [SerializeField] private Slider mPowerSlider;
    [SerializeField] private Slider mRegenSlider;

    [Header("Overtake")]
    [SerializeField] private GameObject OvertakeIndicator;
    public void Speed(float speed) 
    {
        mSpeedText.text = speed.ToString("0.00");
    }

    public void BatteryPercentage(float batteryPercentage) 
    {
        mBattery.text = batteryPercentage.ToString("0.00");
        mBattSlider.value = batteryPercentage / 100;
    }

    public void PowerMeter(float energyuse, float regen) 
    {
        mPowerSlider.value = energyuse;
        mRegenSlider.value = regen;
    }

    public void OverTakeIndicator(bool overtake) 
    {
        OvertakeIndicator.SetActive(overtake);
    }

}
