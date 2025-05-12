using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    //���� ����
    [SerializeField] private Button startButton;
    //�ɼ�â ����
    [SerializeField] private Button optionButton;
    //���� ���� ��û
    [SerializeField] private Button exitButton;

    //�ܺη� ������ �̺�Ʈ ���� ��������Ʈ ���
    //Action�� �Ű����� ���� ��������Ʈ Ÿ��
    //��ư�� �������� �˸��⸸ �ϰ�, ������ �� ������ �ܺ�(GameManager)�� ����
    public event System.Action OnStartRequested;
    public event System.Action OnOptionRequested;
    public event System.Action OnExitRequested;

    private void Awake()
    {
        //Start��ư�� Ŭ���Ǹ� OnStartRequested �̺�Ʈ�� ȣ���� = ���� ���� ��û
        startButton.onClick.AddListener(() => OnStartRequested?.Invoke());
        //Option��ư�� Ŭ���Ǹ� OnOptionRequested �̺�Ʈ�� ȣ���� = �ɼ� ����
        optionButton.onClick.AddListener(() => OnOptionRequested?.Invoke());
        //Exit��ư�� Ŭ���Ǹ� OnExitRequested �̺�Ʈ�� ȣ���� = ���� ���� ��û
        exitButton.onClick.AddListener(() => OnExitRequested?.Invoke());
    }

}
