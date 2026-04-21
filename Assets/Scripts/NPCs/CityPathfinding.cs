using UnityEngine;
using UnityEngine.AI;

namespace Scripts.NPCs
{
    [DisallowMultipleComponent]
    public class CityPathfinding : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private CityGroundPathfindingPositions groundPathfindingPositions;
        
        void Start()
        {
            agent.SetDestination(groundPathfindingPositions.NewPosition());
        }
    
        void Update()
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                agent.SetDestination(groundPathfindingPositions.NewPosition());
            }
        }
    }
}