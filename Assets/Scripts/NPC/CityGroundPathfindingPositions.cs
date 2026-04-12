using UnityEngine;

public class CityGroundPathfindingPositions : MonoBehaviour
{
    public Transform[] targets;

    public Vector3 NewPosition()
    {
        int randomIndex = Random.Range(0, targets.Length);
        Transform newTarget = targets[randomIndex];
        return newTarget.position;
    }
}
