using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class PlayerHealth : MonoBehaviour
    {
        private readonly int _maxHealth = 100;
        private int _health;
        public int Health { get => _health; private set => _health = Mathf.Max(0, value); }

        private void Awake()
        {
            _health = _maxHealth;
        }
        public void DecreaseHealth(int amount)
        {
            if (amount == 0)
            {
                Debug.LogError("Trying to decrease health by zero...");
            }
            if (_health == 0)
            {
                Debug.LogError("trying to decrease health when it is already at zero...");
            }

            int prevHealth = _health;
            Health -= amount;
            Debug.Log($"Decreased health from {prevHealth} to {_health}");
        }
        public void IncreaseHealth(int amount)
        {
            if (amount == 0)
            {
                Debug.LogError("Trying to increase health by zero...");
            }

            int prevHealth = _health;
            Health += amount;
            Debug.Log($"Increased health from {prevHealth} to {_health}");
        }
    }
}