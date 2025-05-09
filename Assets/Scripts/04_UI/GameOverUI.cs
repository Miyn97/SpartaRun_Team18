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

    //// 게임오버 UI
    //[Header("Game Over")]
    //[SerializeField] private GameObject gameOverUI;
    //[SerializeField] private TextMeshProUGUI finalScoreText;
    //[SerializeField] private TextMeshProUGUI highScoreText;

    //// 버튼
    //[Header("Button")]
    //[SerializeField] private Button restartButton;
    //[SerializeField] private Button homeButton; // 로비로 돌아가는 버튼 이름 바꿔도 좋음

    //private void Awake()
    //{
    //    gameObject.SetActive(false); // 시작시에는 게임오버 UI 비활성화
    //}

    //public void ShowGameOverUI(int finalScore, int highScore)
    //{
    //    // 0 텍스트 문자열로 바꿔서 화면에 표시
    //    finalScoreText.text = finalScore.ToString();
    //    highScoreText.text = highScore.ToString();

    //    // 게임오버 UI 활성화
    //    gameOverUI.SetActive(true);
    //}
}
