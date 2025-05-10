using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    //Score 라벨로 그룹 만들어줌
    [Header("Score")]
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

    [Header("Pause/Option")]
    //일시정지 메뉴창과 정지 버튼을 연결할 변수
    // 단순 게임 일시정지기능만 or 옵션메뉴 판넬로 해도 괜찮음,
    // 시작-> 옵션 과 동일한 곳으로 이동하는 방식도 괜찮음
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button pauseButton;

    //delegate는 이벤트를 외부에 전달할 수 있는 형식
    public delegate void PauseRequestedHandler();
    //OnpauseRequested는 정지 버튼이 눌렸다 라는 사실만 외부 ViewModel에게 알림
    public event PauseRequestedHandler OnPauseRequested;

    private void Awake()
    {
        // 옵션패널 처음부터 보여주지 않음.
        optionPanel.SetActive(false);
        //정지 버튼을 클릭하면 NotifyPauseRequested() 함수가 호출되도록 연결
        pauseButton.onClick.AddListener(NotifyPauseRequested);
    }

    public void SetScore(int score)
    {
        // 진행 점수 문자열로 바꿔서 화면에 표시
        currentScoreText.text = score.ToString();
    }

    public void SetHealth(int currentHp)
    {
        // 데미지 받은 직후, 체력 UI 업데이트
        for (int i = 0; i < hpImages.Length; i++)
        {
            //하나의 하트가 2체력 담당하니까
            //i번째의 하트가 현재 남은 체력을 얼마나 표현해야 하는지 계산
            int heartIndex = Mathf.Clamp(currentHp - i * 2, 0, 2);
            Sprite sprite = heartIndex switch
            {
                0 => heart_full,
                1 => heart_half,
                _ => heart_empty
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
