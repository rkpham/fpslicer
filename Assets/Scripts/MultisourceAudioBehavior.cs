using UnityEngine;
using UnityEngine.Rendering;

public class MultisourceAudioBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource[] _audioSource;

    // Update is called once per frame
    public void PlayAudio(AudioClip _audioclip, float _volume)
    {
        foreach(AudioSource a in _audioSource)
        {
            a.Pause();
            a.clip = _audioclip;
            a.volume = _volume;
            a.Play();
        }
    }
}
