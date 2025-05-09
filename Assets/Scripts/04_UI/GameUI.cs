using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("CurrentScore")]
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;

    [Header("Hp")]
    public TextMeshProUGUI hpText;

    public void UpdateScoreText(int currentScore, int highScore)
    {
        // TODO: 게임매니져에서 현재점수, 최고점수를 문자열로 바꿔서 화면에 표시
        currentScoreText.text = currentScore.ToString();
        highScoreText.text = highScore.ToString();
    }

    
}
