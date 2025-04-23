using UnityEngine;

[CreateAssetMenu(fileName = "BasicActionData", menuName = "Scriptable Objects/BasicActionData")]
public class BasicActionData : ActionData
{
    public override void OnActionActiveStarted(Player player) {}

    public override void OnActionChargeStarted(Player player) {}

    public override void OnActionFinished(Player player) {}

    public override void OnActionWindupStarted(Player player) {}
}
