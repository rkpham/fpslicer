using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public Entity entity;

    private void OnTriggerEnter(Collider other)
    {
        entity.OnAttackTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        entity.OnAttackTriggerExit(other);
    }
}
