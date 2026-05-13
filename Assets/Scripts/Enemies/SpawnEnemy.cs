using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts.Enemies
{
    [DisallowMultipleComponent]
    public class SpawnEnemy : MonoBehaviour
    {
        [SerializeField] private Transform _enemyPositionsHolder;
        [SerializeField] private TextMeshProUGUI _enemyAnnouncementText;

        [SerializeField] private List<GameObject> _easyGroup = new();
        [SerializeField] private List<GameObject> _normalGroup = new();
        [SerializeField] private List<GameObject> _hardGroup = new();

        [SerializeField] private float _scaling = 100f;

        private Coroutine _coroutine;

        private readonly Queue<GameObject> _easyPool = new();
        private readonly Queue<GameObject> _normalPool = new();
        private readonly Queue<GameObject> _hardPool = new();

        private const int PoolSize = 20;

        private void Awake()
        {
            if (_enemyPositionsHolder == null)
            {
                _enemyPositionsHolder = FindFirstObjectByType<EnemyPositionsHolder>().transform;
            }
        }
        private void Start()
        {
            LoadEnemyPool(_easyGroup, _easyPool);
            LoadEnemyPool(_normalGroup, _normalPool);
            LoadEnemyPool(_hardGroup, _hardPool);
        }
        private void LoadEnemyPool(List<GameObject> group, Queue<GameObject> pool)
        {
            for (int i = 0; i < PoolSize; i++)
            {
                GameObject prefab = group[Random.Range(0, group.Count)];
                GameObject instance = Instantiate(prefab, _enemyPositionsHolder);
                instance.SetActive(false);
                pool.Enqueue(instance);
            }
        }

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
                SpawnEnemyFromPool(_easyGroup, _easyPool);
            else if (chance < easyChance + normalChance)
                SpawnEnemyFromPool(_normalGroup, _normalPool);
            else
                SpawnEnemyFromPool(_hardGroup, _hardPool);
        }

        private void SpawnEnemyFromPool(List<GameObject> group, Queue<GameObject> pool)
        {
            SpawnFromPool(group, pool);
            Debug.Log("EasyEnemySpawned");
        }

        private void SpawnFromPool(List<GameObject> group, Queue<GameObject> pool)
        {
            GameObject prefab = group[Random.Range(0, group.Count)];

            GameObject enemy;

            if (pool.Count > 0)
            {
                enemy = pool.Dequeue();
            }
            else // Instantiate new enemy if pool is exhausted
            {
                enemy = Instantiate(prefab, _enemyPositionsHolder);
            }

            _coroutine = StartCoroutine(DisplayText(enemy));
            enemy.SetActive(true);
        }

        private IEnumerator DisplayText(GameObject enemy)
        {
            _enemyAnnouncementText.text = enemy.GetComponent<Enemy>().displayName + "!!!";
            yield return new WaitForSeconds(1f);
            _enemyAnnouncementText.text = null;
        }

        public void ReturnToPool(GameObject enemy, Queue<GameObject> pool)
        {
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }
}