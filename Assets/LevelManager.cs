using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Boss bozz;
    public EnvBehavior envBehavior;
    void Start()
    {
        bozz = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        envBehavior = GameObject.Find("NewEnv").GetComponent<EnvBehavior>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        IncThaStage(other.gameObject);
    }
    //FOR TESTING
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncThaStage(GameObject.Find("Boss"));
        }
    }
    private void IncThaStage(GameObject other)
    {
        if (bozz != null && other.tag == "Boss")
        {
            bozz.reSpawnYaCunt();
        }
        envBehavior.NextStage();
    }

}
