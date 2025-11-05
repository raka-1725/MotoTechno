using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PositionManager : MonoBehaviour
{
    StartManager mStartManager;
    NPC_Path mCheckPoints;
    CountDownStart mCountDownStart;

    int totalVehicles;
    public int totalCheckPoints => mCheckPoints.waypoints.Count;

    public float UpdateInterval = 0.1f;
    private float updateTimer;

    public int playerStartPos;
    private bool bStarted = false;

    private float startdelay;
    private float startdelayduration = 3f;

    public List<GameObject> mVehicles;
    private void Awake()
    {
        mStartManager = FindAnyObjectByType<StartManager>();
        mCheckPoints = FindAnyObjectByType<NPC_Path>();

        totalVehicles = mStartManager.NumberOfNPC + 1;
        playerStartPos = mStartManager.PlayerStartGrid;

        mCountDownStart = FindAnyObjectByType<CountDownStart>();
        mCountDownStart.onRaceStart += RaceStart;
    }
    private void Start()
    {
        SetGrid();
    }
    private void SetGrid()
    {
        mVehicles = mVehicles
        .OrderBy(v => v.GetComponent<VehiclePosition>().position)
        .ToList();

        for (int i = 0; i < mVehicles.Count; i++)
        {
            var vp = mVehicles[i].GetComponent<VehiclePosition>();
            vp.position = i + 1;
            vp.vehicleIndex = i;
            if (vp.bIsPlayer)
            {
                vp.UpdatePosition();
            }
        }
    }
    private void RaceStart(CountDownStart sender)
    {
        bStarted = true;
    }
    private void Update()
    {
        if (!bStarted) return;
        updateTimer += Time.deltaTime;
        startdelay += Time.deltaTime;
        if (updateTimer >= UpdateInterval && startdelay > startdelayduration) 
        {
            SetCarPosition();
            updateTimer = 0;
        }
    }
    void SetCarPosition() 
    {
        foreach (GameObject v in mVehicles)
        {
            v.GetComponent<VehiclePosition>().UpdateDistanceAlongTrack(mCheckPoints);
        }
        mVehicles = mVehicles
            .OrderByDescending(v => v.GetComponent<VehiclePosition>().distanceAlongTrack)
            .ThenBy(v => v.GetComponent<VehiclePosition>().vehicleIndex)
            .ToList();
        for (int i = 0; i < mVehicles.Count; i++)
        {
            var vp = mVehicles[i].GetComponent<VehiclePosition>();
            vp.position = i + 1;
            vp.vehicleIndex = i;
        }
    }
}
