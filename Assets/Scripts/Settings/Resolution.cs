using UnityEngine;

public class Resolution : MonoBehaviour
{
    void Start()
    {
        ApplyResolution();
    }

    void ApplyResolution()
    {
        Screen.SetResolution(1280, 720, Screen.fullScreen);
    }
}
