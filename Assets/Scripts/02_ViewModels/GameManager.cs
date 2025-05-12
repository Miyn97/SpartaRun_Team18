using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //전역에서 한번만 생성 + GameManager.Instance 로 쉽게 접근가능
    public static GameManager Instance { get; private set; }
    //게임의 현재 상태
    public enum GameState { Intro, Start, InGame, GameOver }
    //현재 상태 저장 변수
    public GameState CurrentState { get; private set; }
    //점수, 체력과 현재 스테이지 정보를 저장하는 변수
    public int Score { get; private set; }
    public int BestScore { get; private set; } // 최고 점수
    public int CurrentHp { get; private set; } // 현재 체력
    public int CurrentStage { get; private set; }

    //StartUI 참조
    [SerializeField] private StartUI startUI;
    //IntroUI 참조
    [SerializeField] private IntroUI introUI;
    //GameUI 참조
    [SerializeField] private GameUI gameUI;
    //GameOverUI 참조
    [SerializeField] private GameOverUI gameOverUI;

    //현재 게임이 일시정지 상태인지 나타내는 변수.
    //상태를 토글할 때 사용됨
    private bool isPaused = false;

    private void Awake()
    {
        //GameManager 2개 방지 + 싱글톤 원칙
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //GameManager를 전역 인스턴스로 등록
        Instance = this;
        //씬 전환되어도 이 오브젝트는 사라지지 않도록 설정 (게임 흐름 유지)
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        BestScore = PlayerPrefs.GetInt("BestScore", 0); // 최고 점수 불러오기
        Score = 0; // 점수 리셋
        CurrentHp = 6; // 체력 리셋 (6으로 설정)

        //게임이 시작되면 Intro 상태로 전환
        //ChangeState(GameState.Intro); // 최종본에서는 필요함

        //GameUI가 보낸 이벤트를 구독
        gameUI.OnPauseRequested += TogglePause;
        //GameOverUI가 보낸 이벤트를 구독
        //UI는 버튼만 누르고 실제 로직은 GameManager가 처리
        gameOverUI.OnRestartRequested += RestartGame;
        gameOverUI.OnReturnHomeRequested += ReturnToHome;

        //StartUI 이벤트 구독
        startUI.OnStartRequested += HandleStartGame;
        startUI.OnOptionRequested += HandleOption;
        startUI.OnExitRequested += HandleExit;

        //IntroUI 이벤트 구독
        introUI.OnIntroComplete += HandleIntroComplete;
    }

    //실제 일시정지를 수행하는 메서드
    private void TogglePause()
    {
        //현재 일시정지 상태를 반전시킴
        isPaused = !isPaused;
        //Unity의 시간 속도를 조절하는 변수
        //0으로 설정하면 모든 Update나 FixedUpdate 실행이 멈춤
        Time.timeScale = isPaused ? 0 : 1;
        //옵션창 UI를 켜거나 끄는 명령
        //View에게 명령만 내림
        gameUI.ToggleOptionPanel(isPaused);
    }

    //게임 오버 상황을 호출할 때 외부에서 사용할 수 있는 함수
    public void TriggerGameOver(int score, int highScore)
    {
        //점수와 최고 점수를 전달 + GameOverUI를 보여줌
        gameOverUI.Show(score, highScore);
        //상태를 GameOver로 전환
        ChangeState(GameState.GameOver);
    }

    //IntroUI에서 인트로(스토리)가 끝났을 때 호출되는 콜백 함수
    //트리거되면 실행됨
    private void HandleIntroComplete()
    {
        //현재 게임 상태를 Start로 전환
        //즉, StartScene 또는 시작화면 UI로 넘어가는 역할을 수행
        ChangeState(GameState.Start);
    }

    //StartUI에서 게임 시작 버튼을 누르면 이 함수가 호출
    private void HandleStartGame()
    {
        //게임 상태를 InGame로 변경
        //일반적으로 MainScene을 로드하고, 실제 게임 플레이를 시작하는 상태로 전환
        ChangeState(GameState.InGame);
    }

    //StartUI에서 옵션 버튼을 누르면 이 함수가 호출
    private void HandleOption()
    {
        Debug.Log("옵션 버튼 눌림 - 옵션 UI 처리 예정");
        //추후 옵션 UI.Show() 등 달아주시면 됩니다 선량님
    }

    //게임 종료 버튼을 눌렸을 때 실행되는 함수
    private void HandleExit()
    {
        //실행된 빌드 상태에서 게임을 종료
        //Windows, Mac, Android 등에서 종료되며 에디터에서는 무시됨
        Application.Quit();

        //이 코드는 Unity 에디터에서 실행 중일 때만 작동하도록 조건부 컴파일 지시자입니다
        //에디터에서도 종료 버튼을 누르면 Play모드가 꺼지도록 해줌
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //게임 상태를 전환하는 메서드 (호출 가능)
    public void ChangeState(GameState newState)
    {
        //상태값 갱신
        CurrentState = newState;

        //상태에 따라서 다른 씬 로드
        switch (newState)
        {
            case GameState.Intro:
                SceneManager.LoadScene("03_IntroScene");
                break;
            case GameState.Start:
                SceneManager.LoadScene("01_StartScene");
                break;
            case GameState.InGame:
                SceneManager.LoadScene("02_MainScene");
                // 새 게임 시작시 체력, 점수 다시 초기화
                Score = 0; // 점수 리셋
                CurrentHp = 6; // 체력 리셋 (6으로 설정)
                UIManager.Instance.UpdateHealth(CurrentHp); // UIManager에 체력 업데이트 요청
                UIManager.Instance.UpdateScore(Score, BestScore); // UIManager에 점수 업데이트 요청
                break;
            case GameState.GameOver:
                //씬 전환X, UI만 표시
                UIManager.Instance.ShowGameOverUI(Score, BestScore);
                break;
        }
    }

    //플레이어가 아이템(코인)을 먹거나 할 때 점수 증가
    public void AddScore(int value)
    {
        Score += value;
        if (Score > BestScore)
        {
            BestScore = Score; //최고 점수 갱신
            PlayerPrefs.SetInt("BestScore", BestScore); //최고 점수 저장
        }
        //점수 증가 후, UIManager에 현재 점수 업데이트 요청
        UIManager.Instance?.UpdateScore(Score, BestScore);
    }

    // 체력감소 테스트용 _ryang
    public void TakeDamage(int damage)
    {
        CurrentHp = Mathf.Max(CurrentHp - damage, 0); //체력 감소 (최소 0)
        UIManager.Instance.UpdateHealth(CurrentHp); //UIManager에 체력 업데이트 요청

        if (CurrentHp <= 0)
        {
            ChangeState(GameState.GameOver);
        }
    }

    //난이도 증가나 다음 맵 전환에 사용
    public void SetStage(int stage)
    {
        CurrentStage = stage;
    }

    //게임 재시작 관련 메서드
    public void RestartGame()
    {
        Score = 0;
        SetStage(1); //스테이지 초기화 예시
        //SceneManager.LoadScene("MainScene"); //MainScene을 로드
        ChangeState(GameState.InGame); //상태를 InGame으로 변경
    }

    //홈으로 돌아가기 버튼이 눌렸을 때 실행
    private void ReturnToHome()
    {
        //상태 초기화
        Score = 0;
        SetStage(0);
        //Start 씬으로 이동
        ChangeState(GameState.Start);
    }

    ////게임 종료
    //public void QuitGame()
    //{
    //    Application.Quit();
    //}


}
