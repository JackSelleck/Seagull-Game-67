using UnityEngine;

namespace Scripts.NPCs
{
    [DisallowMultipleComponent]
    public class CityGroundPathfindingPositions : MonoBehaviour
    {
        // List of target positions to give to npcs
        [SerializeField] private Transform[] _targets;

        public Vector3 NewPosition()
        {
            int randomIndex = Random.Range(0, _targets.Length);
            Transform newTarget = _targets[randomIndex];
            return newTarget.position;
        }
    }
}
