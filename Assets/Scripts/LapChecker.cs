using UnityEngine;

public class LapChecker : MonoBehaviour
{
    DisplayRaceStats mDisplayRaceStats;
    CountDownStart mCountDownStart;
    [SerializeField] private GameObject[] mCheckPoints;

    public bool bStopTimer = true;

    [Header("CurrentLap/Sector")]
    public int currentLap = 0;
    public int currentSector = 0;

    [Header("SectorTime")]
    [SerializeField] private float LapTime;
    [SerializeField] private float SectorTime;

    [SerializeField] private int mCurrentcheckPoint = -1;

    private void Awake()
    {
        mCheckPoints = GameObject.FindGameObjectsWithTag("SectorCheckpoints");
        mDisplayRaceStats = FindAnyObjectByType<DisplayRaceStats>();
        mCountDownStart = FindAnyObjectByType<CountDownStart>();
        foreach (GameObject checkpoints in mCheckPoints) 
        {
            Checkpoint checkpoint = checkpoints.GetComponent<Checkpoint>();
            checkpoint.SetLapChecker();
        }
        mCountDownStart.onRaceStart += RaceStart;
    }
    private void RaceStart(CountDownStart sender) 
    {
        bStopTimer = false;
    }
    public void PassCheckPoint(int checkpointIndex) 
    {
        if (mCurrentcheckPoint == -1 && checkpointIndex == 0)
        {
            mCurrentcheckPoint = 0;
            currentSector = 1;
            LapTime = 0f;
            return;
        }
        if (checkpointIndex == (mCurrentcheckPoint + 1) % mCheckPoints.Length)
        {
            mCurrentcheckPoint = checkpointIndex;
            currentSector++; currentSector = 1;
            SectorTime = UpdateSectorTime();

            if (checkpointIndex == 0) 
            {
                mDisplayRaceStats.UpdateLapUI(currentLap + 1, LapTime);
                NewLap();
            }
        }
    }

    private void Update()
    {
        LapTimer();
        mDisplayRaceStats.UpdateLapTimeUI(LapTime);
        mDisplayRaceStats.UpdateSetcorUI(SectorTime);
    }

    private float UpdateSectorTime() 
    {

        float sectortime = LapTime;


        return sectortime;
    }

    private void LapTimer() 
    {
        if (!bStopTimer) 
        {
            LapTime += Time.deltaTime;
        }
    }

    private void NewLap() 
    {
        LapTime = 0;
        currentLap++;
    }
}
