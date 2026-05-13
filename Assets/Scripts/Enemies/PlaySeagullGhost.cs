using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemies
{
    [DisallowMultipleComponent]
    public class PlaySeagullGhost : Enemy
    {
        private RecordSeagullGhost _recordGhost;
        [SerializeField] private Animator _animator;
        [Tooltip("How many seconds behind the ghost follows")]
        [SerializeField] private float _delaySeconds = 3.67f;
        private bool _playing = false;

        // Cached previous frame animator values
        private bool _prevMoving;
        private bool _prevGrounded;
        private bool _prevGlide;
        private bool _prevIdle;
        private bool _prevSprinting;

        private void Start()
        {
            _delaySeconds = Random.Range(2f, 10f);
            _recordGhost = GetComponentInParent<EnemyPositionsHolder>().recordSeagullGhost;
            if (_recordGhost == null )
            {
                Debug.LogError("Couldnt find record ghost in " + transform.parent.name);
            }
            _recordGhost._ghostPlayback.Add(this);
            _recordGhost.PlayNewGhost(this);
        }

        public void Play(List<GhostFrame> ghostData)
        {
            _playing = true;
            _animator.speed = 0f;
        }

        private void Update()
        {
            if (!_playing) return;
            if (_recordGhost.Frames.Count == 0) return;

            float targetTime = _recordGhost.CurrentTime - _delaySeconds;

            // find the next frame after targetTime
            int index = 0;
            while (_recordGhost.Frames[index].Time < targetTime)
                index++;

            GhostFrame frame = _recordGhost.Frames[index];

            // lerp from wherever the ghost currently is to the next recorded snapshot
            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, frame.Position, Time.deltaTime * (1f / _recordGhost.recordInterval)),
                Quaternion.Lerp(transform.rotation, frame.Rotation, Time.deltaTime * (1f / _recordGhost.recordInterval)));

            // play animations
            if (frame.IsMoving != _prevMoving) { _animator.SetBool(PlayerAnimHash.Moving, frame.IsMoving); _prevMoving = frame.IsMoving; }
            if (frame.IsGrounded != _prevGrounded) { _animator.SetBool(PlayerAnimHash.Grounded, frame.IsGrounded); _prevGrounded = frame.IsGrounded; }
            if (frame.IsGliding != _prevGlide) { _animator.SetBool(PlayerAnimHash.Glide, frame.IsGliding); _prevGlide = frame.IsGliding; }
            if (frame.IsIdle != _prevIdle) { _animator.SetBool(PlayerAnimHash.Idle, frame.IsIdle); _prevIdle = frame.IsIdle; }
            if (frame.IsSprinting != _prevSprinting) { _animator.SetBool(PlayerAnimHash.SprintButton, frame.IsSprinting); _prevSprinting = frame.IsSprinting; }
            _animator.Play(frame.animStateHash, 0, frame.normalizedTime);
        }
    }
} 