using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class GameUI : MonoBehaviour
{
    // 게임 플레이중 보여지는 UI
    private GameManager gameManager;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI currentScoreText;

    [Header("Hp: 3 Hear / 6 Hp")]
    [SerializeField] private Image[] hpImages; // 3개로 나누어서 보여줄 것임. 1개는 2칸으로 나누어서 보여줄 것임.

    [SerializeField] private Sprite heart_full; // heartState = 0
    [SerializeField] private Sprite heart_half; // 1
    [SerializeField] private Sprite heart_empty; // 2

    // 단순 게임 일시정지기능만 or 옵션메뉴 판넬로 해도 괜찮음, 시작-> 옵션 과 동일한 곳으로 이동하는 방식도 괜찮음
    [Header("Pause/Option")]
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button pauseButton;

    private void Awake()
    {
        optionPanel.SetActive(false); // 옵션패널 처음부터 보여주지 않음.
    }


    public void UpdateScore(int currentScore)
    {
        // 진행 점수 문자열로 바꿔서 화면에 표시
        currentScoreText.text = currentScore.ToString();
    }

    public void UpdateHpText(int currentHp)
    {
        // 데미지 받은 직후, 체력 UI 업데이트
        for (int i = 0; i < hpImages.Length; i++)
        {
            // Mathf.Clamp(value, min, max) : value가 min보다 작으면 min을, max보다 크면 max를 반환
            int heartIndex = Mathf.Clamp(currentHp - i * 2, 0, 2);
            Sprite sprite;

            if (heartIndex == 0)
            {
                sprite = heart_full; // 하트 한개당 체력 2
            }
            else if (heartIndex == 1)
            {
                sprite = heart_half; // 개당 체력 1
            }
            else
            {
                sprite = heart_empty; // 개당 체력 0
            }

            hpImages[i].sprite = sprite; // hp UI에 스프라이트(하트이미지) 적용
        }
    }

    // 일시 정지 기능 - GameManager에서 호출
    public void OnClickPauseButton()
    {
        bool isPause = Time.timeScale == 0;
        Time.timeScale = isPause ? 1 : 0; // isPause 가 true면 일시정지, false면 게임 재개
        optionPanel.SetActive(isPause); // 일시정지 상태일 때만 옵션창 UI 활성화
    }
}
