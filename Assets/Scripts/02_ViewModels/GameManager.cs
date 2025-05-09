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
    public int HighScore { get; private set; }
    public int CurrentStage { get; private set; }

    private int currentHp;
    private int maxHp = 6;

    private GameUI gameUI;
    private GameOverUI gameOverUI;


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
        // 최고점 불러오기
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        //게임이 시작되면 Intro 상태로 전환
        ChangeState(GameState.Intro);
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
                //**UI.cs 찾고 초기화
                gameUI = FindObjectOfType<GameUI>();
                gameOverUI = FindObjectOfType<GameOverUI>();
                //초기화
                Score = 0;
                currentHp = maxHp;
                gameUI.UpdateScore(Score);
                gameUI.UpdateHpText(currentHp);
                break;

            case GameState.GameOver:
                //나중에 UIManager랑 연결 (이 라인 삭제가능)
                //씬 전환X, UI만 표시
                gameOverUI.ShowGameOverUI(Score, HighScore);
                break;
        }
    }

    //플레이어가 아이템(코인)을 먹거나 할 때 점수 증가
    public void AddScore(int value)
    {
        Score += value;
        if (Score > HighScore)
        {
            HighScore = Score; // 최고점으로 갱신
            PlayerPrefs.SetInt("HighScore", HighScore);
        }
        
        gameUI.UpdateScore(Score);
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage; //체력 감소
        gameUI.UpdateHpText(currentHp); //UI 업데이트

        if (currentHp <= 0)
        {
            currentHp = 0;
            ChangeState(GameState.GameOver); //게임오버 상태로 전환
        }
    }

    public void CurrentHp(int hp)
    {
        currentHp = hp; //체력 초기화
        //gameUI.UpdateHpText(currentHp, maxHp); //UI 업데이트
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
