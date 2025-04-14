using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public EntityData EntityData;
    public Animator Animator;

    [SerializeField] Transform _orientation;
    [SerializeField] PlayerCamera _camera;
    [SerializeField] Transform _groundedRayStart;
    [SerializeField] Transform _groundedRayEnd;
    public bool IsGrounded => _isGrounded;
    bool _isGrounded;

    Rigidbody rb;

    InputSystemActions inputSystemActions;
    InputAction moveInput;
    InputAction attackInput;
    InputAction blockInput;
    InputAction jumpInput;
    InputAction flourishInput;
    Vector2 moveInputValue;
    bool attacking;
    bool blocking;
    bool jumping;
    bool flourishing;

    private void OnEnable()
    {
        moveInput.Enable();
        attackInput.Enable();
        blockInput.Enable();
        jumpInput.Enable();
        flourishInput.Enable();
    }
    private void OnDisable()
    {
        moveInput.Disable();
        attackInput.Disable();
        blockInput.Disable();
        jumpInput.Disable();
        flourishInput.Disable();
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        inputSystemActions = new InputSystemActions();
        moveInput = inputSystemActions.Player.Move;
        moveInput.performed += PerformedMoveInput;
        moveInput.canceled += CanceledMoveInput;
        attackInput = inputSystemActions.Player.Attack;
        attackInput.performed += PerformedAttackInput;
        attackInput.canceled += CanceledAttackInput;
        blockInput = inputSystemActions.Player.Block;
        blockInput.performed += PerformedBlockInput;
        blockInput.canceled += CanceledBlockInput;
        jumpInput = inputSystemActions.Player.Jump;
        jumpInput.performed += PerformedJumpInput;
        jumpInput.canceled += CanceledJumpInput;
        flourishInput = inputSystemActions.Player.Flourish;
        jumpInput.performed += PerformedJumpInput;
        jumpInput.canceled += CanceledJumpInput;
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
    void PerformedAttackInput(InputAction.CallbackContext ctx)
    {
        
    }
    void CanceledAttackInput(InputAction.CallbackContext ctx)
    {

    }
    void PerformedBlockInput(InputAction.CallbackContext ctx)
    {

    }
    void CanceledBlockInput(InputAction.CallbackContext ctx)
    {

    }
    void PerformedJumpInput(InputAction.CallbackContext ctx)
    {

    }
    void CanceledJumpInput(InputAction.CallbackContext ctx)
    {

    }
    void PerformedFlourishInput(InputAction.CallbackContext ctx)
    {

    }
    void CanceledFlourishInput(InputAction.CallbackContext ctx)
    {

    }
    void TryAttack()
    {

    }
    void HandleMovement()
    {
        Vector3 moveDirection = _orientation.forward * moveInputValue.y + _orientation.right * moveInputValue.x;

        rb.AddForce(moveDirection.normalized * EntityData.BaseSpeed, ForceMode.Force);
    }
}