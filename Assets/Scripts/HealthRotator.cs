using UnityEngine;

public class HealthRotator : MonoBehaviour
{
    public Transform cameraTransform; // Assign the main camera here.

    public float baseSpeed = 10f; // 'a' in your formula
    public float rotationMultiplier = 1f; // 'b' in your formula
    public float movementMultiplier = 1f; // 'c' in your formula

    private Vector3 previousCameraPosition;
    private Quaternion previousCameraRotation;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        previousCameraPosition = cameraTransform.position;
        previousCameraRotation = cameraTransform.rotation;
    }

    void FixedUpdate()
    {
        // Calculate camera movement speed (world space movement)
        Vector3 cameraDeltaPosition = cameraTransform.position - previousCameraPosition;
        float cameraMovementSpeed = cameraDeltaPosition.magnitude / Time.fixedDeltaTime;

        // Calculate camera rotation speed (how fast it's rotating, regardless of axis)
        Quaternion deltaRotation = cameraTransform.rotation * Quaternion.Inverse(previousCameraRotation);
        deltaRotation.ToAngleAxis(out float angleInDegrees, out _);
        float cameraRotationSpeed = Mathf.Abs(angleInDegrees) / Time.fixedDeltaTime;

        // Total rotation speed according to formula
        float totalSpeed = baseSpeed
                         + rotationMultiplier * cameraRotationSpeed
                         + movementMultiplier * cameraMovementSpeed;

        // Rotate about local X axis
        transform.Rotate(Vector3.right, totalSpeed * Time.fixedDeltaTime, Space.Self);

        // Update previous frame data
        previousCameraPosition = cameraTransform.position;
        previousCameraRotation = cameraTransform.rotation;
    }
}
