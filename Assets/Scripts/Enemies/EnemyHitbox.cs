using Scripts.Player;
using UnityEngine;

namespace Scripts.Enemies
{
    /// <summary>
    /// If an enemy has a collider which is not on the same object as the Enemy.cs,
    /// You can add this script to those colliders
    /// </summary>
    [DisallowMultipleComponent]
    public class EnemyHitbox : MonoBehaviour
    {
        [SerializeField] private Enemy _parentEnemy;
        
        private void Awake()
        {
            if (_parentEnemy == null)
            {
                _parentEnemy = GetComponentInParent<Enemy>();
    
                if (_parentEnemy == null)
                    Debug.LogWarning("EnemyHitbox.cs placed on an object with no parent Enemy.cs...", this);
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth player))
                _parentEnemy.HandlePlayerCollision(player);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealth player))
                _parentEnemy.HandlePlayerCollision(player);
        }
    }
}