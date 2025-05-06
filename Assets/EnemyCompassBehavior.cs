using UnityEngine;

public class EnemyCompassBehaviour : MonoBehaviour
{
    private Transform cameraTransform;
    private Transform targetTransform;
    public string TagToLookFor;
    public float offsetAngle;

    void Start()
    {
        // Find Main Camera
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("CompassPointer: No MainCamera found in scene!");
            enabled = false;
            return;
        }

        // Find the "Test" tagged object
        GameObject targetObject = GameObject.FindGameObjectWithTag(TagToLookFor);
        if (targetObject != null)
        {
            targetTransform = targetObject.transform;
        }
        else
        {
            Debug.LogError("CompassPointer: No object with tag " + TagToLookFor +  " found in scene!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (cameraTransform == null || targetTransform == null) return;

        // Get flat positions (ignore height)
        Vector3 cameraPos = cameraTransform.position;
        Vector3 targetPos = targetTransform.position;
        cameraPos.y = 0f;
        targetPos.y = 0f;

        // Direction from camera to target
        Vector3 directionToTarget = (targetPos - cameraPos).normalized;

        // Camera's forward direction (flat)
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        // Calculate signed angle between camera forward and target direction
        float signedAngle = Vector3.SignedAngle(cameraForward, directionToTarget, Vector3.up);

        // Set only local Y rotation, preserving current X and Z
        Vector3 currentLocalEuler = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentLocalEuler.x, signedAngle+offsetAngle, currentLocalEuler.z);
    }
}
