using UnityEngine;

namespace Scripts.UI 
{
    [DisallowMultipleComponent]
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _pauseMenu;
        [SerializeField] private CanvasGroup _HUD;
        [SerializeField] private CanvasGroup _stealFoodAction;
        [SerializeField] private CanvasGroup _options;
        [SerializeField] private CanvasGroup _deathScreen;

        /// <summary>
        /// Set UI elements active or inactive
        /// </summary>
        public void StartMenuActiveSetter(bool setActive)
        {
            _pauseMenu.alpha = setActive? 1 : 0;
        }
        public void HUDActiveSetter(bool setActive)
        {
            _HUD.alpha = setActive ? 1 : 0;
        }
        public void StealFoodActionActiveSetter(bool setActive)
        {
            _stealFoodAction.alpha = setActive ? 1 : 0;
        }
        public void OptionsActiveSetter(bool setActive)
        {
            _stealFoodAction.alpha = setActive ? 1 : 0;
        }
        public void DeathScreenActiveSetter(bool setActive)
        {
            _deathScreen.alpha = setActive ? 1 : 0;
            _deathScreen.interactable = setActive;
            _deathScreen.blocksRaycasts = setActive;
        }


        /// <summary>
        /// Switch to opposite of current state
        /// </summary>
        public void StartMenuActiveSwitch()
        {
            int activeState = _stealFoodAction.alpha.Equals(1) ? 0 : 1;
            _pauseMenu.alpha = activeState;
        }
        public void HUDActiveSwitch()
        {
            int activeState = _stealFoodAction.alpha.Equals(1) ? 0 : 1;
            _HUD.alpha = activeState;
        }
        public void StealFoodActionActiveSwitch()
        {
            int activeState = _stealFoodAction.alpha.Equals(1) ? 0 : 1;
            _stealFoodAction.alpha = activeState;
        }
        public void OptionsActiveSwitch()
        {
            int activeState = _stealFoodAction.alpha.Equals(1) ? 0 : 1;
            _stealFoodAction.alpha = activeState;
        }
    }
}