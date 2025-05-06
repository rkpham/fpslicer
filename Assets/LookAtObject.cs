using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform t;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(t);
    }
}
