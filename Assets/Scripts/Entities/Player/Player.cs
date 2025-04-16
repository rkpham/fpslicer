using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public EntityData EntityData;
    public Animator Animator;

    public float MaxSpeed;
    public float MoveForce;
    public float JumpForce;
    public float JumpMoveForce;

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

    bool canAttack = true;
    bool canBlock = true;
    bool canJump = true;
    bool canFlourish = true;

    float attackTimeLeft;

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
        LimitMovement();
        HandleActions();
    }
    void GetGrounded()
    {
        RaycastHit hit;
        Physics.Raycast(_groundedRayStart.position, _groundedRayEnd.position, out hit);
        _isGrounded = hit.collider != null;
    }
    void HandleMovement()
    {
        Vector3 moveDirection = _orientation.forward * moveInputValue.y + _orientation.right * moveInputValue.x;

        rb.AddForce(moveDirection.normalized * EntityData.BaseSpeed * MoveForce, ForceMode.Force);
    }
    void LimitMovement()
    {
        Vector3 horizVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (horizVelocity.magnitude > MaxSpeed)
        {
            Vector3 limitedHorizVelocity = horizVelocity.normalized * MaxSpeed;
            rb.linearVelocity = new Vector3(limitedHorizVelocity.x, rb.linearVelocity.y, limitedHorizVelocity.z);
        }
    }
    void HandleActions()
    {

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
        TryAttack();
    }
    void TryAttack()
    {
        if (canAttack)
        {
            Animator.SetTrigger("Attacking");
        }
    }
    public void DoAttack()
    {
        RaycastHit raycastHit;
        Debug.DrawRay(_camera.transform.position, _camera.transform.TransformDirection(_camera.transform.forward) * 2f, Color.red);
        if (Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(_camera.transform.forward), out raycastHit, 2f))
        {
            Debug.Log(raycastHit.point);
        }
    }
    public void OnAttackFinished()
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
        rb.AddForce(Vector3.up * JumpForce);

        Vector3 moveDirection = _orientation.forward * moveInputValue.y + _orientation.right * moveInputValue.x;

        rb.AddForce(moveDirection.normalized * EntityData.BaseSpeed * JumpMoveForce, ForceMode.Force);
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
    
}