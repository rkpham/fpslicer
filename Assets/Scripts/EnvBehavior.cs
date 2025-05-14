using UnityEngine;

public class EnvBehavior : MonoBehaviour
{
    // Variables
    public int _stage;
    public EnvAnimAudio a;
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
            a?.PlayAudio(a.b[1], 1);
            if(_stage > 4 && a!=null)
            {
                a = null;
            }
        }
    }
    private Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        a = GetComponentInChildren<EnvAnimAudio>();
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
