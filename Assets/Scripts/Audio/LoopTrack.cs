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
        stepSize = 1 / Loops.Length;
    }


    void Update()
    {
        int nextLoopId = CalcNextLoopIndex(GameState.progress);
        if (nextLoopId != currentLoopId)
        {
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
            double timeScheduled = AudioSettings.dspTime;

            if (currentLoop != null)
            {
                timeScheduled = currentLoop.timeScheduled;

                while (AudioSettings.dspTime > timeScheduled)
                {
                    timeScheduled += currentLoop.duration; // set scheduled time to end time of current loop
                }
            }

            currentLoop.source.SetScheduledEndTime(timeScheduled); // end time for current loop
            s.source.SetScheduledStartTime(timeScheduled);
            s.timeScheduled = timeScheduled;
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
            s.source.loop = true;
        }
    }
}
