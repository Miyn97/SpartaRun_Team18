using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    // ���ӿ��� UI
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Button")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton; // �κ�� ���ư��� ��ư �̸� �ٲ㵵 ����

    private void Awake()
    {
        gameObject.SetActive(false); // ���۽ÿ��� ���ӿ��� UI ��Ȱ��ȭ
    }

    public void ShowGameOverUI(int finalScore, int highScore)
    {
        // 0 �ؽ�Ʈ ���ڿ��� �ٲ㼭 ȭ�鿡 ǥ��
        finalScoreText.text = finalScore.ToString();
        highScoreText.text = highScore.ToString();
        
        // ���ӿ��� UI Ȱ��ȭ
        gameOverUI.SetActive(true);
    }
}
