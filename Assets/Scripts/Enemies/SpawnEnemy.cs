using Scripts.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemies
{
    [DisallowMultipleComponent]
    public class SpawnEnemy : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _easyGroup = new();
        [SerializeField] private List<GameObject> _normalGroup = new();
        [SerializeField] private List<GameObject> _hardGroup = new();

        [SerializeField] private float _scaling = 100f;

        public void OnAnnoyanceIncrease(float annoyanceAmount)
        {
            SpawnEnemyFromAnnoyance(annoyanceAmount);
        }

        private void SpawnEnemyFromAnnoyance(float annoyanceAmount)
        {
            float annoyanceScaling = Mathf.Clamp01(annoyanceAmount / _scaling);

            float easyChance = Mathf.Lerp(98f, 1f, annoyanceScaling);
            float normalChance = Mathf.Lerp(1f, 50f, Mathf.Sin(annoyanceScaling * Mathf.PI));
            float hardChance = Mathf.Lerp(1f, 98f, annoyanceScaling);

            float total =  easyChance + normalChance + hardChance;
            float chance = Random.Range(0f, total);

            if (chance < easyChance)
                SpawnEasyEnemy();
            else if (chance < easyChance + normalChance)
                SpawnNormalEnemy();
            else
                SpawnHardEnemy();
        }

        private void SpawnEasyEnemy()
        {
            Debug.Log("EasyEnemySpawned");
        }
        private void SpawnNormalEnemy()
        {
            Debug.Log("NormalEnemySpawned");
        }
        private void SpawnHardEnemy()
        {
            Debug.Log("HardEnemySpawned");
        }
    }
}