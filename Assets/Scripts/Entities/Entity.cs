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
    public float BaseSpeed = 16.0f;
    public float MaxSpeed = 3f;
    public float MoveForce = 8f;
    public float MoveInfluence = 1.0f;
    public bool Blocking = false;

    protected List<Collider> currentAttackColliders = new List<Collider>();

    protected Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    virtual public void ApplyDamage(DamageInstance damageInstance)
    {
        MoveInfluence = 0.0f;
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
    protected void LimitMovement()
    {
        Vector3 horizVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (horizVelocity.magnitude > MaxSpeed)
        {
            Vector3 limitedHorizVelocity = horizVelocity.normalized * MaxSpeed;
            rb.linearVelocity = new Vector3(limitedHorizVelocity.x, rb.linearVelocity.y, limitedHorizVelocity.z);
        }
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