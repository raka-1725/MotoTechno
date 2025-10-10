using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class LapChecker : MonoBehaviour
{
    [SerializeField] private GameObject[] mCheckPoints;
    [Header("CurrentLap/Sector")]
    public int currentLap = 0;
    public int currentSector = 0;

    [Header("SectorTime")]
    [SerializeField] private float LapTime;
    [SerializeField] private float SectorTime;

    [SerializeField] private int mCurrentcheckPoint;

    public void PassCheckPoint(int checkpointindex) 
    {
        if (checkpointindex > mCheckPoints.Length) 
        {
            mCurrentcheckPoint = 0;

        }
        mCurrentcheckPoint++;
        currentSector++;

        if (mCurrentcheckPoint > 3) 
        {
            mCurrentcheckPoint = 1;
            currentSector = 0;
            NewLap();
        }
        SectorTime = UpdateSectorTime();


    }

    private void Update()
    {
        LapTimer();
    }

    private float UpdateSectorTime() 
    {

        float sectortime = LapTime;


        return sectortime;
    }

    private void LapTimer() 
    {
        LapTime += Time.deltaTime;
    }

    private void NewLap() 
    {
        LapTime = 0;
        currentLap++;
    }
}
