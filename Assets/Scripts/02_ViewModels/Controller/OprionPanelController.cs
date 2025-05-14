using UnityEngine;
using UnityEngine.UI;

public class OptionPanelController : MonoBehaviour
{
    [Header("Slider References")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // �ʱ�ȭ: SoundManager�� ���� �������� �����̴� �� ����
        if (SoundManager.Instance != null)
        {
            bgmSlider.value = SoundManager.Instance.bgmVolume;
            sfxSlider.value = SoundManager.Instance.sfxVolume;
        }

        // �����̴� �̺�Ʈ ���
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnBGMVolumeChanged(float value)  // ������� ���� �����̴�
    {
        SoundManager.Instance?.SetBGMVolume(value);
    }

    private void OnSFXVolumeChanged(float value) // ȿ���� ���� �����̴�
    {
        SoundManager.Instance?.SetSFXVolume(value);
    }
}