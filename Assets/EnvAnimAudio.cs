using UnityEngine;
using UnityEngine.Audio;

public class EnvAnimAudio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MultisourceAudioBehavior High;
    public AudioClip[] highSounds;
    public MultisourceAudioBehavior Wide;
    public AudioClip[] wideSounds;
    public MultisourceAudioBehavior Global;
    public AudioClip[] globalSounds;
    private bool waitingForZZTOP = false;


    //First field is HIGH, second is WIDE, third is GLOBAL
    float[,] volumes = new float[,]
    {
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 },
    };
}
