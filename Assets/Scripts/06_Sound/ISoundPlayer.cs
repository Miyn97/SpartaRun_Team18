using UnityEngine;

public interface ISoundPlayer
{
    void Play(AudioClip clip, float pitchVar = 0f);
    void Stop();
    void SetVolume(float volume);
}