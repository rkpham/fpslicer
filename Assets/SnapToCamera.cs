using UnityEngine;

public class SnapToCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform t;
    void Start()
    {
        t = Camera.main.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = t.position;
        transform.rotation = t.rotation;
    }
}
