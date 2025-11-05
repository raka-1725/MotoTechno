using UnityEngine;

public class VehiclePosition : MonoBehaviour
{
    DisplayRaceStats mDisplayRaceStats;

    public bool bIsPlayer;

    PositionManager mPositionManager;
    StartManager mStartManager;
    CountDownStart mCountDownStart;
    RaceManager mRaceManager;
    public int vehicleIndex;
    public int checkPointPassed;

    public int position;
    public int laps;
    public float distanceAlongTrack;

    private bool bStared = false;

    private void Awake()
    {
        mPositionManager = FindAnyObjectByType<PositionManager>();
        mStartManager = FindAnyObjectByType<StartManager>();
        mCountDownStart = FindAnyObjectByType<CountDownStart>();
        if (bIsPlayer) 
        { 
            mRaceManager = FindAnyObjectByType<RaceManager>();
            mDisplayRaceStats = FindAnyObjectByType<DisplayRaceStats>();
            SetInitialPosition(mStartManager.PlayerStartGrid + 1);
            mDisplayRaceStats.UpdatePositionUI(position);
        }
        else 
        { 
            mDisplayRaceStats = null;
            mRaceManager = null;
        }

        mCountDownStart.onRaceStart += RaceStart;
    }
    private void RaceStart(CountDownStart sender)
    {
        bStared = true;
    }

    public void SetInitialPosition(int startpos) 
    {
        position = startpos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint")) 
        {
            checkPointPassed += 1;
        }
    }

    private void Update()
    {
        if (!bIsPlayer) return;
        UpdatePosition();
    }

    public void UpdatePosition() 
    {
        if (!bStared) return;
        mDisplayRaceStats.UpdatePositionUI(position);
        mRaceManager.position = position;
    }

    public void UpdateDistanceAlongTrack(NPC_Path path)
    {
        if (path == null || path.waypoints.Count == 0)
            return;
        int checkpointCount = path.waypoints.Count;
        distanceAlongTrack = 0f;
        int safePassed = Mathf.Clamp(checkPointPassed, 0, checkpointCount - 1);

        for (int i = 0; i < safePassed; i++)
        {
            int nextIndex = (i + 1) % checkpointCount;
            distanceAlongTrack += Vector3.Distance(path.waypoints[i].position, path.waypoints[nextIndex].position);

        }

        int currentIndex = safePassed % checkpointCount;
        Vector3 currentWp = path.waypoints[currentIndex].position;
        int nextWpIndex = (currentIndex + 1) % checkpointCount;
        Vector3 nextWp = path.waypoints[nextWpIndex].position;
        float distanceToNext = Vector3.Distance(transform.position, nextWp);


        if (distanceToNext < 5f)
        {
            checkPointPassed ++;

            if (checkPointPassed >= checkpointCount)
            {
                checkPointPassed = 0;
                laps++;
            }
        }


        float segmentLength = Vector3.Distance(currentWp, nextWp);
        float segmentProgress = Vector3.Distance(currentWp, transform.position);
        segmentProgress = Mathf.Clamp(segmentProgress, 0f, segmentLength);
        distanceAlongTrack += segmentProgress;
        distanceAlongTrack += laps * path.totoalLength;
    }
}
