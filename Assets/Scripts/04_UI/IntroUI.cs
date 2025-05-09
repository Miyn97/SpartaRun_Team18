using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    // Start UI�� ���� ��ư�� ����.

    [Header("Intro UI")]
    [SerializeField] private GameObject introCanvas;
    [SerializeField] private Button skipButton;

    public void SkipIntroButton()
    {
        // ��Ʈ�� UI ��Ȱ��ȭ
        introCanvas.SetActive(false);
        // StartUI Ȱ��ȭ
        GameManager.Instance.ChangeState(GameManager.GameState.Start);
    }
}
