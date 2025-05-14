using UnityEngine;

public class waterKillThaBubble : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Optional: only destroy objects with BubbleButtbehavior to avoid unintended deletes
        BubbleButtbehavior bubble = other.GetComponent<BubbleButtbehavior>();
        if (bubble != null)
        {
            bubble.killThyself();
        }
    }
}
