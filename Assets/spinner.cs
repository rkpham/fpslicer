using UnityEngine;

public class spinner : MonoBehaviour
{
    public float Speed;
    void Update()
    {
        transform.Rotate(0, Time.deltaTime*Speed, 0);
    }
}
