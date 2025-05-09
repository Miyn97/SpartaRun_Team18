using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class GameUI : MonoBehaviour
{
    // ���� �÷����� �������� UI
    private GameManager gameManager;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI currentScoreText;

    [Header("Hp: 3 Hear / 6 Hp")]
    [SerializeField] private Image[] hpImages; // 3���� ����� ������ ����. 1���� 2ĭ���� ����� ������ ����.

    [SerializeField] private Sprite heart_full; // heartState = 0
    [SerializeField] private Sprite heart_half; // 1
    [SerializeField] private Sprite heart_empty; // 2

    // �ܼ� ���� �Ͻ�������ɸ� or �ɼǸ޴� �ǳڷ� �ص� ������, ����-> �ɼ� �� ������ ������ �̵��ϴ� ��ĵ� ������
    [Header("Pause/Option")]
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button pauseButton;

    private void Awake()
    {
        optionPanel.SetActive(false); // �ɼ��г� ó������ �������� ����.
    }


    public void UpdateScore(int currentScore)
    {
        // ���� ���� ���ڿ��� �ٲ㼭 ȭ�鿡 ǥ��
        currentScoreText.text = currentScore.ToString();
    }

    public void UpdateHpText(int currentHp)
    {
        // ������ ���� ����, ü�� UI ������Ʈ
        for (int i = 0; i < hpImages.Length; i++)
        {
            // Mathf.Clamp(value, min, max) : value�� min���� ������ min��, max���� ũ�� max�� ��ȯ
            int heartIndex = Mathf.Clamp(currentHp - i * 2, 0, 2);
            Sprite sprite;

            if (heartIndex == 0)
            {
                sprite = heart_full; // ��Ʈ �Ѱ��� ü�� 2
            }
            else if (heartIndex == 1)
            {
                sprite = heart_half; // ���� ü�� 1
            }
            else
            {
                sprite = heart_empty; // ���� ü�� 0
            }

            hpImages[i].sprite = sprite; // hp UI�� ��������Ʈ(��Ʈ�̹���) ����
        }
    }

    // �Ͻ� ���� ��� - GameManager���� ȣ��
    public void OnClickPauseButton()
    {
        bool isPause = Time.timeScale == 0;
        Time.timeScale = isPause ? 1 : 0; // isPause �� true�� �Ͻ�����, false�� ���� �簳
        optionPanel.SetActive(isPause); // �Ͻ����� ������ ���� �ɼ�â UI Ȱ��ȭ
    }
}
