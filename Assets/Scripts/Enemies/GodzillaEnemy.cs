using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemies
{
    [DisallowMultipleComponent]
    public class GodzillaEnemy : Enemy
    {
        [Tooltip("Points the godzilla can move to")]
        [SerializeField] private List<Transform> _points;
        [SerializeField] private Vector3 _laseRotation = new(30f, 60f, 90f);
        [SerializeField] private float _laserSpeed = 0.2f;
        [Tooltip("How low the godzilla starts and lowers to")]
        [SerializeField] private float _lowerHeight = -100f;
        [Tooltip("How high the godzilla rises")]
        [SerializeField] private float _raiseHeight = 0f;
        [Tooltip("Point for godzilla to be looking at")]
        [SerializeField] private Transform _lookPos;
        [Space]
        [SerializeField] private Transform _godzilla;
        [SerializeField] private Transform _laserRotationParent;

        private bool isCurrentlyUp = false;
        private float moveTimer;
        private readonly float moveInterval = 60f;
        private int _currentIndex = 0;

        private void Start()
        {
            StartCoroutine(MoveGodzilla());
        }

        private void Update()
        {
            // Move lazer around
            _laserRotationParent.Rotate(_laserSpeed * Time.deltaTime * _laseRotation);

            // Move up and down to a new location every once in a while
            moveTimer += Time.deltaTime;
            if (moveTimer >= moveInterval)
            {
                moveTimer = 0f;
                StartCoroutine(MoveGodzilla());
                isCurrentlyUp = false;
            }
        }

        private IEnumerator MoveGodzilla()
        {
            _laserRotationParent.gameObject.SetActive(false);

            // If currently up then move down first
            if (isCurrentlyUp)
            {
                Vector3 posi = _godzilla.position;

                while (Mathf.Abs(posi.y + _lowerHeight) > 0.01f)
                {
                    Vector3 dir = _lookPos.position - posi;
                    _godzilla.forward = -dir.normalized;

                    posi.y = Mathf.MoveTowards(posi.y, - _lowerHeight, Time.deltaTime * 6.7f);
                    _godzilla.position = posi;

                    yield return null;
                }
            }

            // Choose a new position and then rise from there
            _currentIndex = Random.Range(0, _points.Count);
            Vector3 movePoint = _points[_currentIndex].position;
            _godzilla.position = new Vector3(movePoint.x, movePoint.y - _lowerHeight, movePoint.z);
            Vector3 pos = _godzilla.position;

            while (Mathf.Abs(_godzilla.position.y - _raiseHeight) > 0.01f)
            {
                Vector3 dir = _lookPos.position - pos;
                _godzilla.forward = -dir.normalized;

                pos.y = Mathf.MoveTowards(pos.y, _raiseHeight, Time.deltaTime * 6.7f);
                _godzilla.position = pos;

                yield return null;
            }

            // Start laser once risen
            _laserRotationParent.gameObject.SetActive(true);
            isCurrentlyUp = true;
        }
    }
}
