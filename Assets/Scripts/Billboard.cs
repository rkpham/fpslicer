using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        GetComponent<RectTransform>().transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
