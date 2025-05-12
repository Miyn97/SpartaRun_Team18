using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //�̱��� ���� + UIManager.Instance �� ���ٰ���
    public static UIManager Instance { get; private set; }

    //�ν����Ϳ��� UI ��ҵ��� �������ִ� �ð��� ��
    //���� ���� ������ִ� ����.
    [Header("UI Screens")]

    //�� UI�� Ȱ��ȭ/��Ȱ��ȭ�ϰų� ������ ������ �� ���
    private IntroUI introUI;
    private StartUI startUI;
    private GameUI gameUI;
    private GameOverUI gameOverUI; // [SerializeField] ����_ryang

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
        DontDestroyOnLoad(gameObject); // �� ��ȯ�Ǿ ������� �ʵ��� ���� (���� �帧 ����) _ryang
    }


    //���� �ε�� ������ �ڵ����� OnSceneLoaded()�� ����
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //���� ��Ȱ��ȭ�� �� �ڵ����� OnSceneLoaded()�� ����
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "03_IntroScene":
                //���� �ε�Ǹ� UIManager�� �� UI�� ã�Ƽ� �ʱ�ȭ
                introUI = FindObjectOfType<IntroUI>();
                introUI?.gameObject.SetActive(true);
                break;
            case "01_StartScene":
                //���� ȭ�� UI�� ã�� Ȱ��ȭ
                startUI = FindObjectOfType<StartUI>();
                startUI?.gameObject.SetActive(true);
                break;
            case "02_MainScene":
                //���� �� UI�� ã�� Ȱ��ȭ
                gameUI = FindObjectOfType<GameUI>();
                // GameOverUI ã�� / null���� ������
                var all = Resources.FindObjectsOfTypeAll<GameOverUI>();
                gameOverUI = System.Array.Find(all, ui => ui.gameObject.scene == scene);
                // ���� ���� UI�� ��Ȱ��ȭ
                gameUI?.gameObject.SetActive(true);
                gameOverUI?.gameObject.SetActive(false); // ���� ���� UI�� ��Ȱ��ȭ
                break;
        }
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
    public void ShowGameOverUI(int finalScore, int bestScore)
    {
        //���� ���� ��Ȳ������ �����ִ� UI�� ��
        gameOverUI.Show(finalScore, bestScore);
    }

    //GameUI�� ���� ǥ�� ������ ��û
    public void UpdateScore(int score, int bestScore)
    {
        //gameUI�� null�� �ƴ� ���� �����
        gameUI?.SetScore(score, bestScore);
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
