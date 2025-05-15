using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Boss bozz;
    public DeathBehavior deathBehavior;
    public EnvBehavior envBehavior;
    void Start()
    {
        bozz = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        envBehavior = GameObject.Find("NewEnv").GetComponent<EnvBehavior>();
        deathBehavior = GameObject.FindAnyObjectByType<DeathBehavior>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss")
        {
            IncThaStage(other.gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
            deathBehavior.DIE();
        }
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
