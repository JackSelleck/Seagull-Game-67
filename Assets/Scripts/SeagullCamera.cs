using UnityEngine;

public class SeagullCamera : MonoBehaviour
{
    [SerializeField] private Transform seagull;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, 0f);
    [SerializeField] private float rotationSmoothTime = 0.15f;
    [SerializeField] private float positionSmoothTime = 0.05f;

    private float yawVelocity;
    private float currentYaw;
    private Vector3 posVelocity;

    private void LateUpdate()
    {
        if (seagull == null)
            return;

        float targetYaw = seagull.eulerAngles.y;
        currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);

        Vector3 targetPos = seagull.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref posVelocity, positionSmoothTime);
    }
}