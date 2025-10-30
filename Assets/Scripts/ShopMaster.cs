using System.Collections.Generic;
using UnityEngine;

public class ShopMaster : MonoBehaviour
{
    [SerializeField] MotorCycleCusomization mCustomSpec;

    
    [SerializeField] private int currentPoints;

    [SerializeField] private List<StatusUpgrade> mUpgrades;

    private void Start()
    {
        mCustomSpec = new MotorCycleCusomization();
        mUpgrades = new List<StatusUpgrade>();
    }

    public void SetUpgrades() 
    {
        foreach (StatusUpgrade statusupgrades in mUpgrades) 
        {
            
        }
    }
}
