using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemies
{
    [DisallowMultipleComponent]
    public class BallEnemy : Enemy
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<Transform> _points;
        [SerializeField] private float _speedAmplifier = 5f;
        [SerializeField] private float _arcAmplifier = 2f;

        private Vector3 _start;
        private Vector3 _end;

        private int _currentIndex = 0;
        private float _moveDuration = 1f;
        private float _time = 0f;

        private void Awake()
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody>();   
        }

        private void OnEnable()
        {
            _points = GetComponentInParent<EnemyPositionsHolder>().ballEnemyPositions;
            NextLocation(_currentIndex);
        }

        private void Start()
        {
            if (_points.Count < 0)
            {
                Debug.Log("Ball enemy has no assigned positions!");
                return;
            }
        }

        private void FixedUpdate()
        {
            if (_points.Count < 0)
            {
                Debug.Log("Ball enemy has no assigned positions!");
                return;
            }

            _time += Time.fixedDeltaTime;
            float t = _time / _moveDuration;

            if (t >= 0.95f)
            {
                transform.localScale = new Vector3(transform.localScale.x, 1.67f, transform.localScale.z);
            }
            if (t >= 1f)
            {
                _rb.MovePosition(_end);
                NextLocation(_currentIndex++);
                transform.localScale = new Vector3(transform.localScale.x, 2.67f, transform.localScale.z);
                return;
            }

            Vector3 pos = Vector3.Lerp(_start, _end, t);


            float distance = Vector3.Distance(_start, _end);
            float height = Mathf.Sin(t * Mathf.PI) * _arcAmplifier * distance; 
            pos.y += height;

            _rb.MovePosition(pos);
        }

        private void NextLocation(int index)
        {
            _time = 0f;
            _audioSource.Play();

            if (index >= _points.Count)
            {
                index = 0;
                _currentIndex = 0;
            }

            _start = transform.position;
            _end = _points[index].position;

            float distance = (_start - _end).magnitude / 4;
            _moveDuration = distance / _speedAmplifier;
        }
    }
}