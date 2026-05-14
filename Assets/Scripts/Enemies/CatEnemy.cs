using UnityEngine;

namespace Scripts.Enemies
{
    public class CatEnemy : Enemy
    {
        protected override bool OverrideOnTrigger => true;

        [SerializeField] private Animator _animator;
        private Quaternion _startRot;

        private void Start()
        {
            _startRot = transform.rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagConstants.Player))
            {
                transform.rotation = _startRot;
                transform.LookAt(other.transform);
                _animator.SetTrigger("Leap");
            }
        }
    }
}