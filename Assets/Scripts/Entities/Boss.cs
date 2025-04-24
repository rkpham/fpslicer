using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;

public class Boss : Entity
{
    public Player Player;
    public Transform Orientation;
    public MeshRenderer ModelRenderer;
    public MeshRenderer WeaponRenderer;
    public CurrentAction CurrentAction = CurrentAction.Wait;
    public TextMeshProUGUI text;

    Vector2 strafeDirection = Vector2.zero;
    float tickTimer = 1.0f;
    float aggressiveness = 0.0f;

    private void FixedUpdate()
    {
        Transform playerTransform = Player.gameObject.transform;
        Vector2 horizPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos = new Vector2(playerTransform.position.x, playerTransform.position.z);
        Vector2 targetTo = targetPos - horizPos;
        float angle = Mathf.Atan2(targetTo.x, targetTo.y) * Mathf.Rad2Deg;
        Orientation.rotation = Quaternion.Euler(0, angle, 0);

        ModelRenderer.material.color = ModelRenderer.material.color + ((Color.white - ModelRenderer.material.color) * Time.deltaTime * 10f);

        text.text = string.Format("Timer:{0}\nAgressiveness: {1}\nStamina: {2}", tickTimer, aggressiveness, Stamina);

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
        ProcessMovement();
        LimitMovement();
    }
    public override void ApplyDamage(DamageInstance damageInstance)
    {
        if (CurrentAction == CurrentAction.Wait && Stamina >= 0.6f)
        {
            StartCoroutine(DoBlock());
        }
        base.ApplyDamage(damageInstance);
        ModelRenderer.material.color = Color.red;
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
                if (Stamina >= 0.9f)
                {
                    StartCoroutine(DoChargeAttack());
                }
                else if (Stamina >= 0.3f)
                {
                    StartCoroutine(DoAttack());
                }
                else if (Stamina >= 0.1f)
                {
                    StartCoroutine(DoBlock());
                }
            }
        }
        
    }
    void StartStrafe()
    {
        int strafeChoose = Random.Range(0, 3);

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
    void ProcessMovement()
    {
        MoveInfluence = Mathf.MoveTowards(MoveInfluence, 1f, Time.fixedDeltaTime);
        Vector3 moveDirection;
        moveDirection = Orientation.forward * strafeDirection.y + Orientation.right * strafeDirection.x;

        rb.AddForce(moveDirection.normalized * BaseSpeed * MoveForce * MoveInfluence, ForceMode.Force);
    }
    IEnumerator DoChargeAttack()
    {
        CurrentAction = CurrentAction.ChargeAttack;
        Stamina -= 0.4f;
        WeaponRenderer.enabled = true;
        yield return new WaitForSeconds(1);
        DamageInstance damageInstance = new DamageInstance();
        damageInstance.Stamina = 0.3f;
        damageInstance.Pushback = 240f;
        damageInstance.Direction = Orientation.forward;
        damageInstance.Amount = 30f;
        DoMeleeHit(damageInstance);
        yield return new WaitForSeconds(1.99f);
        WeaponRenderer.enabled = false;
        CurrentAction = CurrentAction.Wait;

    }
    IEnumerator DoAttack()
    {
        CurrentAction = CurrentAction.Attack;
        Stamina -= 0.3f;
        WeaponRenderer.enabled = true;
        yield return new WaitForSeconds(0.5f);
        DamageInstance damageInstance = new DamageInstance();
        damageInstance.Stamina = 0.1f;
        damageInstance.Pushback = 120f;
        damageInstance.Direction = Orientation.forward;
        damageInstance.Amount = 10f;
        DoMeleeHit(damageInstance);
        yield return new WaitForSeconds(0.49f);
        WeaponRenderer.enabled = false;
        CurrentAction = CurrentAction.Wait;
    }
    IEnumerator DoBlock()
    {
        Stamina -= 0.2f;
        Blocking = true;
        yield return new WaitForSeconds(0.5f);
        Blocking = false;
        CurrentAction = CurrentAction.Wait;
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
    }
}

public enum CurrentAction
{
    Wait,
    Attack,
    ChargeAttack,
    Block
}