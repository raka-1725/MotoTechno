using System.Collections.Generic;
using UnityEngine;

public class NPC_Path : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;

        Transform[] pathTrans = GetComponentsInChildren<Transform>();
        waypoints = new List<Transform>();

        for (int i = 0; i < pathTrans.Length; i++) 
        {
            if (pathTrans[i] != transform) 
            {
                waypoints.Add(pathTrans[i]);
            }
        }

        for (int i = 0; i < waypoints.Count; i++) 
        {
            Vector3 currentPoint = waypoints[i].position;
            Vector3 previousPoint = Vector3.zero;
            if (i > 0)
            {
                previousPoint = waypoints[i - 1].position;
            }
            else if (i == 0 && waypoints.Count > 1) 
            {
                previousPoint = waypoints[waypoints.Count - 1].position;
            }
            Gizmos.DrawLine(previousPoint, currentPoint);
        }
    }
}
