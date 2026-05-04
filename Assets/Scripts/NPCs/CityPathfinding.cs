using UnityEngine;
using UnityEngine.AI;

namespace Scripts.NPCs
{
    [DisallowMultipleComponent]
    public class CityPathfinding : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private CityGroundPathfindingPositions groundPathfindingPositions;

        private float timer;
        private readonly float interval = 1f;

        void Start()
        {
            agent.SetDestination(groundPathfindingPositions.NewPosition());
        }
    
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0f;

                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    agent.SetDestination(groundPathfindingPositions.NewPosition());
                }
            }
        }
    }
} 