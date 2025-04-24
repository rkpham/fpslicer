using UnityEngine;

public class Entity : MonoBehaviour
{
    public delegate void OnDamaged(DamageInstance instance);
    public event OnDamaged onDamaged;
    public delegate void OnDied();
    public event OnDied onDied;

    public float MaxHealth = 100f;
    public float Health = 100f;
    public float BaseSpeed = 16.0f;
    public bool Blocking = false;

    protected Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    virtual public void ApplyDamage(DamageInstance damageInstance)
    {
        Debug.Log("Took damage!");
        Health -= damageInstance.Amount;
        if (Health < 0)
        {
            Die();
        }
        onDamaged?.Invoke(damageInstance);
    }
    virtual public void Die()
    {
        onDied?.Invoke();
    }
}

public struct DamageInstance
{
    public float Amount;
    public float Stamina;
    public float Pushback;
    public Vector3 Direction;
}