using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class ShootPoop : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Layers to allow poop on")]
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _range = 100f;
        [SerializeField] private float _spawnTime = 0.1f;
        [Tooltip("How much player speed affects poop forward velocity")]
        [SerializeField] private float _velocityInfluence = 0.5f;
        [Space]
        [Header("References")]
        [SerializeField] private UnityEngine.Camera _cam;
        [SerializeField] private List<Transform> _poopPool;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private GameObject _poopSprite;

        private float _timer;

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _spawnTime)
            {
                _timer = 0f;
                PoopSpriteHitSurface();
            }
        }

        private void PoopSpriteHitSurface()
        {
            // movement direction of bird
            Vector3 velocityDir = new Vector3(_rb.linearVelocity.normalized.x,0,_rb.linearVelocity.normalized.z);
            // how much movement speed should be considered
            float influence = Mathf.Clamp01(_rb.linearVelocity.magnitude / 10f) * _velocityInfluence;
            // Final direction of poop based on speed and direction, allows to be spawned ahead of the bird when going fast
            Vector3 rayDir = (Vector3.down + velocityDir * influence).normalized;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDir, out hit, _range, _layerMask, QueryTriggerInteraction.Ignore))
            {
                // spawn poop sprite at hit point
                // use pooled poops first
                if (_poopPool.Count > 0)
                {
                    _poopPool[0].position = hit.point + hit.normal * 0.01f;
                    _poopPool[0].rotation = Quaternion.LookRotation(hit.normal);
                    _poopPool[0].parent = hit.collider.transform;
                    _poopPool[0].Rotate(0f, 0f, Random.Range(0f, 360f));
                    _poopPool.RemoveAt(0);
                }
                else // spawn in poops if pool is exhausted
                {
                    GameObject PoopSprite = Instantiate(
                    original: _poopSprite,
                    position: hit.point + hit.normal * 0.01f,
                    rotation: Quaternion.LookRotation(hit.normal),
                    parent: hit.collider.transform
                    );

                    // random rotation to look different from eachover
                    PoopSprite.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
                }


            }
        }
    }
}