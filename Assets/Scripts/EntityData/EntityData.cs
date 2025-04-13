using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/EntityData")]
public class EntityData : ScriptableObject
{
    public delegate void OnDamaged(DamageInstance instance);
    public event OnDamaged onDamaged;
    public delegate void OnDied();
    public event OnDied onDied;

    private const float _maxHealth = 100f;
    private float _health = _maxHealth;
    public float Health => _health;
    private float _baseSpeed = 16.0f;
    public float BaseSpeed => _baseSpeed;
    


    public void ApplyDamage(DamageInstance damageInstance)
    {
        _health -= damageInstance.Amount;
        if (_health < 0)
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