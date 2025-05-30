using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    //Score 라벨로 그룹 만들어줌
    [Header("Score")]
    [SerializeField] private GameObject scoreHp_Panel;
    //textMeshProUGUI는 화면에 텍스트를 그리는 TMP용 컴포넌트
    [SerializeField] private TextMeshProUGUI currentScoreText;

    [Header("Hp: 3 Heart / 6 Hp")]
    //하트 3개 (2 HP씩 총6)로 체력을 보여줄 배열
    // 3개로 나누어서 보여줄 것임. 1개는 2칸으로 나누어서 보여줄 것임.
    [SerializeField] private Image[] hpImages;
    //하트가 풀 HP일 때
    [SerializeField] private Sprite heart_full;
    //하트가 1 HP일 때
    [SerializeField] private Sprite heart_half;
    //하트가 0 HP일 때
    [SerializeField] private Sprite heart_empty;

    [Header("Option")]
    //설정창 (일시정지 됨)
    [SerializeField] private Button optionButton;
    [SerializeField] private GameObject optionPanel;

    [Header("PanelButtons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button homeButton;

    //[Header("Audio")]
    //[SerializeField] private Slider volumeSlider; // 음량 슬라이더(연결 되면 주석처리 해제하고 적용)

    //delegate는 이벤트를 외부에 전달할 수 있는 형식
    public delegate void PauseRequestedHandler();
    //OnpauseRequested는 정지 버튼이 눌렸다 라는 사실만 외부 ViewModel에게 알림
    public event PauseRequestedHandler OnPauseRequested;
    //StartScene으로 돌아가고 싶다 라는 요청을 외부에 알림
    public event System.Action OnReturnHomeRequested;

    //AudioManager에게 볼륨을 조절하라고 요청
    // 슬라이더 값이 바뀔 때마다 해당 값 호출 => 나중에 SoundManager에서 해당 값 받아서 볼륨 변경되도록 구현해야함.
    //public event System.Action<float> OnVolumeChanged; // (연결 되면 주석처리 해제하고 적용)

    private void Awake()
    {
        optionPanel.SetActive(false);

        //정지 버튼을 클릭하면 NotifyPauseRequested() 함수가 호출되도록 연결
        optionButton.onClick.AddListener(NotifyPauseRequested);
        //일시정지 버튼 클릭 시 일시정지 요청 이벤트 발생 (구독자에게 알림)
        OnPauseRequested += () => optionPanel.SetActive(true);

        //Resume버튼 : 옵션창 닫고, 일시정지 해제
        resumeButton.onClick.AddListener(() =>
        {
            optionPanel.SetActive(false);
            NotifyPauseRequested();
        });

        //home버튼 : StartScene으로 돌아가기
        homeButton.onClick.AddListener(() =>
        {
            //게임을 종료하고, 시작 화면으로 돌아가야함
            optionPanel.SetActive(false);
            OnReturnHomeRequested?.Invoke();
        });

        // 볼륨 슬라이더의 값이 바뀔 때마다 OnVolumeChanged 이벤트를 호출
        //volumeSlider.onValueChanged.AddListener(value => OnVolumeChanged?.Invoke(value));
    }

    // 옵션 버튼 제외한 GameUI 비활성화
    public void HideGameUI()
    {
        scoreHp_Panel.SetActive(false);
    }

    public void SetScore(int score, int bestScore)
    {
        // 진행 점수 문자열로 바꿔서 화면에 표시
        currentScoreText.text = score.ToString();
        // 게임중에는 최고 점수 표시 안함
    }

    public void SetHealth(int currentHp)
    {
        // 데미지 받은 직후, 체력 UI 업데이트
        for (int i = 0; i < hpImages.Length; i++)
        {
            //하나의 하트가 2체력 담당하니까
            //i번째의 하트가 현재 남은 체력을 얼마나 표현해야 하는지 계산
            int hpHeart = Mathf.Clamp(currentHp - i * 2, 0, 2);
            Sprite sprite = hpHeart switch
            {
                0 => heart_empty,
                1 => heart_half,
                _ => heart_full
            };
            // hp UI에 스프라이트(하트이미지) 적용
            hpImages[i].sprite = sprite;
        }

    }

    //외부에서 옵션창을 켜거나 끌 수 있도록 공개된 함수.
    public void ToggleOptionPanel(bool isActive)
    {
        //View는 UI만 보여주고, 실제 게임 일시정지는 GameManager가 처리
        optionPanel.SetActive(isActive);
    }

    //정지 버튼이 눌리면, 외부에 "일시정지 요청이 발생했다!" 라고 알림
    private void NotifyPauseRequested()
    {
        //직접 일시정지 하지 않고, 요청만 보내는 구조
        OnPauseRequested?.Invoke();
       
    }
}
