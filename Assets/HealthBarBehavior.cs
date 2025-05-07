using UnityEngine;

public class HealthBarBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Player player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Set y transform to the health as a percentage.
    }
}
