using UnityEngine;

public class VehiclePosition : MonoBehaviour
{
    PositionManager mPositionManager;
    public int vehicleIndex;
    public int checkPointPassed;

    public int position;

    private void Awake()
    {
        mPositionManager = FindAnyObjectByType<PositionManager>();
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
            mPositionManager.ComparePosition(vehicleIndex);
        }
    }
}
