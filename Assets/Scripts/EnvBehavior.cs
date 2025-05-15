using UnityEngine;

public class EnvBehavior : MonoBehaviour
{
    // Variables
    public int _stage;
    public EnvAnimAudio a;
    public GawkerBehaviour g;
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
            g.setIntensity(_stage);
        }
    }
    private Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        a = GetComponentInChildren<EnvAnimAudio>();
        g = GameObject.Find("Gawker").GetComponent<GawkerBehaviour>();
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
