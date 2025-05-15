using UnityEngine;

public class GawkerBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int _intensity;
    public int BaseGlow;
    public int Intensity
    {
        get { return _intensity; }
        set
        {
            _intensity = value + BaseGlow;
            GetComponent<Light>().intensity = _intensity * 1000;
        }
    }

    public void setIntensity(int intensity)
    {
        Intensity = intensity;
    }

}
