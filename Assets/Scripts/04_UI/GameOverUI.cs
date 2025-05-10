using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    //�ν����� �󿡼� Core��� �������� ����
    [Header("Score")]
    //�̹� �÷����� ���� ������ ����� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI finalScoreText;
    //����� �ְ� ������ ����� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI highScoreText;

    //�ν����Ϳ��� Button�̶�� �׷����� ��� ǥ��
    [Header("Button")]
    //������ �ٽ� ������ �� ������ ��ư
    [SerializeField] private Button restartButton;
    //���� ȭ�� �Ǵ� �κ�� ���ư��� ���� ��ư
    [SerializeField] private Button homeButton;

    //�ܺο� ��ư Ŭ���� �˸��� �̺�Ʈ ����
    //�� View�� ���� ���¸� �ٲ��� �ʰ�, ��ư�� ���ȴٴ� ��û�� �̺�Ʈ�� �˷���
    public event System.Action OnRestartRequested;
    public event System.Action OnReturnHomeRequested;

    private void Awake()
    {
        //���� �� ��Ȱ��ȭ ���·� ������ �־�� �ϹǷ� ����
        gameObject.SetActive(false);

        //�ٽ� ���� ��ư�� Ŭ���ϸ� OnRestartRequested �̺�Ʈ�� ȣ��
        //�� �̺�Ʈ�� ����� �����ڰ� ���� ���� �����ϰ� ȣ��
        restartButton.onClick.AddListener(() => OnRestartRequested?.Invoke());
        //Ȩ���� ��ư Ŭ���� OnReturnHomeRequested �̺�Ʈ�� ȣ����
        //�ܺο� "Ȩ���� ���ư��� �ؿ�!"��� �˸� ��, ���� �ൿ�� ���� ����
        homeButton.onClick.AddListener(() => OnReturnHomeRequested?.Invoke());
    }

    //���� ������ �߻��ϸ� �ܺΰ� �� �޼��带 ȣ��
    //������ �����ϰ� UI�� �����ְ� ��
    public void Show(int finalScore, int highScore)
    {
        //���޹��� �������� �ؽ�Ʈ�� ��ȯ�ؼ� UI�� ǥ����
        finalScoreText.text = finalScore.ToString();
        highScoreText.text = highScore.ToString();

        //�� GameOverUI ������Ʈ�� Ȱ��ȭ�ؼ� ���� ȭ�鿡 ��Ÿ���� ��
        gameObject.SetActive(true);
    }

    //������ �ٽ� �����ϰų�,
    //Ȩ���� ���ư� �� UI�� ����� ���� �Լ�
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
