using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopTrack : MonoBehaviour
{
    [SerializeField] private Sound[] Loops;
    [SerializeField] private AudioManager audioManager;

    private int currentLoopId;
    private float stepSize;
    private double levelStartTime = 0;
    public bool active = false;

    void Awake()
    {
        CreateAudioSources();
        stepSize = 1.0f / Loops.Length;
    }


    void Update()
    {
        int nextLoopId = CalcNextLoopIndex(GameState.distance);
        if (active && nextLoopId != currentLoopId)
        {
            PlayLoopScheduled(nextLoopId);
        }
    }

    public void PlayLoopScheduled(int index)
    {
        print("called play scheduled on loop " + index);
        Sound s = Loops[index];

        if (s != null && !s.source.isPlaying)
        {
            Sound[] scheduledLoops = Array.FindAll(Loops, loop => loop.timeScheduled > AudioSettings.dspTime);

            foreach(Sound loop in scheduledLoops)
            {
                loop.source.Stop();
            }

            Sound currentLoop = Array.Find(Loops, loop => loop.source.isPlaying);

            if (currentLoop != null)
            {
                double timeScheduled = levelStartTime > AudioSettings.dspTime ? levelStartTime : currentLoop.timeScheduled;

                while (timeScheduled < AudioSettings.dspTime)
                {
                    // double duration = (double)currentLoop.source.clip.samples / currentLoop.source.clip.frequency;
                    timeScheduled += (currentLoop.duration / 2);
                    //timeScheduled += currentLoop.duration;
                }

                currentLoop.source.SetScheduledEndTime(timeScheduled);
                AudioFade.ScheduledFadeOut(currentLoop.source, 0.1f, (float)timeScheduled);

                if (index == 0)
                {
                    audioManager.PlayScheduled(AudioManager.GlobalSound.Transition1, (float)(timeScheduled - AudioSettings.dspTime));
                    timeScheduled += audioManager.FindSound(AudioManager.GlobalSound.Transition1).duration;
                }

                s.source.PlayScheduled(timeScheduled);
                s.timeScheduled = timeScheduled;
            }
            else
            {
                if (levelStartTime > AudioSettings.dspTime)
                {
                    s.source.PlayScheduled(levelStartTime);
                    s.timeScheduled = levelStartTime;
                }
                else
                {
                    s.source.Play();
                    s.timeScheduled = AudioSettings.dspTime;
                }
                
            }

            currentLoopId = index;
        }
    }

    public void StopAll()
    {
        Sound[] currentLoops = Array.FindAll(Loops, loop => loop.source.isPlaying);
        foreach (Sound loop in currentLoops)
        {
            loop.source.Stop();
            // AudioFade.FadeOut(loop?.source, 0.05f);
        }

        Sound[] scheduledLoops = Array.FindAll(Loops, loop => loop.timeScheduled > AudioSettings.dspTime);
        foreach (Sound loop in scheduledLoops)
        {
            loop.source.Stop();
        }
    }

    public void SetLevelStartTime(float secFromNow)
    {
        levelStartTime = AudioSettings.dspTime + secFromNow;
    }

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        StopAll();
        active = false;
    }


    private int CalcNextLoopIndex(float progress)
    {
        return (int)Math.Floor(progress / stepSize);   
    }


    private void CreateAudioSources()
    {
        foreach (Sound s in Loops)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = s.output;
            s.source.loop = true;
        }
    }
}
