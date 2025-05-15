using UnityEngine;

public class windBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator a;
    public AudioSource aSource;
    // Update is called once per frame
    void Update()
    {
        aSource.volume = (a.GetInteger("Stage") + 5) / 10f;
    }
}
