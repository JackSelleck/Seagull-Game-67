using Scripts.Player;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int damage;
    private float timer;
    private readonly float interval = 0.04f;

    protected virtual void SlowedEnemyUpdate() { }
    protected virtual void OnPlayerHit(PlayerHealth player) { }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            SlowedEnemyUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealth player))
        {
            player.DecreaseHealth(damage);
            OnPlayerHit(player);
        }
    }
}
