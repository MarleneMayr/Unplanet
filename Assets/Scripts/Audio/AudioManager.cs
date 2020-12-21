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
        Main,
        End, 
        Forshadowing,
        Found4,
        Found8,
        Transition1,
        Transition2,
    }

    [SerializeField] private Sound[] GlobalSounds;
    [SerializeField] private LoopTrack loopTrack;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private bool loopMode = true;


    private List<Sound> PausedSounds = new List<Sound>();

    void Awake()
    {
        SetGlobalAudioSources();
    }

    private void Update()
    {

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

    public void PlayScheduled(GlobalSound name, float secFromNow)
    {
        Sound s = FindSound(name);
        s?.source.PlayScheduled(AudioSettings.dspTime + secFromNow);
    }


    // GAME STATES //

    public void PlayFoundShort()
    {
        if (loopMode)
        {
            loopTrack.StopAll();
            loopTrack.SetLevelStartTime(6);
        }
        else
        {
            Stop(GlobalSound.Main);
            PlayScheduled(GlobalSound.Main, 6);
        }

        PlayOnce(GlobalSound.Found4);
    }

    public void PlayFoundLong()
    {
        if (loopMode)
        {
            loopTrack.StopAll();
            loopTrack.SetLevelStartTime(9);
        }
        else
        {
            Stop(GlobalSound.Main);
            PlayScheduled(GlobalSound.Main, 9);
        }

        PlayOnce(GlobalSound.Found8);
    }

    public void PlayEnd()
    {
        loopTrack.StopAll();
        Stop(GlobalSound.Main);
        PlayOnce(GlobalSound.End);
    }

    public void StartMusic()
    {
        PlayOnce(GlobalSound.Forshadowing);
        if (loopMode)
        {
            loopTrack.Activate();
            loopTrack.SetLevelStartTime(6);
        }
        else
        {
            PlayScheduled(GlobalSound.Main, 6);
        }

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
