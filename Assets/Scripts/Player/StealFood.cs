using Scripts.UI;
using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class StealFood : MonoBehaviour
    {
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private AnnoyanceManager _annoyanceManager;
        private bool _canSteal = false;

        private void Awake()
        {
            if (_uiManager == null)
            {
                _uiManager = GameObject.FindWithTag(TagConstants.MainCanvas).GetComponent<UIManager>();

                if (_uiManager == null)
                {
                    Debug.LogError("StealFood.cs Unable to find UIManager!");
                }
            }

            if (_annoyanceManager == null)
                _annoyanceManager = GetComponent<AnnoyanceManager>();        
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
                _uiManager.StealFoodActionActiveSetter(true);
                _annoyanceManager.IncreaseAnnoyance(5);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TagConstants.NPC))
            {
                _canSteal = false;
                _uiManager.StealFoodActionActiveSetter(false);
            }
        }
    }
}