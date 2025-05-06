using UnityEngine;

public class cameraMatcher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Transform>().position = Vector3.zero;
    }
}

