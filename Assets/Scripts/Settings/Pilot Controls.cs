using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class PilotControls : MonoBehaviour
{
    public static new bool enabled = true;

    [SerializeField] private TextMeshProUGUI text;

    public void SwitchEnabled()
    {
        enabled = !enabled;
        text.text = "Pilot Control Scheme: " + enabled;
    }
}
