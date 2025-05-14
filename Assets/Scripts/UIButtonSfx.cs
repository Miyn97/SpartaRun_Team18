using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSfx : MonoBehaviour
{
    public AudioClip clickClip;
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => SoundManager.Instance.PlaySFX(clickClip));
    }
}
