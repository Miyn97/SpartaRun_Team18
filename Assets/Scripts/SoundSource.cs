using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource audioSource;

    public void Play(AudioClip clip, float volume = 1f, float pitchVariance = 0f)
    {
        if (clip == null) return;

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
        audioSource.Play();

        Destroy(gameObject, clip.length + 0.1f); // 자동 파괴
    }

    // 볼륨을 설정하는 메서드
    public void SetVolume(float sfxVolume, float bgmVolume)
    {
        if (audioSource != null)
        {
            audioSource.volume = bgmVolume;
        }
    }
}