using UnityEngine;

public class IntroAnimationBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool isDoneYet = false;
    LevelManager manager;
    Boss boss;
    void Start()
    {
        manager = GameObject.FindAnyObjectByType<LevelManager>();
        boss = GameObject.FindAnyObjectByType<Boss>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!isDoneYet)
        {
            isDoneYet=true;
        }
        else
        {
            manager.IncThaStage(GameObject.Find("Boss"));
        }
    }
}
