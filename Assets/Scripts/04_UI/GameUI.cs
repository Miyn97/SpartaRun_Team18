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
        // TODO: ���ӸŴ������� ��������, �ְ������� ���ڿ��� �ٲ㼭 ȭ�鿡 ǥ��
        currentScoreText.text = currentScore.ToString();
        highScoreText.text = highScore.ToString();
    }

    
}
