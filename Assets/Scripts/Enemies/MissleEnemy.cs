using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemies
{
    public class MissleEnemy : Enemy
    {
        private List<Transform> _points;
        protected override bool OverrideOnCollision => true;

        [SerializeField] private GameObject _explosion;
        private bool _explosionPlaced = false;

        private void Start()
        {
            _points = GetComponentInParent<EnemyPositionsHolder>().misslePositions;
            ChooseNewPos();
        }

        private void ChooseNewPos()
        {
            Vector3 pos = (_points[Random.Range(0, _points.Count)].position);
            pos.y = 100f;
            transform.position = pos;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_explosionPlaced) return;
            ContactPoint contact = collision.GetContact(0);
            GameObject explosion = Instantiate(_explosion, contact.point, Quaternion.LookRotation(contact.normal));
            _explosionPlaced = true;
            StartCoroutine(Explosion(explosion, 5f));
            Vector3 pos = (_points[Random.Range(0, _points.Count)].position);
            pos.y = 100f;
            transform.position = pos;
        }

        private IEnumerator Explosion(GameObject explosion, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                explosion.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 35, elapsed / duration);
                yield return null;
            }
            ChooseNewPos();
            Destroy(explosion);
            _explosionPlaced = false;
            yield return null;
        }
    }
}