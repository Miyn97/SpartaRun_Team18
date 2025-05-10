using UnityEngine;

public class UIManager : MonoBehaviour
{
    //싱글톤 생성 + UIManager.Instance 로 접근가능
    public static UIManager Instance { get; private set; }

    //인스펙터에서 UI 요소들을 구분해주는 시각적 라벨
    //보기 쉽게 만들어주는 역할.
    [Header("UI Screens")]

    //각 UI를 활성화/비활성화하거나 데이터 전달할 때 사용
    private IntroUI introUI;
    private StartUI startUI;
    private GameUI gameUI;
    private GameOverUI gameOverUI; // [SerializeField] 지움_ryang

    private void Awake()
    {
        //UIManager 2개 방지 + 싱글톤 원칙
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //UIManager를 인스턴스로 등록
        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환되어도 사라지지 않도록 설정 (게임 흐름 유지) _ryang
    }

    // 씬 이름에 따라 UI를 찾아서 필요한 것만 찾아서 연결_ryang
    private void SceneLoad()
    {
        //씬 이름에 따라 UI를 찾아서 연결
        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            case "03_IntroScene":
                introUI = FindObjectOfType<IntroUI>();
                introUI?.gameObject.SetActive(true); //IntroUI가 null이 아닐 때만 활성화
                break;
            case "01_StartScene":
                startUI = FindObjectOfType<StartUI>();
                startUI?.gameObject.SetActive(true); //StartUI가 null이 아닐 때만 활성화
                break;
            case "02_MainScene":
                gameUI = FindObjectOfType<GameUI>();
                gameOverUI = FindObjectOfType<GameOverUI>();
                gameUI.gameObject.SetActive(true);
                gameOverUI.gameObject.SetActive(false); //GameUI는 활성화, GameOverUI는 비활성화
                break;
        }
    }

    //Intro 화면을 보여주는 함수
    public void ShowIntroUI()
    {
        //다른 UI는 전부 꺼지고 introUI만 켜지게 함
        SetOnlyActive(introUI);
    }

    //Start 화면을 보여주는 함수
    public void ShowStartUI()
    {
        //시작 화면(UI)만 활성화.
        SetOnlyActive(startUI);
    }

    //GameUI 화면을 보여주는 함수
    public void ShowGameUI()
    {
        //게임 중 점수나 체력 등을 보여주는 GameUI만 활성화
        SetOnlyActive(gameUI);
    }

    //GameOverUI 화면을 보여주는 함수
    public void ShowGameOverUI(int finalScore, int highScore)
    {
        //게임 오버 상황에서만 보여주는 UI를 켬
        gameOverUI.Show(finalScore, highScore);
    }

    //GameUI의 점수 표시 갱신을 요청
    public void UpdateScore(int score)
    {
        //gameUI가 null이 아닐 때만 실행됨
        gameUI?.SetScore(score);
    }

    //체력 UI를 갱신하는 함수
    public void UpdateHealth(int health)
    {
        //역시 GameUI에 값을 넘겨줄 뿐, UIManager는 직접 표시하지 않음
        gameUI?.SetHealth(health);
    }

    //전달된 하나의 UI만 켜고, 나머지는 전부 끄는 공통 전환 함수
    private void SetOnlyActive(MonoBehaviour targetUI)
    {
        introUI?.gameObject.SetActive(false);
        startUI?.gameObject.SetActive(false);
        gameUI?.gameObject.SetActive(false);
        gameOverUI?.gameObject.SetActive(false);

        if (targetUI != null)
            targetUI.gameObject.SetActive(true);
    }
}
