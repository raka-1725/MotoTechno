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
    public int awardCredit;

    public bool bFinalLap;
    public bool bRaceFinished;

    public Action<RaceManager> onRaceFinished;

    private void Start()
    {
        mFinishRaceUI = GetComponent<FinishRaceUI>();
        mRaceStats = GetComponent<DisplayRaceStats>();
        mLapChecker = FindAnyObjectByType<LapChecker>();

        totalLap = RaceConfig.TotalLaps;
        SetTotalLaps(totalLap);
    }
    void SetTotalLaps(int total) 
    {
        totalLap = total;
        mRaceStats.SetTotalLaps(total);
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

        if (!bFinalLap && currentLap == totalLap)
        {
            FinalLap();
        }

        if (!bRaceFinished && currentLap >= totalLap) 
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
        AwardPoints();
        bRaceFinished = true;
        mFinishRaceUI.Finish(this);
        onRaceFinished?.Invoke(this);

    }

    private void AwardPoints() 
    {
        switch (position) 
        {
            case 1 :
                awardCredit = 20;
                break;
            case 2:
                awardCredit = 15;
                break;
            case 3:
                awardCredit = 12;
                break;
            case 4:
                awardCredit = 10;
                break;
            case 5:
                awardCredit = 8;
                break;
            case 6:
                awardCredit = 6;
                break;
            case 7:
                awardCredit = 4;
                break;
            case 8:
                awardCredit = 3;
                break;
            case 9:
                awardCredit = 2;
                break;
            case 10:
                awardCredit = 1;
                break;
            case 11:
                awardCredit = 0;
                break;
            case 12:
                awardCredit = 0;
                break;
            case 13:
                awardCredit = 0;
                break;
            case 14:
                awardCredit = 0;
                break;
            case 15:
                awardCredit = 0;
                break;
            default:
                break;
        }

        PlayerCredits.Instance.addMoney(awardCredit);
        Debug.Log($"Player credit : {PlayerCredits.Instance.credit}");
    }


}
