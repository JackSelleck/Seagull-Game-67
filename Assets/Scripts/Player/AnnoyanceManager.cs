using Scripts.Enemies;
using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class AnnoyanceManager : MonoBehaviour
    {
        private float _annoyance;
        public float Annoyance { get => _annoyance; private set => _annoyance = Mathf.Max(0, value); }

        [SerializeField] private AnnoyanceBar _annoyanceBar;
        [SerializeField] private SpawnEnemy _spawnEnemy;

        private void Awake()
        {
            if (_annoyanceBar == null)
            {
                _annoyanceBar = GameObject.FindWithTag(TagConstants.MainCanvas).GetComponentInChildren<AnnoyanceBar>();
                
                if (_annoyanceBar == null)
                {
                    Debug.LogError("AnnoyanceManager.cs Unable to find Canvas prefab!");
                }
            }

            if (_spawnEnemy == null)
            {
                _spawnEnemy = FindFirstObjectByType<SpawnEnemy>();

                if (_annoyanceBar == null)
                {
                    Debug.LogError("AnnoyanceManager.cs Unable to find Spawn Enemy prefab!");
                }
            }
        }

        public void IncreaseAnnoyance(int increaseAmount)
        {
            if (increaseAmount < 0)
            {
                Debug.LogError("Trying to increase annoyance negative amount...");
            }

            float prevAnnoyance = _annoyance;
            Annoyance += increaseAmount;
            _annoyanceBar.OnAnnoyanceIncrease(increaseAmount, Annoyance);
            _spawnEnemy.OnAnnoyanceIncrease(Annoyance);
            Debug.Log($"Increased annoyance from {prevAnnoyance} to {_annoyance}");
        }

        // Not planned for use anywhere at the moment
        public void DecreaseAnnoyance(int amount)
        {
            if (amount > 0)
            {
                Debug.LogError("Trying to decrease annoyance positive amount...");
            }

            float prevAnnoyance = _annoyance;
            Annoyance -= amount;
            Debug.Log($"Increased annoyance from {prevAnnoyance} to {_annoyance}");
        }
    }
} 