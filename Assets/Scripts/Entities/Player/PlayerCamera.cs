using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform orientation;

    public float sensX = 1f;
    public float sensY = 1f;

    private float yRotation;
    private float xRotation;

    InputSystemActions inputSystemActions;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputSystemActions = new InputSystemActions();
        inputSystemActions.Player.Look.Enable();
        inputSystemActions.Player.Look.performed += Look;
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Look(InputAction.CallbackContext ctx)
    {
        Vector2 lookValue = ctx.ReadValue<Vector2>();
        yRotation += lookValue.x * sensX;

        xRotation -= lookValue.y * sensY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
