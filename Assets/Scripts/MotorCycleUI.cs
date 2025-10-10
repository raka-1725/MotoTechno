using TMPro;
using UnityEngine;

public class MotorCycleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mSpeedText;
    [SerializeField] private TextMeshProUGUI mBattery;

    public void Speed(float speed) 
    {
        mSpeedText.text = speed.ToString("0.00");
    }

    public void BatteryPercentage(float batteryPercentage) 
    {
        mBattery.text = batteryPercentage.ToString("0.00");
    }
}
