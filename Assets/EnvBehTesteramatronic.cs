using UnityEngine;


public class EnvBehTesteramatronic : MonoBehaviour
{
    [SerializeField] public EnvBehavior a;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            a.NextStage();
        }
    }
}
