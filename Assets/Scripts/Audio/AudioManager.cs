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
        Steps,
        End, 
        Forshadowing,
        Found4,
        Found8,
        Transition1,
        Transition2,
        Loop1
    }

    [SerializeField] private Sound[] GlobalSounds;
    [SerializeField] private Sound[] Loops;

    private Sound currentLoop;
    private Sound nextLoop;

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

    public void PlayLoopScheduled(int index)
    {
        Sound s = Loops[index];

        if (s != null && !s.source.isPlaying)
        {
            if (nextLoop != null)
            {
                nextLoop.source.Stop(); // if next loop already scheduled, stop it
            }

            double timeScheduled = AudioSettings.dspTime;

            if (currentLoop != null)
            {
                 timeScheduled = currentLoop.timeScheduled + currentLoop.duration; // set scheduled time to end time of current loop
            }

            currentLoop.source.SetScheduledEndTime(timeScheduled); // end time for current loop
            s.source.SetScheduledStartTime(timeScheduled);
            s.timeScheduled = timeScheduled;
            nextLoop = s;
        }
    }

    public void Stop(GlobalSound name)
    {
        Sound s = FindSound(name);
        s?.source.Stop();
    }

    public void StopMusic()
    {
        if (currentLoop != null) nextLoop.source.Stop();
        if (nextLoop != null) nextLoop.source.Stop();
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
        StopMusic();
        PlayOnce(GlobalSound.Found4);
    }

    public void PlayFoundLong()
    {
        StopMusic();
        PlayOnce(GlobalSound.Found8);
    }

    public void StartMusic()
    {
        currentLoop = FindSound(GlobalSound.Forshadowing);
        PlayOnce(GlobalSound.Forshadowing);
    }

    public void PlayEnd()
    {
        StopMusic();
        PlayOnce(GlobalSound.End);
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

        foreach (Sound s in Loops)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = true;
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
