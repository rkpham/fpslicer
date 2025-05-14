using UnityEngine;

public class MouseCameraLean : MonoBehaviour
{
    [Header("Lean Settings")]
    public float maxYaw = 10f;         // Left-right tilt in degrees (around local Y)
    public float maxPitch = 5f;        // Up-down tilt in degrees (around local X)
    public float sensitivity = 1f;
    public float smoothSpeed = 5f;

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        float normalizedX = (mousePos.x - screenCenter.x) / (Screen.width / 2f);
        float normalizedY = (mousePos.y - screenCenter.y) / (Screen.height / 2f);

        normalizedX = Mathf.Clamp(normalizedX, -1f, 1f);
        normalizedY = Mathf.Clamp(normalizedY, -1f, 1f);

        // Calculate target lean angles
        float yaw = normalizedX * maxYaw * sensitivity;       // local Y rotation
        float pitch = -normalizedY * maxPitch * sensitivity;  // local X rotation

        // Build a local rotation offset based on the camera's UP-facing orientation
        Quaternion targetLean = Quaternion.Euler(pitch, yaw, 0f);
        Quaternion targetRotation = initialRotation * targetLean;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}
