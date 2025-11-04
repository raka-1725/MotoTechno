using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    CountDownStart mCountDownStart;
    PositionManager mPositionManager;

    [Header("NPC")]
    [SerializeField] private GameObject mNPC_Motorcycle_Prefab;

    [Header("Player")]
    [SerializeField] private GameObject mPlayerPrefab;

    [Header("Grids")]
    public List<Transform> startingGrid = new List<Transform>();

    [Header("Race Config")]
    public int NumberOfNPC;
    public int PlayerStartGrid;
    public bool StartFromLast;
    public bool StartFromFirst;

    private void Awake()
    {
        startingGrid.Clear();
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("StartGrid")) 
            {
                startingGrid.Add(child);
            }
        }

        mPositionManager = GetComponent<PositionManager>();
        mCountDownStart = GetComponent<CountDownStart>();

    }

    private void Start()
    {
        if (RaceConfig.bCanLoad == false) { return; }
        NumberOfNPC = RaceConfig.NumberOfNPC;
        StartFromFirst = RaceConfig.StartFromFirst;
        StartFromLast = RaceConfig.StartFromLast;
        PlayerStartGrid = RaceConfig.PlayerStartGrid;
        Debug.Log(NumberOfNPC);
        CreateGrid();
    }

    public void CreateGrid() 
    {
        int startingGridIndex = 0;

        if (StartFromFirst)
        {
            PlayerStartGrid = 0;
        }
        else if (StartFromLast) 
        {
            PlayerStartGrid = NumberOfNPC;
        }
        if (!StartFromFirst && !StartFromLast) 
        {
            PlayerStartGrid = Random.Range(2, NumberOfNPC + 1);
        }
        GameObject motorcyclePlayer = Instantiate(mPlayerPrefab, startingGrid[PlayerStartGrid].transform.position, Quaternion.Euler(0, 270, 0));
        mPositionManager.mVehicles.Add(motorcyclePlayer);
        motorcyclePlayer.GetComponent<VehiclePosition>().vehicleIndex = 1;
        for (int npc = 0; npc < NumberOfNPC; npc++)
        {
            if (PlayerStartGrid == startingGridIndex)
            {
                startingGridIndex++; 
            }
            GameObject motorcycleNpc = Instantiate(mNPC_Motorcycle_Prefab, startingGrid[startingGridIndex].transform.position, Quaternion.Euler(0,90,0));
            MotorCycleController_NPC npcScript = motorcycleNpc.gameObject.GetComponent<MotorCycleController_NPC>();
            npcScript.SetStartTransform(startingGrid[startingGridIndex].transform);
            mPositionManager.mVehicles.Add(motorcycleNpc);
            motorcycleNpc.GetComponent<VehiclePosition>().vehicleIndex = npc + 2;
            startingGridIndex++;
        }
        mCountDownStart.StartCountDown();
    }
}