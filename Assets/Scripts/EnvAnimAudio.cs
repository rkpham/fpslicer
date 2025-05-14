using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class EnvAnimAudio : MonoBehaviour
{
    public AudioSource[] a;
    public AudioClip[] b;
    private void Start()
    {
        a = GetComponents<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    { 
        PlayAudio(clip, 1f); // Default volume 1
    }

    public void PlayAudio(AudioClip clip, float volume)
    {
        AudioSource toUse = FindAvailableSource();
        if (toUse != null)
        {
            toUse.clip = clip;
            toUse.volume = volume;
            toUse.Play();
        }
        else
        {
            Debug.Log("No available AudioSources");
        }
    }

    public void PlayAudio(AudioClip clip, float volume, float cutTime)
    {
        AudioSource toUse = FindAvailableSource();
        if (toUse != null)
        {
            toUse.clip = clip;
            toUse.volume = volume;
            toUse.Play();
            StartCoroutine(StopAfterSeconds(toUse, cutTime));
        }
        else
        {
            Debug.Log("No available AudioSources");
        }
    }

    private AudioSource FindAvailableSource()
    {
        foreach (AudioSource source in a)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }

    IEnumerator StopAfterSeconds(AudioSource source, float seconds)
    {
        AudioClip startingClip = source.clip;
        yield return new WaitForSeconds(seconds);

        if (source != null && source.isPlaying && source.clip == startingClip)
        {
            source.Stop();
        }
    }
}
