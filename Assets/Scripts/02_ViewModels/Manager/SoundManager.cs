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
            DontDestroyOnLoad(gameObject); // �� �̵��ص� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
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
                Debug.LogWarning("�������� �ʴ� ���� BGM�Դϴ�.");
                return;
        }

        if (clipToPlay != null)
        {
            // ī�޶� ���� ��ġ���� �� ���� ���
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
