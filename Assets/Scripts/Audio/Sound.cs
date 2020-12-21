using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioManager.GlobalSound name;
    public AudioClip clip;
    public AudioMixerGroup output;

    [Range(0, 40)]
    public int duration;

    [Range(0f, 1f)]
    public float volume = 1;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    public double timeScheduled;
}
