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
    public int HighScore { get; private set; }
    public int CurrentStage { get; private set; }

    private int currentHp;
    private int maxHp = 6;

    private GameUI gameUI;
    private GameOverUI gameOverUI;


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
        // �ְ��� �ҷ�����
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        //������ ���۵Ǹ� Intro ���·� ��ȯ
        ChangeState(GameState.Intro);
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
                //**UI.cs ã�� �ʱ�ȭ
                gameUI = FindObjectOfType<GameUI>();
                gameOverUI = FindObjectOfType<GameOverUI>();
                //�ʱ�ȭ
                Score = 0;
                currentHp = maxHp;
                gameUI.UpdateScore(Score);
                gameUI.UpdateHpText(currentHp);
                break;

            case GameState.GameOver:
                //���߿� UIManager�� ���� (�� ���� ��������)
                //�� ��ȯX, UI�� ǥ��
                gameOverUI.ShowGameOverUI(Score, HighScore);
                break;
        }
    }

    //�÷��̾ ������(����)�� �԰ų� �� �� ���� ����
    public void AddScore(int value)
    {
        Score += value;
        if (Score > HighScore)
        {
            HighScore = Score; // �ְ������� ����
            PlayerPrefs.SetInt("HighScore", HighScore);
        }
        
        gameUI.UpdateScore(Score);
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage; //ü�� ����
        gameUI.UpdateHpText(currentHp); //UI ������Ʈ

        if (currentHp <= 0)
        {
            currentHp = 0;
            ChangeState(GameState.GameOver); //���ӿ��� ���·� ��ȯ
        }
    }

    public void CurrentHp(int hp)
    {
        currentHp = hp; //ü�� �ʱ�ȭ
        //gameUI.UpdateHpText(currentHp, maxHp); //UI ������Ʈ
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
