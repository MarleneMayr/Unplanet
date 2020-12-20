using System.Collections.Generic;
using UnityEngine;


/*
 * AudioManager from Stonehenge AR application
 * use with caution and adapt to new surroundings
 * Thanks
 */


public class AudioManager : MonoBehaviour
{
    public enum GlobalSound
    {
        Loop,
        Steps,
        End, 
        Forshadowing,
        Found4,
        Found8,
        Transition1,
        Transition2,
    }

    [SerializeField] private Sound[] GlobalSounds;
    [SerializeField] private LoopTrack loopTrack;

    private List<Sound> PausedSounds = new List<Sound>();

    void Awake()
    {
        SetGlobalAudioSources();
    }


    // PLAY METHODS //

    public void Play(GlobalSound name)
    {
        Sound s = FindSound(name);
        s?.source.Play();
    }

    public void PlayOnce(GlobalSound name)
    {
        Sound s = FindSound(name);

        if (s != null && !s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(GlobalSound name)
    {
        Sound s = FindSound(name);
        s?.source.Stop();
    }

    public void PauseIfPlaying(GlobalSound name)
    {
        Sound s = FindSound(name);

        if (s != null && s.source.isPlaying)
        {
            s.source.Pause();
            PausedSounds.Add(s);
        }
    }

    public void ResumeIfPaused(GlobalSound name)
    {
        Sound s = FindSound(name);

        if (s != null && PausedSounds.Contains(s))
        {
            s.source.Play();
            PausedSounds.Remove(s);
        }
    }


    // GAME STATES //

    public void PlayFoundShort()
    {
        loopTrack.StopAll();
        PlayOnce(GlobalSound.Found4);
    }

    public void PlayFoundLong()
    {
        loopTrack.StopAll();
        PlayOnce(GlobalSound.Found8);
    }

    public void PlayEnd()
    {
        loopTrack.StopAll();
        PlayOnce(GlobalSound.End);
    }

    public void StartMusic()
    {
        loopTrack.PlayLoopScheduled(0);
    }


    // SETUP //

    private void SetGlobalAudioSources()
    {
        foreach (Sound s in GlobalSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    private Sound FindSound(GlobalSound name)
    {
        Sound s = System.Array.Find(GlobalSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound '" + name + "' does not exist in GlobalSounds Array!");
            return null;
        }
        return s;
    }
}
