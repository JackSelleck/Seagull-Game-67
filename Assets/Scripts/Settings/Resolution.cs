using UnityEngine;

namespace Scripts.Settings
{
    [DisallowMultipleComponent]
    public class Resolution : MonoBehaviour
    {
        void Start()
        {

        }

        void ApplyResolution()
        {
            Screen.SetResolution(1280, 720, Screen.fullScreen);
        }
    }
}