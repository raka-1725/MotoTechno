using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private LapChecker mLapChecker;
    public int CheckPointIndex;

    public void SetLapChecker() 
    {
        mLapChecker = GameObject.FindGameObjectWithTag("PlayerMotorCycle").GetComponent<LapChecker>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerMotorCycle")) 
        return;

        mLapChecker.PassCheckPoint(CheckPointIndex);
    }
}
