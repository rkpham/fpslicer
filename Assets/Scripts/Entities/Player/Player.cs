using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Player;

public class Player : Entity
{
    public ezsoundificatinator _ezsoundificatinator;

    private void Start()
    {
        _ezsoundificatinator = GetComponentInChildren<ezsoundificatinator>();
    }


    public delegate void OnActionPerformed(ActionData actionData);
    public event OnActionPerformed onActionPerformed;

    public Animator ViewmodelAnim;

    public ActionData BaseAttackData;
    public ActionData BaseBlockData;
    public ActionData BaseJumpData;
    public ActionData BaseFlourishData;

    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] Transform groundedRayStart;
    [SerializeField] Transform groundedRayEnd;
    [SerializeField] PostProcessManager postProcessManager;
    public bool CurrentlyBlocking = false;

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
        flourishInput.performed += PerformedFlourishInput;
        flourishInput.canceled += CanceledFlourishInput;
    }
    void Update()
    {
    }
    void FixedUpdate()
    {
        GetGrounded();
        HandleMovement(moveInputValue);
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
        if (jumpRecoveryTimeLeft > 0 || !IsGrounded)
            return;
        
        jumping = true;
        lastJumpTime = Time.time;
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
                if (currentActionData.WindupLength == -1)
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
                            if (ViewmodelAnim.runtimeAnimatorController)
                                ViewmodelAnim.SetBool("SkipActive", true);
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
                        if (ViewmodelAnim.runtimeAnimatorController)
                            ViewmodelAnim.SetBool("SkipActive", true);
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
                            if (ViewmodelAnim.runtimeAnimatorController)
                                ViewmodelAnim.SetTrigger("RestartCharge");
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
    void ResetAnimVariables()
    {
        ViewmodelAnim.SetBool("StartWindup", false);
        ViewmodelAnim.SetBool("StartCharge", false);
        ViewmodelAnim.SetBool("StartActive", false);
        ViewmodelAnim.SetBool("StartRecovery", false);
        ViewmodelAnim.SetBool("RestartCharge", false);
        ViewmodelAnim.SetBool("SkipWindup", false);
        ViewmodelAnim.SetBool("SkipCharge", false);
        ViewmodelAnim.SetBool("SkipActive", false);
        ViewmodelAnim.SetBool("SkipWindupAndCharge", false);
    }
    void StartActionWindup(ActionData actionData, ActionType actionType)
    {
        onActionPerformed?.Invoke(actionData);
        windupTimeElapsed = 0f;
        chargeTimeElapsed = 0f;
        activeTimeElapsed = 0f;
        currentActionData = actionData;
        currentActionType = actionType;
        ViewmodelAnim.runtimeAnimatorController = actionData.AnimatorOverrideController;
        if (ViewmodelAnim.runtimeAnimatorController != null)
            ResetAnimVariables();
        currentActionData.OnActionWindupStarted(this);
        if (ViewmodelAnim.runtimeAnimatorController)
            ViewmodelAnim.SetBool("CancelRecovery", true);

        if (currentActionData.forceApplyStage == ActionStage.Windup)
        {
            ApplyLocalForce(currentActionData.moveForce, currentActionData.inputModulate);
        }

        if (currentActionData.WindupLength == 0)
        {
            if (currentActionData.MaxChargeLength == 0 || !currentActionData.CanCharge)
            {
                StartActionActive();
                if (ViewmodelAnim.runtimeAnimatorController)
                {
                    ViewmodelAnim.SetBool("SkipCharge", true);
                    ViewmodelAnim.SetBool("SkipWindupAndCharge", true);
                }
            }
            else
            {
                StartActionCharge();
                if (ViewmodelAnim.runtimeAnimatorController)
                    ViewmodelAnim.SetBool("SkipWindup", true);
            }
        }
        else
        {
            currentActionStage = ActionStage.Windup;
            if (ViewmodelAnim.runtimeAnimatorController)
                ViewmodelAnim.SetBool("StartWindup", true);
        }
    }
    public void StartActionCharge()
    {
        chargeTimeElapsed = 0f;
        currentActionData.OnActionChargeStarted(this);

        if (currentActionData.forceApplyStage == ActionStage.Charge)
        {
            ApplyLocalForce(currentActionData.moveForce, currentActionData.inputModulate);
        }

        if (currentActionData.MaxChargeLength == 0 || !currentActionData.CanCharge)
        {
            StartActionActive();
            if (ViewmodelAnim.runtimeAnimatorController)
                ViewmodelAnim.SetBool("SkipCharge", true);
        }
        else
        {
            currentActionStage = ActionStage.Charge;
            if (ViewmodelAnim.runtimeAnimatorController)
                ViewmodelAnim.SetBool("StartCharge", true);
        }
    }
    public void StartActionActive()
    {
        activeTimeElapsed = 0f;
        currentActionData.OnActionActiveStarted(this);

        if(currentActionType == ActionType.Attack)
        {
            _ezsoundificatinator.playDaBih();
        }

        if (currentActionData.forceApplyStage == ActionStage.Active)
        {
            ApplyLocalForce(currentActionData.moveForce, currentActionData.inputModulate);
        }
        if (currentActionData.IsAttack)
        {
            DoMeleeHit();
        }
        if (currentActionData.ActiveLength == 0)
        {
            FinishAction();
            if (ViewmodelAnim.runtimeAnimatorController)
                ViewmodelAnim.SetBool("SkipActive", true);
        }
        else
        {
            currentActionStage = ActionStage.Active;
            if (ViewmodelAnim.runtimeAnimatorController)
                ViewmodelAnim.SetBool("StartActive", true);
        }
    }
    void FinishAction()
    {
        currentActionData.OnActionFinished(this);
        if (ViewmodelAnim.runtimeAnimatorController)
            ViewmodelAnim.SetBool("CancelRecovery", false);
        if (currentActionData.forceApplyStage == ActionStage.Recovery)
        {
            ApplyLocalForce(currentActionData.moveForce, currentActionData.inputModulate);
        }
        if (currentActionData.AttackChain != null && Time.time - lastAttackTime <= currentActionData.AttackComboTimeFromEnd)
        {
            if (ViewmodelAnim.runtimeAnimatorController)
                ViewmodelAnim.SetTrigger("StartChain");
            StartActionWindup(currentActionData.AttackChain, ActionType.Attack);
        }
        else if (currentActionData.BlockChain != null && Time.time - lastBlockTime <= currentActionData.BlockComboTimeFromEnd)
        {
            if (ViewmodelAnim.runtimeAnimatorController)
                ViewmodelAnim.SetTrigger("StartChain");
            StartActionWindup(currentActionData.BlockChain, ActionType.Block);
        }
        else if (currentActionData.JumpChain != null && Time.time - lastJumpTime <= currentActionData.JumpComboTimeFromEnd)
        {
            if (ViewmodelAnim.runtimeAnimatorController)
                ViewmodelAnim.SetTrigger("StartChain");
            StartActionWindup(currentActionData.JumpChain, ActionType.Jump);
        }
        else if (currentActionData.FlourishChain != null && Time.time - lastFlourishTime <= currentActionData.FlourishComboTimeFromEnd)
        {
            if (ViewmodelAnim.runtimeAnimatorController)
                ViewmodelAnim.SetTrigger("StartChain");
            StartActionWindup(currentActionData.FlourishChain, ActionType.Flourish);
        }
        else
        {
            if (ViewmodelAnim.runtimeAnimatorController)
            {
                ResetAnimVariables();
                ViewmodelAnim.SetBool("SkipActive", true);
                ViewmodelAnim.SetBool("StartRecovery", true);
            }
            attackRecoveryTimeLeft = currentActionData.AttackRecoveryTime;
            blockRecoveryTimeLeft = currentActionData.BlockRecoveryTime;
            jumpRecoveryTimeLeft = currentActionData.JumpRecoveryTime;
            flourishRecoveryTimeLeft = currentActionData.FlourishRecoveryTime;
            currentActionData = null;
        }
    }
    public void DoMeleeHit()
    {
        foreach (Collider collider in currentAttackColliders)
        {
            Entity entityComponent = collider.gameObject.GetComponent<Entity>();
            if (entityComponent != null)
            {
                DamageInstance damageInstance = new DamageInstance();
                damageInstance.Amount = currentActionData.Damage;
                damageInstance.Stamina = currentActionData.StaminaDamage;
                damageInstance.Pushback = currentActionData.Pushback;
                damageInstance.Direction = transform.forward;
                damageInstance.Blockable = !currentActionData.Unblockable;

                entityComponent.ApplyDamage(damageInstance);
            }
        }
    }
    public override void ApplyDamage(DamageInstance damageInstance)
    {
        base.ApplyDamage(damageInstance);
        if (blocking)
        {
            StartActionActive();
        }
        else
        {
            postProcessManager.DamageEffect();
        }
    }
    void ApplyLocalForce(Vector3 force, bool inputModulate)
    {
        Vector3 localForce;
        if (inputModulate)
        {
            localForce = moveInputValue.x * transform.right * force.x + force.y * transform.up + moveInputValue.y * transform.forward * force.z;
        }
        else
        {
            localForce = force.x * transform.right + force.y * transform.up + force.z * transform.forward;
        }
        rb.linearVelocity += localForce;
    }
}

enum ActionType
{
    Attack,
    Block,
    Jump,
    Flourish
}

public enum ActionStage
{
    Windup,
    Charge,
    Active,
    Recovery
}