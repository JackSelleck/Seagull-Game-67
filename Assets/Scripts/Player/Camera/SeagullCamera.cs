using UnityEngine;

namespace Scripts.Player.Camera
{
    [DisallowMultipleComponent]
    public class SeagullCamera : MonoBehaviour
    {
        [SerializeField] private Transform _seagull;
        [SerializeField] private Vector3 _offset = new(0f, 1.5f, 0f);
        [SerializeField] private float _rotationSmoothTime = 0.15f;
        [SerializeField] private float _positionSmoothTime = 0.05f;

        private float yawVelocity;
        private float currentYaw;
        private Vector3 posVelocity;

        private void Awake()
        {
            if (_seagull == null)
            {
                _seagull = GameObject.FindGameObjectWithTag(TagConstants.Player).transform;
            }
        }
        private void LateUpdate()
        {
            float targetYaw = _seagull.eulerAngles.y;
            currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);

            Vector3 targetPos = _seagull.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref posVelocity, _positionSmoothTime);
        }
    }
}