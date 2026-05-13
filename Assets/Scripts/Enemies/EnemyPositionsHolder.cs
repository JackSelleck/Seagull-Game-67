using Scripts.Enemies;
using System.Collections.Generic;
using UnityEngine;

// Some enemies need certain positions in the scene to reference from upon instantion to work properly, they are stored here
public class EnemyPositionsHolder : MonoBehaviour
{
    [Header("References")]
    public RecordSeagullGhost recordSeagullGhost;
    [Header("Positions")]
    [Header("Ball")]
    public List<Transform> ballEnemyPositions = new();
    [Header("Godzilla")]
    public List<Transform> godzillaEnemyPositions = new();
    public Transform godzillaEnemyLookPos;
}
