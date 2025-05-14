using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class ThreeDeeClickability : MonoBehaviour
{
    [Header("Hover Movement")]
    public float moveDistance = 0.5f;         // Distance to move toward camera
    public float moveSpeed = 5f;              // Interpolation speed

    [Header("Scene Settings")]
    public string sceneToLoad;                // Scene name to load on click

    [Header("Raycast Settings")]
    public LayerMask raycastLayers = ~0;     // Default: all layers

    private Vector3 originalLocalPosition;
    private Vector3 targetLocalPosition;
    private bool isHovered = false;

    private void Start()
    {
        originalLocalPosition = transform.localPosition;
        targetLocalPosition = originalLocalPosition;
    }

    private void Update()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Raycast from camera to mouse
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        isHovered = false;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayers))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isHovered = true;

                if (Input.GetMouseButtonDown(0) && !string.IsNullOrEmpty(sceneToLoad))
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
        }

        // Calculate direction to camera in local space
        Vector3 worldToCam = (cam.transform.position - transform.position).normalized;
        Vector3 localToCamDir = transform.parent != null
            ? transform.parent.InverseTransformDirection(worldToCam)
            : worldToCam; // fallback if no parent

        // Update target position in local space
        if (isHovered)
        {
            targetLocalPosition = originalLocalPosition + localToCamDir * moveDistance;
        }
        else
        {
            targetLocalPosition = originalLocalPosition;
        }

        // Smooth movement in local space
        if (Vector3.Distance(transform.localPosition, targetLocalPosition) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.localPosition = targetLocalPosition;
        }
    }
}
