using UnityEngine;

namespace Scripts.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        private readonly int maxHealth = 100;
        private int health;
        public int Health { get => health; private set => health = Mathf.Max(0, value); }

        private void Awake()
        {
            health = maxHealth;
        }
        public void DecreaseHealth(int amount)
        {
            if (amount == 0)
            {
                Debug.LogError("Trying to decrease health by zero...");
            }
            if (health == 0)
            {
                Debug.LogError("trying to decrease health when it is already at zero...");
            }

            int prevHealth = health;
            Health -= amount;
            Debug.Log($"Decreased health from {prevHealth} to {health}");
        }
        public void IncreaseHealth(int amount)
        {
            if (amount == 0)
            {
                Debug.LogError("Trying to increase health by zero...");
            }

            int prevHealth = health;
            Health += amount;
            Debug.Log($"Increased health from {prevHealth} to {health}");
        }
    }
}