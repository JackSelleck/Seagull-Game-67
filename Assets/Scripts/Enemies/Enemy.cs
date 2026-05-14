using Scripts.Player;
using UnityEngine;

namespace Scripts.Enemies
{
    [DisallowMultipleComponent]
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private string _displayName;
        public string displayName { get => _displayName; set => _displayName = value; }
        protected virtual bool OverrideOnCollision => false;
        protected virtual bool OverrideOnTrigger => false;

        [SerializeField] protected int _damage;

        private float _timer;
        private readonly float _interval = 0.04f;

        /// <summary>
        /// Slow update method to use, in the case that a constant update can be avoided for performance
        /// </summary>
        protected virtual void SlowedEnemyUpdate() { }

        /// <summary>
        /// To apply custom logic upon hitting the player
        /// </summary>
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
                if (OverrideOnTrigger) return;
                HandlePlayerCollision(player);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealth player))
            {
                if (OverrideOnCollision) return;
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