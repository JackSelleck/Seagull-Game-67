using UnityEngine;
using UnityEngine.AI;

namespace Scripts.NPCs
{
    [DisallowMultipleComponent]
    public class CityPathfinding : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private CityGroundPathfindingPositions _groundPathfindingPositions;

        private float timer;
        private readonly float interval = 1f;

        void Start()
        {
            _agent.SetDestination(_groundPathfindingPositions.NewPosition());
        }
    
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0f;

                if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
                {
                    _agent.SetDestination(_groundPathfindingPositions.NewPosition());
                }
            }
        }
    }
} 