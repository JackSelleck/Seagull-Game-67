using UnityEngine;

namespace Scripts.Player
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("General flight stats")]
        [Tooltip("Maximum Z rotation of character")]
        public float maxBankAngle = 80;

        [Header("Flap Mode Stats")]
        [Tooltip("Speed whilst flapping")]
        public float flapModeForce = 10;
        [Tooltip("How fast you can turn up/down whilst in flap mode")]
        public float flapModeVerticalRot = 100;
        [Tooltip("How fast you can turn left/right whilst in flap mode")]
        public float flapModeHorizontalRot = 100;
        [Tooltip("Gravity intensity whilst in flap mode")]
        public float flapModeGravity = 2;

        [Header("Glide Mode Stats")]
        [Tooltip("Speed whilst gliding")]
        public float glideModeForce = 15;
        [Tooltip("How fast you can turn up/down whilst in glide mode")]
        public float glideModeVerticalRot = 50;
        [Tooltip("How fast you can turn left/right whilst in glide mode")]
        public float glideModeHorizontalRot = 50;
        [Tooltip("Gravity intensity whilst in gliding mode")]
        public float glidingModeGravity = 6;

        [Header("Walk Mode Stats")]
        [Tooltip("Walk Mode Smoothing")]
        public float smoothTime = 0.2f;
        [Tooltip("Gravity intensity whilst in walking mode")]
        public float groundedGravity = 7;
        public float walkSpeed = 1.67f;
        public float sprintSpeed = 2.67f;
        public float jumpForce = 2;
    }
}