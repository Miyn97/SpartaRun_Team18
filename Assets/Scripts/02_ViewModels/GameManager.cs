using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //�������� �ѹ��� ���� + GameManager.Instance �� ���� ���ٰ���
    public static GameManager Instance { get; private set; }
    //������ ���� ����
    public enum GameState { Intro, Start, InGame, GameOver }
    //���� ���� ���� ����
    public GameState CurrentState { get; private set; }
    //����, ü�°� ���� �������� ������ �����ϴ� ����
    public int Score { get; private set; }
    public int BestScore { get; private set; } // �ְ� ����
    public int CurrentHp { get; private set; } // ���� ü��
    public int CurrentStage { get; private set; }

    //StartUI ����
    private StartUI startUI;
    //IntroUI ����
    private IntroUI introUI;
    //GameUI ����
    private GameUI gameUI;
    //GameOverUI ����
    private GameOverUI gameOverUI;

    //���� ������ �Ͻ����� �������� ��Ÿ���� ����.
    //���¸� ����� �� ����
    private bool isPaused = false;

    private void Awake()
    {
        //GameManager 2�� ���� + �̱��� ��Ģ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //GameManager�� ���� �ν��Ͻ��� ���
        Instance = this;
        //�� ��ȯ�Ǿ �� ������Ʈ�� ������� �ʵ��� ���� (���� �帧 ����)
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //������ ���۵Ǹ� Intro ���·� ��ȯ
        ChangeState(GameState.Intro);

        //GameUI�� ���� �̺�Ʈ�� ����
        gameUI.OnPauseRequested += TogglePause;
        //GameOverUI�� ���� �̺�Ʈ�� ����
        //UI�� ��ư�� ������ ���� ������ GameManager�� ó��
        gameOverUI.OnRestartRequested += RestartGame;
        gameOverUI.OnReturnHomeRequested += ReturnToHome;

        //StartUI �̺�Ʈ ����
        startUI.OnStartRequested += HandleStartGame;
        startUI.OnOptionRequested += HandleOption;
        startUI.OnExitRequested += HandleExit;

        //IntroUI �̺�Ʈ ����
        introUI.OnIntroComplete += HandleIntroComplete;
    }

    //���� �Ͻ������� �����ϴ� �޼���
    private void TogglePause()
    {
        //���� �Ͻ����� ���¸� ������Ŵ
        isPaused = !isPaused;
        //Unity�� �ð� �ӵ��� �����ϴ� ����
        //0���� �����ϸ� ��� Update�� FixedUpdate ������ ����
        Time.timeScale = isPaused ? 0 : 1;
        //�ɼ�â UI�� �Ѱų� ���� ���
        //View���� ��ɸ� ����
        gameUI.ToggleOptionPanel(isPaused);
    }

    //���� ���� ��Ȳ�� ȣ���� �� �ܺο��� ����� �� �ִ� �Լ�
    public void TriggerGameOver(int score, int highScore)
    {
        //������ �ְ� ������ ���� + GameOverUI�� ������
        gameOverUI.Show(score, highScore);
        //���¸� GameOver�� ��ȯ
        ChangeState(GameState.GameOver);
    }

    //IntroUI���� ��Ʈ��(���丮)�� ������ �� ȣ��Ǵ� �ݹ� �Լ�
    //Ʈ���ŵǸ� �����
    private void HandleIntroComplete()
    {
        //���� ���� ���¸� Start�� ��ȯ
        //��, StartScene �Ǵ� ����ȭ�� UI�� �Ѿ�� ������ ����
        ChangeState(GameState.Start);
    }

    //StartUI���� ���� ���� ��ư�� ������ �� �Լ��� ȣ��
    private void HandleStartGame()
    {
        //���� ���¸� InGame�� ����
        //�Ϲ������� MainScene�� �ε��ϰ�, ���� ���� �÷��̸� �����ϴ� ���·� ��ȯ
        ChangeState(GameState.InGame);
    }

    //StartUI���� �ɼ� ��ư�� ������ �� �Լ��� ȣ��
    private void HandleOption()
    {
        Debug.Log("�ɼ� ��ư ���� - �ɼ� UI ó�� ����");
        //���� �ɼ� UI.Show() �� �޾��ֽø� �˴ϴ� ������
    }

    //���� ���� ��ư�� ������ �� ����Ǵ� �Լ�
    private void HandleExit()
    {
        //����� ���� ���¿��� ������ ����
        //Windows, Mac, Android ��� ����Ǹ� �����Ϳ����� ���õ�
        Application.Quit();

        //�� �ڵ�� Unity �����Ϳ��� ���� ���� ���� �۵��ϵ��� ���Ǻ� ������ �������Դϴ�
        //�����Ϳ����� ���� ��ư�� ������ Play��尡 �������� ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //���� ���¸� ��ȯ�ϴ� �޼��� (ȣ�� ����)
    public void ChangeState(GameState newState)
    {
        //���°� ����
        CurrentState = newState;

        //���¿� ���� �ٸ� �� �ε�
        switch (newState)
        {
            case GameState.Intro:
                SceneManager.LoadScene("03_IntroScene");
                UIManager.Instance?.ShowIntroUI(); //IntroUI�� ������_ryang
                break;
            case GameState.Start:
                SceneManager.LoadScene("01_StartScene");
                UIManager.Instance?.ShowIntroUI(); //StartUI�� ������_ryang
                break;
            case GameState.InGame:
                SceneManager.LoadScene("02_MainScene");
                break;
            case GameState.GameOver:
                //�� ��ȯX, UI�� ǥ��
                UIManager.Instance.ShowGameOverUI(Score, BestScore);
                break;
        }
    }

    //�÷��̾ ������(����)�� �԰ų� �� �� ���� ����
    public void AddScore(int value)
    {
        Score += value;
        gameUI.SetScore(Score);
        //���߿� UIManager�� ���� (�� ���� ��������)
        //���� ���� ��, UIManager�� ���� ���� ������Ʈ ��û
        //UIManager.Instance?.UpdateScore(Score); //(�ּ� ó�� ���� ����)
    }

    //���̵� ������ ���� �� ��ȯ�� ���
    public void SetStage(int stage)
    {
        CurrentStage = stage;
    }

    //���� ����� ���� �޼���
    public void RestartGame()
    {
        Score = 0;
        SetStage(1); //�������� �ʱ�ȭ ����
        //SceneManager.LoadScene("MainScene"); //MainScene�� �ε�
        ChangeState(GameState.InGame); //���¸� InGame���� ����
    }

    //Ȩ���� ���ư��� ��ư�� ������ �� ����
    private void ReturnToHome()
    {
        //���� �ʱ�ȭ
        Score = 0;
        SetStage(0);
        //Start ������ �̵�
        ChangeState(GameState.Start);
    }

    // ü�°��� �׽�Ʈ�� _ryang
    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        gameUI.SetHealth(CurrentHp); // ChangeState()���� GameOverUI ����

        if (CurrentHp <= 0)
        {
            ChangeState(GameState.GameOver);
        }
    }

    ////���� ����
    //public void QuitGame()
    //{
    //    Application.Quit();
    //}


}
