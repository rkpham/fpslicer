using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter(Collider other)
    {
        player.OnAttackTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        player.OnAttackTriggerExit(other);
    }
}
