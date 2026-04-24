using Scripts.Inputs;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class PlayerReferenceManager : MonoBehaviour
    {
        [HideInInspector] public PlayerInputManager Inputs;
        [HideInInspector] public UnityEngine.Camera Cam;
        [HideInInspector] public UIManager UIManager;

        private void Awake()
        {
            Inputs = GetComponent<PlayerInputManager>();
            Cam = UnityEngine.Camera.main;
            UIManager = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<UIManager>();
        }
    }
}