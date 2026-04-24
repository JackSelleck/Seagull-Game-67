using UnityEngine;

namespace Scripts.UI 
{
    public class UIManager : MonoBehaviour
    {
        public GameObject pauseMenu;
        public GameObject HUD;
        public GameObject stealFoodAction;

        /// <summary>
        /// Set UI elements active or inactive
        /// </summary>
        public void PauseMenuActiveSetter(bool setActive)
        {
            pauseMenu.SetActive(setActive);
        }
        public void HUDActiveSwitch(bool setActive)
        {
            HUD.SetActive(setActive);
        }
        public void StealFoodActionActiveSwitch(bool setActive)
        {
            stealFoodAction.SetActive(setActive);
        }

        /// <summary>
        /// Switch active bool to opposite
        /// </summary>
        public void PauseMenuActiveSwitch()
        {
            bool activeState =! pauseMenu.activeSelf;
            pauseMenu.SetActive(activeState);
        }
        public void HUDActiveSwitch()
        {
            bool activeState = !HUD.activeSelf;
            HUD.SetActive(activeState);
        }
        public void StealFoodActionActiveSwitch()
        {
            bool activeState = !stealFoodAction.activeSelf;
            stealFoodAction.SetActive(activeState);
        }
    }
}