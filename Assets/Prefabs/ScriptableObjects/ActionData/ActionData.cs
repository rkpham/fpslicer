using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public abstract class ActionData : ScriptableObject
{
    public AnimatorOverrideController AnimatorOverrideController;
    [Header("Action Combinations")]
    public ActionData AttackChain;
    public ActionData BlockChain;
    public ActionData JumpChain;
    public ActionData FlourishChain;
    [Header("Action Timing")]
    public bool InterruptImmediately;
    public float WindupLength = -1;
    public float ChargeThreshold = -1; // Minimum charge to progress to active; otherwise, use LowChargeDefault
    public float MaxChargeLength = -1; // Maximum charge before being auto released
    public float ActiveLength = -1;
    public float AttackComboTimeFromEnd;
    public float BlockComboTimeFromEnd;
    public float JumpComboTimeFromEnd;
    public float FlourishComboTimeFromEnd;
    public float AttackRecoveryTime;
    public float BlockRecoveryTime;
    public float JumpRecoveryTime;
    public float FlourishRecoveryTime;
    [Header("Action Properties")]
    public ActionData LowChargeDefault;
    public bool CanCharge;
    public bool RepeatCharge; // Return to charge after active state
    public bool IsAttack;
    public bool Unblockable;
    public bool ManualActive; // Active state must be triggered in code
    public float Damage = 5f;
    public float StaminaDamage = 0.5f;
    public float Pushback = 0.5f;
    public float SpeedMult = 0.5f;
    [Header("Movement Properties")]
    public Vector3 moveForce = Vector3.zero;
    public bool inputModulate = true;
    public ActionStage forceApplyStage = ActionStage.Active;
    public abstract void OnActionWindupStarted(Player player);
    public abstract void OnActionChargeStarted(Player player);
    public abstract void OnActionActiveStarted(Player player);
    public abstract void OnActionFinished(Player player);
}
