using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    //인스펙터 상에서 Core라는 섹션으로 구분
    [Header("Score")]
    //이번 플레이의 최종 점수를 출력할 텍스트
    [SerializeField] private TextMeshProUGUI finalScoreText;
    //저장된 최고 점수를 출력할 텍스트
    [SerializeField] private TextMeshProUGUI highScoreText;

    //인스펙터에서 Button이라는 그룹으로 묶어서 표시
    [Header("Button")]
    //게임을 다시 시작할 때 누르는 버튼
    [SerializeField] private Button restartButton;
    //메인 화면 또는 로비로 돌아가기 위한 버튼
    [SerializeField] private Button homeButton;

    //외부에 버튼 클릭을 알리는 이벤트 선언
    //이 View는 직접 상태를 바꾸지 않고, 버튼이 눌렸다는 요청만 이벤트로 알려줌
    public event System.Action OnRestartRequested;
    public event System.Action OnReturnHomeRequested;

    private void Awake()
    {
        //시작 시 비활성화 상태로 숨겨져 있어야 하므로 꺼둠
        gameObject.SetActive(false);

        //다시 시작 버튼을 클릭하면 OnRestartRequested 이벤트를 호출
        //이 이벤트에 연결된 구독자가 있을 때만 안전하게 호출
        restartButton.onClick.AddListener(() => OnRestartRequested?.Invoke());
        //홈으로 버튼 클릭시 OnReturnHomeRequested 이벤트를 호출함
        //외부에 "홈으로 돌아가야 해요!"라고 알릴 뿐, 직접 행동은 하지 않음
        homeButton.onClick.AddListener(() => OnReturnHomeRequested?.Invoke());
    }

    //게임 오버가 발생하면 외부가 이 메서드를 호출
    //점수를 전달하고 UI를 보여주게 됨
    public void Show(int finalScore, int highScore)
    {
        //전달받은 점수들을 텍스트로 변환해서 UI에 표시함
        finalScoreText.text = finalScore.ToString();
        highScoreText.text = highScore.ToString();

        //이 GameOverUI 오브젝트를 활성화해서 실제 화면에 나타나게 함
        gameObject.SetActive(true);
    }

    //게임을 다시 시작하거나,
    //홈으로 돌아갈 때 UI를 숨기기 위한 함수
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
