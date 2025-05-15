using UnityEngine;

public class GawkerBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int _intensity;
    public int BaseGlow;
    public AudioSource[] a;
    public int Intensity
    {
        get { return _intensity; }
        set
        {
            _intensity = value;
            GetComponent<Light>().intensity = (_intensity + BaseGlow)* 1000;
            songChange(value);
        }
    }

    private void Start()
    {
        songChange(0);
    }

    public void setIntensity(int intensity)
    {
        Intensity = intensity;
    }

    public void songChange(int i)
    {
        for(int b =  0; b < a.Length; b++)
        {
            a[b].volume = 0;
        }
        if (i < a.Length)
        {
            a[i].volume = 1;
        }
        else
        {
            a[0].volume = 1;
        }
    }

}
