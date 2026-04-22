using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class StealFood : MonoBehaviour
    {
        [SerializeField] private PlayerManager player;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _stealFoodCG;
        private bool _canSteal = false;

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
                _stealFoodCG.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                _canSteal = false;
                _stealFoodCG.SetActive(false);
            }
        }
    }
}