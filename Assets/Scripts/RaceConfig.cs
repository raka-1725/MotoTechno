using UnityEngine;

public static class RaceConfig
{
    public static int NumberOfNPC;
    public static int PlayerStartGrid;
    public static bool StartFromLast;
    public static bool StartFromFirst;
    public static int TotalLaps;

    public static bool bCanLoad;

    public static void Reset() 
    {
        NumberOfNPC = 0;
        PlayerStartGrid = 1;
        StartFromLast = false;
        StartFromFirst = false;
        TotalLaps = 0;
        bCanLoad = false;
    }

}
