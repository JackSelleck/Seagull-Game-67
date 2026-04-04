using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public static FPSCounter fpsCounter { get; private set; }

    private float deltaTime = 0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int fps = Mathf.CeilToInt(1f / deltaTime);

        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, 200, 50), "FPS: " + fps, style);
    }
}
