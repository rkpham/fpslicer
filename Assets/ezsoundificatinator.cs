using Unity.VisualScripting;
using UnityEngine;

public class ezsoundificatinator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip[] a;

    public void playDaBih()
    {
        if(a.Length == 0)
        {
            Debug.Log("Disbih empty: " + gameObject.name);
            return;
        }
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = a[Random.Range(1, a.Length)];
        GetComponent<AudioSource>().Play();
    }
}
