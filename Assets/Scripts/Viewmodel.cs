using UnityEngine;

public class Viewmodel : MonoBehaviour
{
    [SerializeField]
    Player Player;
    void AttackHit()
    {
        Player.DoAttack();
    }
    void AttackFinished()
    {
        Player.OnAttackFinished();
    }
}
