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

    [SerializeField] private int mCurrentcheckPoint = -1;

    private void Awake()
    {
        mCheckPoints = GameObject.FindGameObjectsWithTag("SectorCheckpoints");
        foreach (GameObject checkpoints in mCheckPoints) 
        {
            Checkpoint checkpoint = checkpoints.GetComponent<Checkpoint>();
            checkpoint.SetLapChecker();
        }
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
                NewLap();
            }
        }
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
