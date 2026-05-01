using UnityEngine;

namespace Scripts.Player.Camera
{
    [DisallowMultipleComponent]
    public class SeagullCamera : MonoBehaviour
    {
        [SerializeField] private Transform _playerPos;
        [SerializeField] private Vector3 _offset = new(0f, 1.5f, 0f);
        [SerializeField] private float _rotationSmoothTime = 0.15f;
        [SerializeField] private float _positionSmoothTime = 0.05f;

        private float yawVelocity;
        private float currentYaw;
        private Vector3 posVelocity;

        private void Awake()
        {
            if (_playerPos == null)
            {
                _playerPos = GameObject.FindGameObjectWithTag(TagConstants.Player).transform;

                if (_playerPos == null)
                {
                    Debug.LogError("SeagullCamera.cs Unable to find Player!");
                }
            }
        }
        private void LateUpdate()
        {
            float targetYaw = _playerPos.eulerAngles.y;
            currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);

            Vector3 targetPos = _playerPos.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref posVelocity, _positionSmoothTime);
        }
    }
}