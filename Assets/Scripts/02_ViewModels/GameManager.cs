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
    //점수와 현재 스테이지 정보를 저장하는 변수
    public int Score { get; private set; }
    public int CurrentStage { get; private set; }

    //인스펙터에서 연결 가능한 GameUI 참조
    [SerializeField] private GameUI gameUI;

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
        //게임이 시작되면 Intro 상태로 전환
        ChangeState(GameState.Intro);
        gameUI.OnPauseRequested += TogglePause;
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

    //게임 상태를 전환하는 메서드 (호출 가능)
    public void ChangeState(GameState newState)
    {
        //상태값 갱신
        CurrentState = newState;

        //상태에 따라서 다른 씬 로드
        switch (newState)
        {
            case GameState.Intro:
                //using UnityEngine.SceneManagement; 유징문만 추가하면 호출 가능
                SceneManager.LoadScene("IntroScene");
                break;
            case GameState.Start:
                SceneManager.LoadScene("StartScene");
                break;
            case GameState.InGame:
                SceneManager.LoadScene("MainScene");
                break;
            case GameState.GameOver:
                //나중에 UIManager랑 연결 (이 라인 삭제가능)
                //씬 전환X, UI만 표시
                //UIManager.Instance?.ShowGameOverUI(); //(주석 처리 해제 가능)
                break;
        }
    }

    //플레이어가 아이템(코인)을 먹거나 할 때 점수 증가
    public void AddScore(int value)
    {
        Score += value;
        //나중에 UIManager랑 연결 (이 라인 삭제가능)
        //점수 증가 후, UIManager에 현재 점수 업데이트 요청
        //UIManager.Instance?.UpdateScore(Score); //(주석 처리 해제 가능)
    }

    //난이도 증가나 다음 맵 전환에 사용
    public void SetStage(int stage)
    {
        CurrentStage = stage;
    }

    //게임 재시작 관련 메서드
    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene"); //MainScene을 로드
        ChangeState(GameState.InGame); //상태를 InGame으로 변경
    }

    //게임 종료
    public void QuitGame()
    {
        Application.Quit();
    }


}
