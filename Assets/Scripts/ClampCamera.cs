using UnityEngine;
using Unity.Cinemachine;

// smoothly clamp upward rotation on cinemachine camera
public class ClampCamera : CinemachineExtension
{
    [SerializeField] private float minPitch = -10f;
    [SerializeField] private float maxPitch = 60f;
    [Tooltip("How far from limit easing starts")]
    [SerializeField] private float easeRange = 15f;
    [Tooltip("How quickly camera smooths to clamped pitch")]
    [SerializeField] private float smoothTime = 0.15f;

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

            float targetPitch = SoftClamp(rawPitch, minPitch, maxPitch, easeRange);

            currentPitch = Mathf.SmoothDamp(
                currentPitch,
                targetPitch,
                ref pitchVelocity,
                smoothTime,
                Mathf.Infinity,
                deltaTime
            );

            state.RawOrientation = Quaternion.Euler(currentPitch, euler.y, euler.z);
        }
    }

    private float SoftClamp(float value, float min, float max, float range)
    {
        float upperStart = max - range;
        if (value > upperStart)
        {
            float t = Mathf.Clamp01((value - upperStart) / range);
            t = Mathf.SmoothStep(0f, 1f, t);
            return Mathf.Lerp(upperStart, max, t);
        }

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