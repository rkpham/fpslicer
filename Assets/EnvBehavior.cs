using UnityEngine;

public class EnvBehavior : MonoBehaviour
{
    // Variables
    private int _stage;
    public int stage
    {
        get
        {
            return _stage;
        }
        private set
        {
            _stage = value;
            runAnim(value);
        }
    }
    private Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        stage = 0;
    }

    private void runAnim(int whichAnim)
    {
        m_Animator.SetInteger("Stage", whichAnim);
    }

    public void NextStage()
    {
        SetStage(stage+1);
    }

    public void SetStage(int whichStageVro)
    {
        stage = whichStageVro;
    }

}
