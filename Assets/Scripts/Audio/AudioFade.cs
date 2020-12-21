using System;
using System.Collections;
using UnityEngine;

public static class AudioFade
{

    public static IEnumerator FadeIn(AudioSource audioSource, float fadeTimeInSec)
    {
        float targetVolume = audioSource.volume;
        float startVolume = 0;  

        float currentTime = 0;

        audioSource.volume = startVolume;
        audioSource.Play();

        while (currentTime < fadeTimeInSec)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeTimeInSec);
            yield return null;
        }

        audioSource.volume = targetVolume;
        yield break;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTimeInSec)
    {
        float targetVolume = 0;
        float startVolume = audioSource.volume;

        float currentTime = 0;

        while (currentTime < fadeTimeInSec)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeTimeInSec);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        yield break;
    }

    public static IEnumerator ScheduledFadeOut(AudioSource audioSource, float fadeTimeInSec, float timeScheduled)
    {
        audioSource.SetScheduledEndTime(timeScheduled);
        yield return new WaitForSeconds(timeScheduled - (float)AudioSettings.dspTime - fadeTimeInSec);

        float targetVolume = 0;
        float startVolume = audioSource.volume;

        float currentTime = 0;

        while (currentTime < fadeTimeInSec)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeTimeInSec);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        yield break;
    }
}
