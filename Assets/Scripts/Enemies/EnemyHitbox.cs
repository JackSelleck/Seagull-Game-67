using Scripts.Player;
using UnityEngine;

namespace Scripts.Enemies
{
    /// <summary>
    /// If an enemy has a collider which is not on the same object as the Enemy.cs,
    /// You can add this script to those colliders
    /// </summary>
    public class EnemyHitbox : MonoBehaviour
    {
        [SerializeField] private Enemy parentEnemy;
        
        private void Awake()
        {
            if (parentEnemy == null)
            {
                parentEnemy = GetComponentInParent<Enemy>();
    
                if (parentEnemy == null)
                    Debug.LogWarning("EnemyHitbox.cs placed on an object with parent Enemy.cs...", this);
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth player))
                parentEnemy.HandlePlayerCollision(player);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealth player))
                parentEnemy.HandlePlayerCollision(player);
        }
    }
}