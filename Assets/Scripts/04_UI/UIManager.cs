using UnityEngine;

public class UIManager : MonoBehaviour
{
    //�̱��� ���� + UIManager.Instance �� ���ٰ���
    public static UIManager Instance { get; private set; }

    //�ν����Ϳ��� UI ��ҵ��� �������ִ� �ð��� ��
    //���� ���� ������ִ� ����.
    [Header("UI Screens")]

    //�� UI�� Ȱ��ȭ/��Ȱ��ȭ�ϰų� ������ ������ �� ���
    [SerializeField] private IntroUI introUI;
    [SerializeField] private StartUI startUI;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameOverUI gameOverUI;

    private void Awake()
    {
        //UIManager 2�� ���� + �̱��� ��Ģ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //UIManager�� �ν��Ͻ��� ���
        Instance = this;
    }

    //Intro ȭ���� �����ִ� �Լ�
    public void ShowIntroUI()
    {
        //�ٸ� UI�� ���� ������ introUI�� ������ ��
        SetOnlyActive(introUI);
    }

    //Start ȭ���� �����ִ� �Լ�
    public void ShowStartUI()
    {
        //���� ȭ��(UI)�� Ȱ��ȭ.
        SetOnlyActive(startUI);
    }

    //GameUI ȭ���� �����ִ� �Լ�
    public void ShowGameUI()
    {
        //���� �� ������ ü�� ���� �����ִ� GameUI�� Ȱ��ȭ
        SetOnlyActive(gameUI);
    }

    //GameOverUI ȭ���� �����ִ� �Լ�
    public void ShowGameOverUI()
    {
        //���� ���� ��Ȳ������ �����ִ� UI�� ��
        SetOnlyActive(gameOverUI);
    }

    //GameUI�� ���� ǥ�� ������ ��û
    public void UpdateScore(int score)
    {
        //gameUI�� null�� �ƴ� ���� �����
        gameUI?.SetScore(score);
    }

    //ü�� UI�� �����ϴ� �Լ�
    public void UpdateHealth(int health)
    {
        //���� GameUI�� ���� �Ѱ��� ��, UIManager�� ���� ǥ������ ����
        gameUI?.SetHealth(health);
    }

    //���޵� �ϳ��� UI�� �Ѱ�, �������� ���� ���� ���� ��ȯ �Լ�
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
