using UnityEngine;

public class SoundSource : MonoBehaviour, ISoundPlayer
{
    private AudioSource source;
    private float currentVolume;

    public void Init(bool loop, float volume)
    {
        if (source == null)
            source = gameObject.AddComponent<AudioSource>();
        source.loop = loop;
        currentVolume = volume;
        source.volume = volume;
    }

    public void Play(AudioClip clip, float pitchVar = 0f)
    {
        if (clip == null) return;
        source.clip = clip;
        source.pitch = 1f + Random.Range(-pitchVar, pitchVar);
        source.volume = currentVolume;
        source.Play();
    }

    public void Stop() => source?.Stop();
    public void SetVolume(float volume)
    {
        currentVolume = volume;
        if (source != null) source.volume = volume;
    }

    public bool IsPlaying => source != null && source.isPlaying;
}