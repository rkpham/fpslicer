using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    public Transform cameraAnchorTransform;

    void Update()
    {
        transform.position = cameraAnchorTransform.position;
    }
}
