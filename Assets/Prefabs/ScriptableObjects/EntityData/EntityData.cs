using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/EntityData")]
public class EntityData : ScriptableObject
{
    public delegate void OnDamaged(DamageInstance instance);
    public event OnDamaged onDamaged;
    public delegate void OnDied();
    public event OnDied onDied;

    public float MaxHealth = 100f;
    public float Health = 100f;
    public float BaseSpeed = 16.0f;
    public bool Blocking = false;
    
    public void ApplyDamage(DamageInstance damageInstance)
    {
        Health -= damageInstance.Amount;
        if (Health < 0)
        {
            Die();
        }
        if (onDamaged != null)
        {
            onDamaged(damageInstance);
        }
    }
    virtual public void Die()
    {
        if (onDied != null)
        {
            onDied();
        }
    }
}

public struct DamageInstance
{
    public float Amount;
}