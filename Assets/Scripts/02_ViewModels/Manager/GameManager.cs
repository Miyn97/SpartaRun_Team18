﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public int MaxHp { get; private set; }

    public int CurrentHp { get; private set; } // 현재 체력
    public int CurrentStage { get; private set; }

    public bool IsInvincible { get; private set; }

    //StartUI 참조
    private StartUI startUI;
    //IntroUI 참조
    private IntroUI introUI;
    //GameUI 참조
    private GameUI gameUI;
    //GameOverUI 참조
    //private GameOverUI gameOverUI;
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private ItemSpawnController itemSpawnController;



    //public event System.Action OnRestartRequested;
    //public event System.Action OnReturnHomeRequested;

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
        MaxHp = 6; // 체력 리셋 (6으로 설정)
        CurrentHp = MaxHp;

        //게임이 시작되면 Intro 상태로 전환
        //ChangeState(GameState.Intro); // 최종본에서는 필요함
        //StartCoroutine(itemManager.SpawnRandomItem(15f));//시작할때 코루틴도 시작, 15초 후에 아이템 생성
        //StartCoroutine(itemManager.SpawnCoin(2));//2초마다 코인생성
    }

    private void OnEnable()
    {
        //씬이 로드될 때마다 자동으로 OnSceneLoaded()를 실행
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        //씬이 비활성화될 때 자동으로 OnSceneLoaded()를 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "03_IntroScene")
        {
            Time.timeScale = 1;
            //IntroUI 찾고 활성화
            introUI = FindObjectOfType<IntroUI>();
            if (introUI != null)
            {
                introUI.OnIntroComplete += HandleIntroComplete;
            }
        }

        if (scene.name == "01_StartScene")
        {
            Time.timeScale = 1;
            //StartUI 찾고 활성화
            startUI = FindObjectOfType<StartUI>();
            if (startUI != null)
            {
                startUI.OnStartRequested += HandleStartGame;
                startUI.OnOptionRequested += HandleOption;
                startUI.OnExitRequested += HandleExit;
            }
        }

        if (scene.name == "02_MainScene")
        {
            Time.timeScale = 1;
            gameUI = FindObjectOfType<GameUI>();
            itemSpawnController = FindObjectOfType<ItemSpawnController>();

            if (itemSpawnController != null)
            {
                StartCoroutine(itemSpawnController.SpawnCoinRoutine(1f));
                StartCoroutine(itemSpawnController.SpawnRandomItemRoutine(8f));
                //Debug.Log("[GameManager] 아이템 스폰 코루틴 시작됨");
            }
            else
            {
                Debug.LogError("[GameManager] ItemSpawnController를 찾을 수 없습니다.");
            }

            gameUI = FindObjectOfType<GameUI>();
            if (gameUI != null)
            {
                gameUI.OnPauseRequested += TogglePause;
                gameUI.OnReturnHomeRequested += ReturnToHome;

                UIManager.Instance.OnRestartRequested += RestartGame;
                UIManager.Instance.OnReturnHomeRequested += ReturnToHome;
                //gameUI.OnVolumeChanged += SoundManager.Instance.SetVolume; // (연결 되면 주석처리 해제하고 적용)
            }
        }
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
    public void TriggerGameOver(int score, int bestScore)
    {
        //게임 오버시 플레이 정지.
        Time.timeScale = 0;
        //아이템 생성 중단
        itemSpawnController?.StopSpawn();
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
        //옵션 UI를 띄우는 로직을 여기에 추가하면 됩니다.
        startUI.ShowOptionPanel();
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
    public void Heal(int amount)
    {
        CurrentHp = Mathf.Min(CurrentHp + amount, MaxHp); //체력 증가 (최대 6)
        UIManager.Instance.UpdateHealth(CurrentHp); //UIManager에 체력 업데이트 요청
    }


    // 체력감소 테스트용 _ryang
    public void TakeDamage(int damage)
    {
        if (IsInvincible == true)
        {
            //Debug.Log("무적임");
            return;
        }
        else
        {
            CurrentHp = Mathf.Max(CurrentHp - damage, 0); //체력 감소 (최소 0)
            UIManager.Instance.UpdateHealth(CurrentHp); //UIManager에 체력 업데이트 요청

            if (CurrentHp <= 0)
            {
                //TriggerGameOver(Score, BestScore); //여기서 GameOver 처리 통합
            }
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
        // 재시작시 시간 다시 흐르게
        Time.timeScale = 1;
        Score = 0;
        SetStage(1); //스테이지 초기화 예시
                     //SceneManager.LoadScene("MainScene"); //MainScene을 로드
        ChangeState(GameState.InGame); //상태를 InGame으로 변경
    }

    //홈으로 돌아가기 버튼이 눌렸을 때 실행
    public void ReturnToHome()
    {
        Time.timeScale = 1; // 시간 흐르게
        isPaused = false; // 일시정지 해제

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
    public void SetInvincible(bool value)
    {
        IsInvincible = value;
    }

}
