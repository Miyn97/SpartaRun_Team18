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
    //������ ���� �������� ������ �����ϴ� ����
    public int Score { get; private set; }
    public int CurrentStage { get; private set; }

    //�ν����Ϳ��� ���� ������ GameUI ����
    [SerializeField] private GameUI gameUI;

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
        gameUI.OnPauseRequested += TogglePause;
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

    //���� ���¸� ��ȯ�ϴ� �޼��� (ȣ�� ����)
    public void ChangeState(GameState newState)
    {
        //���°� ����
        CurrentState = newState;

        //���¿� ���� �ٸ� �� �ε�
        switch (newState)
        {
            case GameState.Intro:
                //using UnityEngine.SceneManagement; ��¡���� �߰��ϸ� ȣ�� ����
                SceneManager.LoadScene("IntroScene");
                break;
            case GameState.Start:
                SceneManager.LoadScene("StartScene");
                break;
            case GameState.InGame:
                SceneManager.LoadScene("MainScene");
                break;
            case GameState.GameOver:
                //���߿� UIManager�� ���� (�� ���� ��������)
                //�� ��ȯX, UI�� ǥ��
                //UIManager.Instance?.ShowGameOverUI(); //(�ּ� ó�� ���� ����)
                break;
        }
    }

    //�÷��̾ ������(����)�� �԰ų� �� �� ���� ����
    public void AddScore(int value)
    {
        Score += value;
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
        SceneManager.LoadScene("MainScene"); //MainScene�� �ε�
        ChangeState(GameState.InGame); //���¸� InGame���� ����
    }

    //���� ����
    public void QuitGame()
    {
        Application.Quit();
    }


}
