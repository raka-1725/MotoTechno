using UnityEngine;

public class SetRaceConfig : MonoBehaviour
{
    [SerializeField] NumSelector mLapSelector;
    [SerializeField] NumSelector mPositionSelector;
    [SerializeField] NumSelector mCPUSelector;


    public int NumberOfNPC;
    public int PlayerStartGrid;
    public bool StartFromLast;
    public bool StartFromFirst;
    public int TotalLaps;


    public void SetStartConfig() 
    {
        TotalLaps = mLapSelector.number;
        PlayerStartGrid = mPositionSelector.number;
        NumberOfNPC = mCPUSelector.number;

        RaceConfig.Reset();
        if (PlayerStartGrid == 1)
        {
            StartFromFirst = true;
            StartFromLast = false;
            RaceConfig.StartFromFirst = StartFromFirst;
        }
        else if (PlayerStartGrid == NumberOfNPC + 1)
        {
            StartFromLast = true;
            StartFromFirst = false;
            RaceConfig.StartFromLast = StartFromLast;
        }
        else 
        {
            StartFromLast = false;
            StartFromFirst = false;
            RaceConfig.PlayerStartGrid = PlayerStartGrid;
        }

        RaceConfig.NumberOfNPC = NumberOfNPC;
        RaceConfig.TotalLaps = TotalLaps;

        RaceConfig.bCanLoad = true;
    }

    private void Reset()
    {
        NumberOfNPC = 0;
        PlayerStartGrid = 1;
        StartFromLast = false;
        StartFromFirst = true;
        TotalLaps = 1;
    }
}
