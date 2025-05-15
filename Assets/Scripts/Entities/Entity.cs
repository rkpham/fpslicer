using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public delegate void OnDamaged(DamageInstance instance);
    public event OnDamaged onDamaged;
    public delegate void OnDied();
    public event OnDied onDied;

    public float MaxHealth = 100f;
    public float Health = 100f;
    public float Stamina = 1.0f;
    public float Friction = 3f;
    public float MaxSpeed = 3f;
    public float AccelerationMult = 16f;
    public float AirAccelerationMult = 4f;
    public float MaxAcceleration = 3f;
    public float MoveInfluence = 1.0f;
    public bool Blocking = false;
    public bool IsGrounded = false;

    protected List<Collider> currentAttackColliders = new List<Collider>();
    protected Rigidbody rb;

    [SerializeField] private Collider entCollider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    virtual public void ApplyDamage(DamageInstance damageInstance)
    {
        MoveInfluence = 0.001f;
        if (Blocking && damageInstance.Blockable)
        {
            rb.AddForce(damageInstance.Direction * damageInstance.Pushback * 0.5f);
        }
        else
        {
            rb.AddForce(damageInstance.Direction * damageInstance.Pushback);
            Health -= damageInstance.Amount;
            Stamina -= damageInstance.Stamina;
            onDamaged?.Invoke(damageInstance);
        }
        if (Health < 0)
        {
            Die();
        }
    }
    protected void GetGrounded()
    {
        IsGrounded = Physics.Raycast(transform.position, -Vector3.up, entCollider.bounds.extents.y + 0.1f);
    }
    void ApplyFriction(float speed, ref Vector3 velocity)
    {
        var drop = 0.0f;
        if (speed < 0.00191)
        {
            return;
        }
        if (IsGrounded)
        {
            double stopSpeed = (5.0 / 16.0) * MaxSpeed;
            double control = (speed < stopSpeed) ? stopSpeed : speed;
            drop = (float)(control * Friction * Time.fixedDeltaTime);
        }

        float newSpeed = speed - drop;
        if (newSpeed < 0)
            newSpeed = 0;
        newSpeed /= speed;
        velocity *= newSpeed;
    }
    virtual protected void HandleMovement(Vector2 direction)
    {
        MoveInfluence = Mathf.MoveTowards(MoveInfluence, 1f, Time.fixedDeltaTime);
        MoveInfluence = Mathf.Clamp01(MoveInfluence);

        Vector3 vel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        ApplyFriction(vel.magnitude, ref vel);

        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.z);

        Vector3 accelDir = (transform.forward * direction.y + transform.right * direction.x).normalized;
        float veer = Vector3.Dot(vel, accelDir.normalized);
        float addSpeed;
        if (IsGrounded)
            addSpeed = MaxSpeed - veer;
        else
            addSpeed = (3f / 40f) * MaxSpeed - veer;

        addSpeed *= MoveInfluence;

        if (addSpeed < 0f)
            addSpeed = 0f;

        float accelSpeed;
        if (IsGrounded)
        {
            accelSpeed = AccelerationMult * Time.fixedDeltaTime * MaxSpeed;
        }
        else
        {
            accelSpeed = AirAccelerationMult * Time.fixedDeltaTime * MaxSpeed;
        }
        accelSpeed = Mathf.Clamp(accelSpeed, 0f, addSpeed);

        rb.linearVelocity += accelDir * accelSpeed;
    }
    virtual public void Die()
    {
        onDied?.Invoke();
    }
    public void OnAttackTriggerEnter(Collider collider)
    {
        currentAttackColliders.Add(collider);
    }
    public void OnAttackTriggerExit(Collider collider)
    {
        currentAttackColliders.Remove(collider);
    }
}

public struct DamageInstance
{
    public float Amount;
    public float Stamina;
    public float Pushback;
    public bool Blockable;
    public Vector3 Direction;
}