using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public Animator ViewmodelAnim;

    public float MaxSpeed;
    public float MoveForce;
    public float AirMoveForce;
    public float JumpForce;
    public float JumpMoveForce;

    public ActionData BaseAttackData;
    public ActionData BaseBlockData;
    public ActionData BaseJumpData;
    public ActionData BaseFlourishData;

    [SerializeField] Transform orientation;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] Transform groundedRayStart;
    [SerializeField] Transform groundedRayEnd;
    public bool IsGrounded => isGrounded;
    bool isGrounded;
    public bool CurrentlyBlocking = false;
    private List<Collider> currentAttackColliders;

    Rigidbody rb;

    InputSystemActions inputSystemActions;
    InputAction moveInput;
    InputAction attackInput;
    InputAction blockInput;
    InputAction jumpInput;
    InputAction flourishInput;
    public Vector2 MoveInputValue => moveInputValue;
    Vector2 moveInputValue;
    bool attacking;
    bool blocking;
    bool jumping;
    bool flourishing;
    float lastAttackTime;
    float lastBlockTime;
    float lastJumpTime;
    float lastFlourishTime;

    ActionData currentActionData;
    ActionStage currentActionStage;
    ActionType currentActionType;
    float windupTimeElapsed;
    float chargeTimeElapsed;
    float activeTimeElapsed;
    float attackRecoveryTimeLeft;
    float blockRecoveryTimeLeft;
    float jumpRecoveryTimeLeft;
    float flourishRecoveryTimeLeft;

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

        // Connect all InputActions
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
        if (attackRecoveryTimeLeft > 0)
            return;

        attacking = true;
        lastAttackTime = Time.time;
        if (currentActionData == null)
        {
            StartActionWindup(BaseAttackData, ActionType.Attack);
        }
    }
    void CanceledAttackInput(InputAction.CallbackContext ctx)
    {
        attacking = false;
    }
    void PerformedBlockInput(InputAction.CallbackContext ctx)
    {
        if (blockRecoveryTimeLeft > 0)
            return;

        blocking = true;
        lastBlockTime = Time.time;
        if (currentActionData == null)
        {
            StartActionWindup(BaseBlockData, ActionType.Block);
        }
    }
    void CanceledBlockInput(InputAction.CallbackContext ctx)
    {
        blocking = false;
    }
    void PerformedJumpInput(InputAction.CallbackContext ctx)
    {
        if (jumpRecoveryTimeLeft > 0)
            return;

        jumping = true;
        lastBlockTime = Time.time;
        if (currentActionData == null)
        {
            StartActionWindup(BaseJumpData, ActionType.Jump);
        }
    }
    void CanceledJumpInput(InputAction.CallbackContext ctx)
    {
        jumping = false;
    }
    void PerformedFlourishInput(InputAction.CallbackContext ctx)
    {
        if (flourishRecoveryTimeLeft > 0)
            return;

        flourishing = true;
        lastFlourishTime = Time.time;
        if (currentActionData == null)
        {
            StartActionWindup(BaseFlourishData, ActionType.Flourish);
        }
    }
    void CanceledFlourishInput(InputAction.CallbackContext ctx)
    {
        flourishing = false;
    }
    void GetGrounded()
    {
        RaycastHit hit;
        Physics.Raycast(groundedRayStart.position, groundedRayEnd.TransformDirection(groundedRayEnd.position), out hit, 0.3f);
        isGrounded = hit.collider != null;
    }
    void HandleMovement()
    {
        Vector3 moveDirection = orientation.forward * moveInputValue.y + orientation.right * moveInputValue.x;

        if (isGrounded)
        {
            rb.linearDamping = 3f;
            rb.AddForce(moveDirection.normalized * EntityData.BaseSpeed * MoveForce, ForceMode.Force);
        }
        else
        {
            rb.linearDamping = 0f;
            rb.AddForce(moveDirection.normalized * EntityData.BaseSpeed * AirMoveForce, ForceMode.Force);
        }
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
        bool stillCharging = false;

        if (attackRecoveryTimeLeft > 0)
            attackRecoveryTimeLeft -= Time.fixedDeltaTime;
        if (blockRecoveryTimeLeft > 0)
            blockRecoveryTimeLeft -= Time.fixedDeltaTime;
        if (jumpRecoveryTimeLeft > 0)
            jumpRecoveryTimeLeft -= Time.fixedDeltaTime;
        if (flourishRecoveryTimeLeft > 0)
            flourishRecoveryTimeLeft -= Time.fixedDeltaTime;

        if (currentActionData == null)
            return;

        switch (currentActionStage)
        {
            case ActionStage.Windup:
                windupTimeElapsed += Time.fixedDeltaTime;
                if (currentActionData.ActiveLength == -1)
                {
                    if (windupTimeElapsed >= ViewmodelAnim.GetCurrentAnimatorStateInfo(0).length)
                    {
                        StartActionCharge();
                    }
                }
                else if (windupTimeElapsed >= currentActionData.WindupLength)
                {
                    StartActionCharge();
                }
                break;
            case ActionStage.Charge:
                chargeTimeElapsed += Time.fixedDeltaTime;

                switch (currentActionType)
                {
                    case ActionType.Attack:
                        stillCharging = attacking;
                        break;
                    case ActionType.Block:
                        stillCharging = blocking;
                        break;
                    case ActionType.Jump:
                        stillCharging = jumping;
                        break;
                    case ActionType.Flourish:
                        stillCharging = flourishing;
                        break;
                }
                if (stillCharging)
                {
                    if (chargeTimeElapsed >= currentActionData.MaxChargeLength && currentActionData.MaxChargeLength > 0)
                    {
                        if (currentActionData.ManualActive)
                        {
                            FinishAction();
                        }
                        else
                        {
                            StartActionActive();
                        }
                    }
                }
                else
                {
                    if (currentActionData.ManualActive)
                    {
                        FinishAction();
                    }
                    else if (currentActionData.LowChargeDefault && chargeTimeElapsed < currentActionData.ChargeThreshold)
                    {
                        StartActionWindup(currentActionData.LowChargeDefault, currentActionType);
                    }
                    else
                    {
                        StartActionActive();
                    }
                }
                break;
            case ActionStage.Active:
                activeTimeElapsed += Time.fixedDeltaTime;
                if (currentActionData.ActiveLength == -1)
                {
                    if (activeTimeElapsed >= ViewmodelAnim.GetCurrentAnimatorStateInfo(0).length)
                    {
                        if (currentActionData.RepeatCharge)
                        {
                            StartActionCharge();
                        }
                        else
                        {
                            FinishAction();
                        }
                    }
                }
                else if (activeTimeElapsed >= currentActionData.ActiveLength)
                {
                    if (currentActionData.RepeatCharge)
                    {
                        StartActionCharge();
                    }
                    else
                    {
                        FinishAction();
                    }
                }
                break;
        }
    }
    void StartActionWindup(ActionData actionData, ActionType actionType)
    {
        windupTimeElapsed = 0f;
        chargeTimeElapsed = 0f;
        activeTimeElapsed = 0f;
        currentActionData = actionData;
        currentActionType = actionType;
        ViewmodelAnim.runtimeAnimatorController = actionData.AnimatorOverrideController;
        currentActionData.OnActionWindupStarted(this);
        if (currentActionData.WindupLength == 0)
        {
            StartActionCharge();
        }
        else
        {
            currentActionStage = ActionStage.Windup;
            if (currentActionData.AnimatorOverrideController)
                ViewmodelAnim.Play("Windup", 0, 0f);
        }
    }
    public void StartActionCharge()
    {
        chargeTimeElapsed = 0f;
        currentActionData.OnActionChargeStarted(this);
        if (currentActionData.MaxChargeLength == 0 || !currentActionData.CanCharge)
        {
            StartActionActive();
        }
        else
        {
            currentActionStage = ActionStage.Charge;
            if (currentActionData.AnimatorOverrideController)
                ViewmodelAnim.Play("ChargeLoop", 0, 0f);
        }
    }
    public void StartActionActive()
    {
        activeTimeElapsed = 0f;
        DoMeleeHit();
        currentActionData.OnActionActiveStarted(this);
        if (currentActionData.ActiveLength == 0)
        {
            FinishAction();
        }
        else
        {
            currentActionStage = ActionStage.Active;
            if (currentActionData.AnimatorOverrideController)
                ViewmodelAnim.Play("Active", 0, 0f);
        }
    }
    void FinishAction()
    {
        currentActionData.OnActionFinished(this);
        if (currentActionData.AttackChain != null && Time.time - lastAttackTime <= currentActionData.AttackComboTimeFromEnd)
        {
            StartActionWindup(currentActionData.AttackChain, ActionType.Attack);
        }
        else if (currentActionData.BlockChain != null && Time.time - lastBlockTime <= currentActionData.BlockComboTimeFromEnd)
        {
            StartActionWindup(currentActionData.BlockChain, ActionType.Block);
        }
        else if (currentActionData.JumpChain != null && Time.time - lastJumpTime <= currentActionData.JumpComboTimeFromEnd)
        {
            StartActionWindup(currentActionData.JumpChain, ActionType.Jump);
        }
        else if (currentActionData.FlourishChain != null && Time.time - lastFlourishTime <= currentActionData.FlourishComboTimeFromEnd)
        {
            StartActionWindup(currentActionData.FlourishChain, ActionType.Flourish);
        }
        else
        {
            if (currentActionData.AnimatorOverrideController)
                ViewmodelAnim.Play("Recovery", 0, 0f);
            attackRecoveryTimeLeft = currentActionData.AttackRecoveryTime;
            blockRecoveryTimeLeft = currentActionData.BlockRecoveryTime;
            jumpRecoveryTimeLeft = currentActionData.JumpRecoveryTime;
            flourishRecoveryTimeLeft = currentActionData.FlourishRecoveryTime;
            currentActionData = null;
        }
    }
    public void OnAttackTriggerEnter(Collider collider)
    {
        currentAttackColliders.Add(collider);
    }
    public void OnAttackTriggerExit(Collider collider)
    {
        currentAttackColliders.Remove(collider);
    }
    public void DoMeleeHit()
    {
        foreach (Collider collider in currentAttackColliders)
        {
            
        }
    }
}

enum ActionType
{
    Attack,
    Block,
    Jump,
    Flourish
}

enum ActionStage
{
    Windup,
    Charge,
    Active,
    Recovery
}