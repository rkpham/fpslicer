using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    Volume volume;
    Vignette vignette;

    private void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
    }

    private void FixedUpdate()
    {
        vignette.color.value = Color.Lerp(vignette.color.value, new Color(0f, 0f, 0f), Time.fixedDeltaTime * 4f);
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.3f, Time.fixedDeltaTime * 4f);
    }
    public void DamageEffect()
    {
        vignette.color.value = Color.red;
        vignette.intensity.value = 0.6f;
    }
}
