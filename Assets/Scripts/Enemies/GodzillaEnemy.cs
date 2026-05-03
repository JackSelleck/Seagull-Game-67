using Scripts.Player;
using UnityEngine;

public class GodzillaEnemy : Enemy
{
    [SerializeField] private Transform lazer;
    [SerializeField] private float laserSpeed = 0.2f;
    [SerializeField] private Vector3 rotation = new(30f, 60f, 90f);

    protected override void SlowedEnemyUpdate()
    {
        lazer.Rotate(laserSpeed * Time.deltaTime * rotation);
    }

    protected override void OnPlayerHit(PlayerHealth player){}
}
