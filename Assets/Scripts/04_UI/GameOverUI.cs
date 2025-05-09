using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Button")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;

    public event System.Action OnRestartRequested;
    public event System.Action OnReturnHomeRequested;

    private void Awake()
    {
        gameObject.SetActive(false);

        restartButton.onClick.AddListener(() => OnRestartRequested?.Invoke());
        homeButton.onClick.AddListener(() => OnReturnHomeRequested?.Invoke());
    }

    public void Show(int finalScore, int highScore)
    {
        finalScoreText.text = finalScore.ToString();
        highScoreText.text = highScore.ToString();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    //// ���ӿ��� UI
    //[Header("Game Over")]
    //[SerializeField] private GameObject gameOverUI;
    //[SerializeField] private TextMeshProUGUI finalScoreText;
    //[SerializeField] private TextMeshProUGUI highScoreText;

    //// ��ư
    //[Header("Button")]
    //[SerializeField] private Button restartButton;
    //[SerializeField] private Button homeButton; // �κ�� ���ư��� ��ư �̸� �ٲ㵵 ����

    //private void Awake()
    //{
    //    gameObject.SetActive(false); // ���۽ÿ��� ���ӿ��� UI ��Ȱ��ȭ
    //}

    //public void ShowGameOverUI(int finalScore, int highScore)
    //{
    //    // 0 �ؽ�Ʈ ���ڿ��� �ٲ㼭 ȭ�鿡 ǥ��
    //    finalScoreText.text = finalScore.ToString();
    //    highScoreText.text = highScore.ToString();

    //    // ���ӿ��� UI Ȱ��ȭ
    //    gameOverUI.SetActive(true);
    //}
}
