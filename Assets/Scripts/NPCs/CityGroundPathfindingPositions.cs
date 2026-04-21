using UnityEngine;

namespace Scripts.NPCs
{
    [DisallowMultipleComponent]
    public class CityGroundPathfindingPositions : MonoBehaviour
    {
        // List of target positions to give to npcs
        public Transform[] targets;

        public Vector3 NewPosition()
        {
            int randomIndex = Random.Range(0, targets.Length);
            Transform newTarget = targets[randomIndex];
            return newTarget.position;
        }
    }
}
