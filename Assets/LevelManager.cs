using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Boss bozz;
    public GameObject bossObject;
    public DeathBehavior deathBehavior;
    public EnvBehavior envBehavior;
    void Start()
    {
        bossObject = GameObject.Find("Boss");
        bozz = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        envBehavior = GameObject.Find("NewEnv").GetComponent<EnvBehavior>();
        deathBehavior = GameObject.FindAnyObjectByType<DeathBehavior>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss")
        {
            IncThaStage();
        }
        else if (other.gameObject.tag == "Player")
        {
            deathBehavior.DIE();
        }
    }
    public void IncThaStage()
    {
        bozz.reSpawnYaCunt();
        envBehavior.NextStage();
    }

}
