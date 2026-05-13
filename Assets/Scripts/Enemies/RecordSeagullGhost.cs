using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemies
{
    [DisallowMultipleComponent]
    public class RecordSeagullGhost : MonoBehaviour
    {
        public List<PlaySeagullGhost> _ghostPlayback;
        public float recordInterval = 0.05f;
        [SerializeField] private Animator _animator;

        public float CurrentTime { get; private set; }
        public IReadOnlyList<GhostFrame> Frames => _frames;

        private readonly List<GhostFrame> _frames = new();
        private float _recordSeconds = 12f;
        private float _nextRecordTime;
        private bool _recording;

        private void Start()
        {
            StartRecording();
            foreach(PlaySeagullGhost ghost in _ghostPlayback)
            {
                ghost.Play(_frames);
            }
        }

        public void PlayNewGhost(PlaySeagullGhost ghost)
        {
            ghost.Play(_frames);
        }

        public void StartRecording()
        {
            _frames.Clear();
            CurrentTime = 0f;
            _nextRecordTime = 0f;
            _recording = true;
        }

        public void StopRecording() => _recording = false;

        void Update()
        {
            if (!_recording) return;

            // limit recording frequency for performance
            CurrentTime += Time.deltaTime;
            if (CurrentTime < _nextRecordTime) return;
            _nextRecordTime = CurrentTime + recordInterval;

            // take a snapshot of the current player state
            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
            _frames.Add(new GhostFrame
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Time = CurrentTime,
                animStateHash = state.fullPathHash,
                normalizedTime = state.normalizedTime,

                IsMoving = _animator.GetBool(PlayerAnimHash.Moving),
                IsGrounded = _animator.GetBool(PlayerAnimHash.Grounded),
                IsGliding = _animator.GetBool(PlayerAnimHash.Glide),
                IsIdle = _animator.GetBool(PlayerAnimHash.Idle),
                IsSprinting = _animator.GetBool(PlayerAnimHash.SprintButton)
            });

            // get rid of frames older than recordSeconds to save memory
            float cutoff = CurrentTime - _recordSeconds;
            while (_frames.Count > 2 && _frames[1].Time < cutoff)
                _frames.RemoveAt(0);
        }
    }

    // Struct of all the variables needed to mimic player movement in ghosts
    public struct GhostFrame
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public float Time;
        public float normalizedTime;
        public int animStateHash;

        // animator parameters
        public bool IsMoving;
        public bool IsGrounded;
        public bool IsGliding;
        public bool IsIdle;
        public bool IsSprinting;
    }
}