using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using SPStudios.Tools;
public class SoundManager : PersistentMonoSingleton
{
    [SerializeField] Stem[] m_stems;
    public void SmoothSound(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(FadeSoundOn(audioSource, fadeTime, audioSource.volume));
    }
    IEnumerator FadeSoundOn(AudioSource audioSource, float fadeTime, float currentVolume)
    {
        if (audioSource == null) yield return null;
        yield return FadeSoundOff(audioSource, fadeTime, currentVolume);
        var t = 0f;
        while (t < fadeTime)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            audioSource.volume = Mathf.Clamp(t, 0, currentVolume);
        }
    }
    IEnumerator FadeSoundOff(AudioSource audioSource, float fadeTime, float currentVolume)
    {
        var t = fadeTime;
        while (t > 0)
        {
            yield return new WaitForEndOfFrame();
            t -= Time.deltaTime;
            if (audioSource != null) audioSource.volume = Mathf.Clamp(t, 0, currentVolume);
        }
    }

    public void SetStem(int index, AudioClip clip)
    {
        if (m_stems.Length <= index || index < 0)
        {
            Debug.LogError("Trying to set an undefined stem");
            return;
        }

        m_stems[index].clip = clip;
    }
    public AudioClip GetStem(int index)
    {
        return m_stems.Length <= index && index < 0 ? null : m_stems[index].clip;
    }
    //--------------------
    [System.Serializable]
    public class Stem
    {
        public AudioSource source;
        public AudioClip clip;
    }
}
