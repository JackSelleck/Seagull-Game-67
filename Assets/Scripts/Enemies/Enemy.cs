using Scripts.Player;
using UnityEngine;

namespace Scripts.Enemies
{
    [DisallowMultipleComponent]
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected int _damage;
        private float _timer;
        private readonly float _interval = 0.04f;

        protected virtual void SlowedEnemyUpdate() { }
        protected virtual void OnPlayerHit(PlayerHealth player) { }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _interval)
            {
                _timer = 0f;
                SlowedEnemyUpdate();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth player))
            {
                HandlePlayerCollision(player);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealth player))
            {
                HandlePlayerCollision(player);
            }
        }
        public void HandlePlayerCollision(PlayerHealth player)
        {
            player.DecreaseHealth(_damage);
            OnPlayerHit(player);
        }
    }
}