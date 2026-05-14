using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemies
{
    public class LazerGridEnemy : Enemy
    {
        private List<Transform> _points;

        private void Start()
        {
            _points = GetComponentInParent<EnemyPositionsHolder>().gridPositions;
            transform.position = _points[Random.Range(0, _points.Count)].position;
            transform.Rotate(0f, Random.Range(0f, 360f), 0f);
        }
    }
}