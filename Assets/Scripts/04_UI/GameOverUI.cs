using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    // 게임오버 UI
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button restartButton;
    
    public void ShowGameOverUI(int currentScore, int highScore)
    {
        // 게임오버 UI 활성화
        gameOverUI.SetActive(true);
        finalScoreText.text = currentScore.ToString();
        highScoreText.text = highScore.ToString();
    }
}
