using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class StealFood : MonoBehaviour
    {
        [SerializeField] private PlayerReferenceManager _playerRefs;
        private bool _canSteal = false;

        private void Awake()
        {
            if (_playerRefs == null)
            {
                _playerRefs = GetComponent<PlayerReferenceManager>();
            }
        }

        private void Update()
        {
            if (!_canSteal)
                return;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                _canSteal = true;
                _playerRefs.UIManager.StealFoodActionActiveSwitch(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                _canSteal = false;
                _playerRefs.UIManager.StealFoodActionActiveSwitch(false);
            }
        }
    }
}