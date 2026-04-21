using UnityEngine;

namespace Scripts.Settings
{
    [DisallowMultipleComponent]
    public class VisualQuality : MonoBehaviour
    {
        [SerializeField] private Material highSpecSkybox;
        [SerializeField] private Camera _camera;
        [SerializeField] private bool lowerRenderDistance = false;
    
        void Start()
        {
            if (lowerRenderDistance)
            {
                RenderSettings.skybox = null;
                RenderSettings.fog = true;
                _camera.farClipPlane = 25;
            }
            else
            {
                RenderSettings.skybox = highSpecSkybox;
                RenderSettings.fog = false;
                _camera.farClipPlane = 100;
            }
        }
    }
}