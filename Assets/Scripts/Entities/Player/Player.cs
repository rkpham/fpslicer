using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum State
{
    Idle,
    Walking,
    Dashing,
    Airborne
}

public class Player : MonoBehaviour
{
    public EntityData EntityData;

    [SerializeField] Transform _orientation;
    [SerializeField] PlayerCamera _camera;
    [SerializeField] Transform _groundedRayStart;
    [SerializeField] Transform _groundedRayEnd;
    public bool IsGrounded => _isGrounded;
    bool _isGrounded;

    Rigidbody rb;

    InputSystemActions inputSystemActions;
    InputAction moveInput;
    Vector2 moveInputValue;

    private void OnEnable()
    {
        moveInput.Enable();
    }
    private void OnDisable()
    {
        moveInput.Disable();
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        inputSystemActions = new InputSystemActions();
        moveInput = inputSystemActions.Player.Move;
        moveInput.performed += PerformedMoveInput;
        moveInput.canceled += CanceledMoveInput;
    }
    void Update()
    {
    }
    void FixedUpdate()
    {
        GetGrounded();
        HandleMovement();
    }
    void GetGrounded()
    {
        RaycastHit hit;
        Physics.Raycast(_groundedRayStart.position, _groundedRayEnd.position, out hit);
        _isGrounded = hit.collider != null;
    }
    void PerformedMoveInput(InputAction.CallbackContext ctx)
    {
        moveInputValue = ctx.ReadValue<Vector2>();
    }
    void CanceledMoveInput(InputAction.CallbackContext ctx)
    {
        moveInputValue = Vector2.zero;
    }
    void HandleMovement()
    {
        Vector3 moveDirection = _orientation.forward * moveInputValue.y + _orientation.right * moveInputValue.x;

        rb.AddForce(moveDirection.normalized * EntityData.BaseSpeed, ForceMode.Force);
    }
}