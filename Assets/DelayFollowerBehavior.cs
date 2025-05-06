using UnityEngine;

public class DelayFollowerBehavior : MonoBehaviour
{
    public Transform target;

    [Header("Position Settings")]
    public float positionFollowSpeed = 5f;   // Linearity: how fast to catch up (higher = faster)
    public float maxPositionLag = 2f;         // Maximum distance allowed before forcibly snapping closer

    [Header("Rotation Settings")]
    public float rotationFollowSpeed = 5f;    // Linearity for rotation
    public float maxRotationLag = 30f;         // Maximum angle (degrees) allowed before snapping faster

    void Update()
    {
        if (target == null) return;

        float deltaTime = Time.fixedDeltaTime;

        // --- POSITION ---
        Vector3 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;

        if (distance > 0.001f)
        {
            // Calculate dynamic follow factor
            float posT = Mathf.Clamp01(positionFollowSpeed * deltaTime);

            // If too far, adjust interpolation to catch up faster
            if (distance > maxPositionLag)
            {
                posT = Mathf.Max(posT, 1f - (maxPositionLag / distance));
            }

            transform.position = Vector3.Lerp(transform.position, target.position, posT);
        }

        // --- ROTATION ---
        float angle = Quaternion.Angle(transform.rotation, target.rotation);

        if (angle > 0.01f)
        {
            float rotT = Mathf.Clamp01(rotationFollowSpeed * deltaTime);

            if (angle > maxRotationLag)
            {
                rotT = Mathf.Max(rotT, 1f - (maxRotationLag / angle));
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotT);
        }
    }
}
