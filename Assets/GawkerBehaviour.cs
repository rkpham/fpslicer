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

    public void setIntensity(int intensity)
    {
        Intensity = intensity;
    }

    public void songChange(int i)
    {
        foreach (AudioSource source in a)
        {
            source.volume = 0;
        }
        a[i].volume = 1;
    }

}
