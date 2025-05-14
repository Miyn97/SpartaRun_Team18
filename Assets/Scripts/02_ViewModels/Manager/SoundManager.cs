using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Volume")]
    [Range(0f, 10f)] public float sfxVolume = 5f;
    [Range(0f, 10f)] public float bgmVolume = 5f;

    [Header("Prefabs")]
    public GameObject soundSourcePrefab;

    [Header("Scene BGM Clips")]
    public AudioClip introBgm;
    public AudioClip startBgm;
    public AudioClip mainBgm;

    private SoundPool sfxPool;
    private SoundSource bgmSource;

    private const string KEY_SFX = "SFX_Volume";
    private const string KEY_BGM = "BGM_Volume";

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log($"[SoundManager] Awake in scene {SceneManager.GetActiveScene().name}");
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 사운드 조절 후 게임 재실행 시 default 값을 5로 지정
            sfxVolume = PlayerPrefs.GetFloat(KEY_SFX, 5f);
            bgmVolume = PlayerPrefs.GetFloat(KEY_BGM, 5f);

            sfxPool = new SoundPool(soundSourcePrefab, transform);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ensure initial scene BGM
        PlaySceneBGM(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[SoundManager] SceneLoaded: {scene.name} → PlaySceneBGM");
        PlaySceneBGM(scene.name);
    }

    private void PlaySceneBGM(string sceneName)
    {
        AudioClip clip = null;
        switch (sceneName)
        {
            case "03_IntroScene":
                clip = introBgm;
                break;
            case "01_StartScene":
                clip = startBgm;
                break;
            case "02_MainScene":
                clip = mainBgm;
                break;
        }
        if (clip != null)
        {
            PlayBGM(clip);
        }
    }

    public void PlaySFX(AudioClip clip, float pitchVar = 0f)
    {
        if (clip == null) return;

        var sfx = sfxPool.Get(loop: false, volume: sfxVolume);
        sfx.Play(clip, pitchVar);
        StartCoroutine(ReturnAfterPlay(sfx, clip.length));
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        if (bgmSource == null)
            bgmSource = sfxPool.Get(loop: true, volume: bgmVolume);
        else
            bgmSource.SetVolume(bgmVolume);

        bgmSource.Play(clip);
    }

    public void StopBGM() => bgmSource?.Stop();

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat(KEY_SFX, value);
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        PlayerPrefs.SetFloat(KEY_BGM, value);
        bgmSource?.SetVolume(bgmVolume);
    }

    private IEnumerator ReturnAfterPlay(SoundSource sfx, float duration)
    {
        yield return new WaitForSeconds(duration + 0.1f);
        sfxPool.Return(sfx);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
