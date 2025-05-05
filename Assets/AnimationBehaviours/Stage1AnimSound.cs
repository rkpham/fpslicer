using UnityEngine;

public class Stage1AnimSound : StateMachineBehaviour
{
    private bool hasTriggered = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasTriggered = false; // Reset at start of state
        EnvAnimAudio envAudio = animator.GetComponentInChildren<EnvAnimAudio>();
        if (envAudio != null)
        {
            float timeRemaining = (1f - stateInfo.normalizedTime) * stateInfo.length / animator.speed;
            envAudio.PlayAudio(envAudio.b[0], 1f, timeRemaining); // correct cutTime
            Debug.Log($"envaudio found, timeRemaining: {timeRemaining}");
        }
        else
        {
            Debug.LogWarning("EnvAnimAudio not found on animator children!");
        }

        Debug.Log("state entered");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasTriggered && stateInfo.normalizedTime >= 1f)
        {
            EnvAnimAudio envAudio = animator.GetComponentInChildren<EnvAnimAudio>();
            if (envAudio != null)
            {
                envAudio.PlayAudio(envAudio.b[2], 1f); // Not stateInfo.length — you likely want standard volume 1f
                Debug.Log("envaudio played at animation finish");
            }
            else
            {
                Debug.LogWarning("EnvAnimAudio not found on animator children!");
            }

            hasTriggered = true; // Make sure it only fires once
        }
    }
}