using UnityEngine;

[CreateAssetMenu(fileName = "BlockActionData", menuName = "Scriptable Objects/BlockActionData")]
public class BlockActionData : ActionData
{
    public override void OnActionWindupStarted(Player player) {}

    public override void OnActionChargeStarted(Player player)
    {
        player.EntityData.Blocking = true;
    }
    public override void OnActionActiveStarted(Player player)
    {
        player.EntityData.Blocking = false;
    }
    public override void OnActionFinished(Player player) {}
}
