using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopTrack : MonoBehaviour
{
    [SerializeField] private Sound[] Loops;

    private int currentLoopId;
    private float stepSize;

    void Awake()
    {
        CreateAudioSources();
        stepSize = 1.0f / Loops.Length;
    }


    void Update()
    {
        int nextLoopId = CalcNextLoopIndex(GameState.progress);
        if (nextLoopId != currentLoopId)
        {
            print("current time: " + AudioSettings.dspTime);
            print("progress: " + GameState.progress + ". next loop id: " + nextLoopId + ". step size: " + stepSize);
            PlayLoopScheduled(nextLoopId);
        }
    }

    public void PlayLoopScheduled(int index)
    {
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
                double timeScheduled = currentLoop.timeScheduled;

                while (timeScheduled < AudioSettings.dspTime)
                {
                    // double duration = (double)currentLoop.source.clip.samples / currentLoop.source.clip.frequency;
                    timeScheduled += currentLoop.duration;
                }

                currentLoop.source.SetScheduledEndTime(timeScheduled);
                s.source.PlayScheduled(timeScheduled);
                s.timeScheduled = timeScheduled;
                print("Scheduled loop " + index);

            }
            else
            {
                s.source.Play();
                s.timeScheduled = AudioSettings.dspTime;
                print("Playing loop " + index);
            }

            currentLoopId = index;
        }
    }

    public void StopAll()
    {
        Sound currentLoop = Array.Find(Loops, loop => loop.source.isPlaying);
        currentLoop.source.Stop();

        Sound[] scheduledLoops = Array.FindAll(Loops, loop => loop.timeScheduled > AudioSettings.dspTime);
        foreach (Sound loop in scheduledLoops)
        {
            loop.source.Stop();
        }
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
