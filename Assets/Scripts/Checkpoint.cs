using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private LapChecker mLapChecker;
    public int CheckPointIndex;

    private void Awake()
    {
         mLapChecker = FindAnyObjectByType<LapChecker>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerMotorCycle")) 
        return;

        mLapChecker.PassCheckPoint(CheckPointIndex);
    }
}
