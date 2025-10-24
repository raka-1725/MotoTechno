using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    StartManager mStartManager;
    NPC_Path mCheckPoints;

    int totalVehicles;
    int totalCheckPoints;

    public List<GameObject> mVehicles;
    private void Awake()
    {
        mStartManager = FindAnyObjectByType<StartManager>();
        mCheckPoints = FindAnyObjectByType<NPC_Path>();

        totalVehicles = mStartManager.NumberOfNPC + 1;
        totalCheckPoints = mCheckPoints.waypoints.Count;
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        SetCarPosition();
    }
    void SetCarPosition() 
    {
        for (int i = 0; i < totalVehicles; i++) 
        {
            mVehicles[i].GetComponent<VehiclePosition>().position = i + 1;
            mVehicles[i].GetComponent <VehiclePosition>().vehicleIndex = i;
        }
        
    }

    public void ComparePosition(int vehicleIndex) 
    {
        if (mVehicles[vehicleIndex].GetComponent<VehiclePosition>().position <= 1)
            return;
        GameObject currentVehicle = mVehicles[vehicleIndex];
        int currentPos = currentVehicle.GetComponent<VehiclePosition>().position;
        int currentCheckPointIndex = currentVehicle.GetComponent<VehiclePosition>().checkPointPassed;

        GameObject vehicleInfront = null;
        int inFrontPos = 0;
        int inFrontCheckPointindex = 0;

        for (int i = 0; i < totalVehicles; i++) 
        {
            if (mVehicles[i].GetComponent<VehiclePosition>().position == currentPos - 1) 
            {
                vehicleInfront = mVehicles[i];
                inFrontPos = vehicleInfront.GetComponent<VehiclePosition>().position;
                inFrontCheckPointindex = vehicleInfront.GetComponent<VehiclePosition>().checkPointPassed;
                break;
            }
        }

        if (currentCheckPointIndex > inFrontCheckPointindex) 
        {
            currentVehicle.GetComponent<VehiclePosition>().position = currentPos - 1;
            vehicleInfront.GetComponent<VehiclePosition>().position = inFrontPos + 1;
        }
    }
}
