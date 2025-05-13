using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    IntroScene,
    MainScene,
    StartScene
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Scene BGM Clips")]
    public AudioClip IntroBGM;
    public AudioClip MainBGM;
    public AudioClip StartBGM;

    [Header("Effect Sound Clips")]
    public List<AudioClip> sfxClips = new List<AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동해도 유지
        }
        else
        {
            Destroy(gameObject); // 중복 제거
        }
    }

    public void PlaySceneBGM(SoundType type)
    {
        AudioClip clipToPlay = null;

        switch (type)
        {
            case SoundType.IntroScene:
                clipToPlay = IntroBGM;
                break;
            case SoundType.MainScene:
                clipToPlay = MainBGM;
                break;
            case SoundType.StartScene:
                clipToPlay = StartBGM;
                break;
            default:
                Debug.LogWarning("존재하지 않는 씬의 BGM입니다.");
                return;
        }

        if (clipToPlay != null)
        {
            // 카메라 기준 위치에서 한 번만 재생
            AudioSource.PlayClipAtPoint(clipToPlay, Camera.main.transform.position);
        }
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }

}
