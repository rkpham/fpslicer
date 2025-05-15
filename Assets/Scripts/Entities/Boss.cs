using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;

public class Boss : Entity
{
    public Player Player;
    public Renderer ModelRenderer;

    [SerializeField] Animator anim;
    CurrentAction CurrentAction = CurrentAction.Wait;
    Vector2 strafeDirection = Vector2.zero;
    float tickTimer = 1.0f;
    float aggressiveness = 0.0f;
    public ezsoundificatinator _ezsoundificatinator;

    private void Start()
    {
        _ezsoundificatinator = GetComponentInChildren<ezsoundificatinator>();
    }

    private void FixedUpdate()
    {
        Transform playerTransform = Player.gameObject.transform;
        Vector2 horizPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos = new Vector2(playerTransform.position.x, playerTransform.position.z);
        Vector2 targetTo = targetPos - horizPos;
        float angle = Mathf.Atan2(targetTo.x, targetTo.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        ModelRenderer.material.color = ModelRenderer.material.color + ((Color.white - ModelRenderer.material.color) * Time.deltaTime * 10f);

        if (aggressiveness < 10f)
        {
            aggressiveness += (playerTransform.position - transform.position).magnitude * 0.01f;
        }

        tickTimer -= Time.deltaTime;
        if (tickTimer < 0)
        {
            tickTimer = 1f;

            ProcessActions();
            StartStrafe();
        }
        GetGrounded();
        HandleMovement(strafeDirection);
    }
    public override void ApplyDamage(DamageInstance damageInstance)
    {
        if (CurrentAction == CurrentAction.Wait && Stamina >= 0.6f)
        {
            DoParry();
        }
        base.ApplyDamage(damageInstance);
        ModelRenderer.material.color = Color.red;
    }
    protected override void HandleMovement(Vector2 direction)
    {
        base.HandleMovement(direction);
        Vector3 horizVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).normalized;
        float speedX = Vector3.Dot(transform.right, horizVel);
        float speedY = Vector3.Dot(transform.forward, horizVel);
        anim.SetFloat("SpeedX", speedX);
        anim.SetFloat("SpeedY", speedY);
        anim.SetFloat("Speed", horizVel.magnitude * 3f);
    }
    void ProcessActions()
    {
        if (Stamina < 1.0f)
        {
            Stamina += 0.1f;
        }

        bool doSomething = Random.Range(0f, 1f) > 0.5f;

        if (doSomething)
        {
            if (CurrentAction == CurrentAction.Wait)
            {
                {
                    DoChargeAttack();
                }
                if (Stamina >= 0.3f)
                {
                    DoAttack();
                }
                else if (Stamina >= 0.1f)
                {
                    DoBlock();
                }
            }
        }
        
    }
    void StartStrafe()
    {
        int strafeChoose = Random.Range(0, 4);

        switch (strafeChoose)
        {
            case 0:
                strafeDirection = new Vector2(1f, 0f);
                break;
            case 1:
                strafeDirection = new Vector2(0f, 1f);
                break;
            case 2:
                strafeDirection = new Vector2(-1f, 0f);
                break;
            case 3:
                strafeDirection = new Vector2(0f, 0f);
                break;
            default:
                strafeDirection = new Vector2(0f, 0f);
                break;
        }
    }
    void DoChargeAttackHit()
    {
        DamageInstance damageInstance = new DamageInstance();
        damageInstance.Stamina = 0.3f;
        damageInstance.Pushback = 400f;
        damageInstance.Direction = transform.forward;
        damageInstance.Amount = 30f;
        DoMeleeHit(damageInstance);
    }
    void DoChargeAttack()
    {
        anim.SetTrigger("ChargeStab");
        CurrentAction = CurrentAction.ChargeAttack;
        Stamina -= 0.4f;
    }
    void DoAttackHit()
    {
        DamageInstance damageInstance = new DamageInstance();
        damageInstance.Stamina = 0.1f;
        damageInstance.Pushback = 240f;
        damageInstance.Direction = transform.forward;
        damageInstance.Amount = 10f;
        DoMeleeHit(damageInstance);
    }
    void DoAttack()
    {
        anim.SetTrigger("Stab");
        CurrentAction = CurrentAction.Attack;
        Stamina -= 0.3f;
    }
    void DoParry()
    {
        Stamina -= 0.2f;
        Blocking = true;
    }
    void DoBlock()
    {
        Stamina -= 0.2f;
        Blocking = true;
        Blocking = false;
    }
    void DoMeleeHit(DamageInstance damageInstance)
    {
        foreach (Collider collider in currentAttackColliders)
        {
            Entity entityComponent = collider.gameObject.GetComponent<Entity>();
            if (entityComponent != null)
            {
                entityComponent.ApplyDamage(damageInstance);
            }
        }
        _ezsoundificatinator.playDaBih();
    }
    void OnAnimEnded()
    {
        CurrentAction = CurrentAction.Wait;
        Blocking = false;
    }
}

public enum CurrentAction
{
    Wait,
    Attack,
    ChargeAttack,
    Block
}