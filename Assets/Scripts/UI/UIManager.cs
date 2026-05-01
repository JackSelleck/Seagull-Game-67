using UnityEngine;

namespace Scripts.UI 
{
    [DisallowMultipleComponent]
    public class UIManager : MonoBehaviour
    {
        public CanvasGroup pauseMenu;
        public CanvasGroup HUD;
        public CanvasGroup stealFoodAction;

        /// <summary>
        /// Set UI elements active or inactive
        /// </summary>
        public void PauseMenuActiveSetter(bool setActive)
        {
            pauseMenu.alpha = setActive? 1 : 0;
        }
        public void HUDActiveSwitch(bool setActive)
        {
            HUD.alpha = setActive ? 1 : 0;
        }
        public void StealFoodActionActiveSwitch(bool setActive)
        {
            stealFoodAction.alpha = setActive ? 1 : 0;
        }

        /// <summary>
        /// Switch to opposite of current state
        /// </summary>
        public void PauseMenuActiveSwitch()
        {
            int activeState = stealFoodAction.alpha.Equals(1) ? 0 : 1;
            pauseMenu.alpha = activeState;
        }
        public void HUDActiveSwitch()
        {
            int activeState = stealFoodAction.alpha.Equals(1) ? 0 : 1;
            HUD.alpha = activeState;
        }
        public void StealFoodActionActiveSwitch()
        {
            int activeState = stealFoodAction.alpha.Equals(1) ? 0 : 1;
            stealFoodAction.alpha = activeState;
        }
    }
}