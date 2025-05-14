using UnityEngine;
using UnityEngine.UI;

public class OptionPanelController : MonoBehaviour
{
    [Header("Slider References")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // 초기화: SoundManager의 현재 볼륨으로 슬라이더 값 설정
        if (SoundManager.Instance != null)
        {
            bgmSlider.value = SoundManager.Instance.bgmVolume;
            sfxSlider.value = SoundManager.Instance.sfxVolume;
        }

        // 슬라이더 이벤트 등록
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnBGMVolumeChanged(float value)  // 배경음악 조절 슬라이더
    {
        SoundManager.Instance?.SetBGMVolume(value);
    }

    private void OnSFXVolumeChanged(float value) // 효과음 조절 슬라이더
    {
        SoundManager.Instance?.SetSFXVolume(value);
    }
}