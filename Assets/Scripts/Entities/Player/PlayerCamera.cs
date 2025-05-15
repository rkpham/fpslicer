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
    Vector2 lookValue;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inputSystemActions = new InputSystemActions();
        inputSystemActions.Player.Look.Enable();
        inputSystemActions.Player.Look.performed += Look;
        inputSystemActions.Player.Look.canceled += StopLook;
    }

    // Update is called once per frame
    void Update()
    {
        yRotation += lookValue.x * sensX * Time.deltaTime;

        xRotation -= lookValue.y * sensY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    void Look(InputAction.CallbackContext ctx)
    {
       lookValue = ctx.ReadValue<Vector2>();
    }
    void StopLook(InputAction.CallbackContext ctx)
    {
        lookValue = Vector2.zero;
    }
}
