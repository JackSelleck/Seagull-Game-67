using UnityEngine;
using Unity.Cinemachine;

namespace SeagullMovementSystem
{
    // smoothly clamp upward rotation on cinemachine camera
    public class ClampCamera : CinemachineExtension
    {
        [SerializeField] private float _minPitch = -10f;
        [SerializeField] private float _maxPitch = 60f;
        [Tooltip("How far from limit easing starts")]
        [SerializeField] private float _easeRange = 15f;
        [Tooltip("How quickly camera smooths to clamped pitch")]
        [SerializeField] private float _smoothTime = 0.15f;

        private float currentPitch;
        private float pitchVelocity;
        private bool initialised = false;

        // overrides the cinemachine camera
        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state,
            float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                Vector3 euler = state.RawOrientation.eulerAngles;
                float rawPitch = euler.x > 180f ? euler.x - 360f : euler.x;

                if (!initialised)
                {
                    currentPitch = rawPitch;
                    initialised = true;
                }

                float targetPitch = SoftClamp(rawPitch, _minPitch, _maxPitch, _easeRange);

                currentPitch = Mathf.SmoothDamp(
                    currentPitch,
                    targetPitch,
                    ref pitchVelocity,
                    _smoothTime,
                    Mathf.Infinity,
                    deltaTime
                );

                state.RawOrientation = Quaternion.Euler(currentPitch, euler.y, euler.z);
            }
        }

        // Soften transition to stopping 
        private float SoftClamp(float value, float min, float max, float range)
        {
            // Upward clamp
            float upperStart = max - range;
            if (value > upperStart)
            {
                float t = Mathf.Clamp01((value - upperStart) / range);
                t = Mathf.SmoothStep(0f, 1f, t);
                return Mathf.Lerp(upperStart, max, t);
            }
            // Downward clamp
            float lowerStart = min + range;
            if (value < lowerStart)
            {
                float t = Mathf.Clamp01((lowerStart - value) / range);
                t = Mathf.SmoothStep(0f, 1f, t);
                return Mathf.Lerp(lowerStart, min, t);
            }

            return value;
        }
    }
}