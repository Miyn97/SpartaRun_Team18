using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    // Start UI로 가는 버튼만 구현.

    [Header("Intro UI")]
    [SerializeField] private GameObject introCanvas;
    [SerializeField] private Button skipButton;

    public void SkipIntroButton()
    {
        // 인트로 UI 비활성화
        introCanvas.SetActive(false);
        // StartUI 활성화
        GameManager.Instance.ChangeState(GameManager.GameState.Start);
    }
}
