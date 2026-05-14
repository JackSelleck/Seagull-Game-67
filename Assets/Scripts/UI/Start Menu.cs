using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private UIManager _manager;
        private void Awake()
        {
            Time.timeScale = 0f;
        }
        public void StartGame()
        {
            Time.timeScale = 1;
            _manager.StartMenuActiveSetter(false);
            _manager.HUDActiveSetter(true);
        }
        public void Options()
        {
            _manager.OptionsActiveSetter(true);
        }
        public void Reset()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        public void Exit()
        {
            Application.Quit();
        }
    }
}