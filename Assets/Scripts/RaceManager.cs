using System;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    LapChecker mLapChecker;
    FinishRaceUI mFinishRaceUI;
    DisplayRaceStats mRaceStats;

    public int totalLap;
    public int currentLap;

    public int position;

    public bool bFinalLap;
    public bool bRaceFinished;

    public Action<RaceManager> onRaceFinished;

    private void Start()
    {
        mFinishRaceUI = GetComponent<FinishRaceUI>();
        mRaceStats = GetComponent<DisplayRaceStats>();
        mLapChecker = FindAnyObjectByType<LapChecker>();
    }
    public void SetTotalLaps(int total) 
    {
        totalLap = total;
    }

    private void Update()
    {
        if (mLapChecker == null)
        {
            mLapChecker = FindAnyObjectByType<LapChecker>();
            if (mLapChecker == null)
                return;
        }

        currentLap = mLapChecker.currentLap;

        if (currentLap == totalLap)
        {
            FinalLap();
        }

        if (bFinalLap && currentLap > totalLap) 
        {
            FinishRace();
        }
    }

    private void FinalLap() 
    {
        mRaceStats.IndicateFinalLap();
        bFinalLap = true;
    }

    private void FinishRace() 
    {
        mFinishRaceUI.Finish();
        onRaceFinished?.Invoke(this);
    }


}
