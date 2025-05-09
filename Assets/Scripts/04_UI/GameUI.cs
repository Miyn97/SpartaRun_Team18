using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    //Score �󺧷� �׷� �������
    [Header("Score")]
    //textMeshProUGUI�� ȭ�鿡 �ؽ�Ʈ�� �׸��� TMP�� ������Ʈ
    [SerializeField] private TextMeshProUGUI currentScoreText;

    [Header("Hp: 3 Heart / 6 Hp")]
    //��Ʈ 3�� (2 HP�� ��6)�� ü���� ������ �迭
    // 3���� ����� ������ ����. 1���� 2ĭ���� ����� ������ ����.
    [SerializeField] private Image[] hpImages;
    //��Ʈ�� Ǯ HP�� ��
    [SerializeField] private Sprite heart_full;
    //��Ʈ�� 1 HP�� ��
    [SerializeField] private Sprite heart_half;
    //��Ʈ�� 0 HP�� ��
    [SerializeField] private Sprite heart_empty;

    [Header("Pause/Option")]
    //�Ͻ����� �޴�â�� ���� ��ư�� ������ ����
    // �ܼ� ���� �Ͻ�������ɸ� or �ɼǸ޴� �ǳڷ� �ص� ������,
    // ����-> �ɼ� �� ������ ������ �̵��ϴ� ��ĵ� ������
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button pauseButton;

    //delegate�� �̺�Ʈ�� �ܺο� ������ �� �ִ� ����
    public delegate void PauseRequestedHandler();
    //OnpauseRequested�� ���� ��ư�� ���ȴ� ��� ��Ǹ� �ܺ� ViewModel���� �˸�
    public event PauseRequestedHandler OnPauseRequested;

    private void Awake()
    {
        // �ɼ��г� ó������ �������� ����.
        optionPanel.SetActive(false);
        //���� ��ư�� Ŭ���ϸ� NotifyPauseRequested() �Լ��� ȣ��ǵ��� ����
        pauseButton.onClick.AddListener(NotifyPauseRequested);
    }

    public void SetScore(int score)
    {
        // ���� ���� ���ڿ��� �ٲ㼭 ȭ�鿡 ǥ��
        currentScoreText.text = score.ToString();
    }

    public void SetHealth(int currentHp)
    {
        // ������ ���� ����, ü�� UI ������Ʈ
        for (int i = 0; i < hpImages.Length; i++)
        {
            //�ϳ��� ��Ʈ�� 2ü�� ����ϴϱ�
            //i��°�� ��Ʈ�� ���� ���� ü���� �󸶳� ǥ���ؾ� �ϴ��� ���
            int heartIndex = Mathf.Clamp(currentHp - i * 2, 0, 2);
            Sprite sprite = heartIndex switch
            {
                0 => heart_full,
                1 => heart_half,
                _ => heart_empty
            };
            // hp UI�� ��������Ʈ(��Ʈ�̹���) ����
            hpImages[i].sprite = sprite;
        }

    }

    //�ܺο��� �ɼ�â�� �Ѱų� �� �� �ֵ��� ������ �Լ�.
    public void ToggleOptionPanel(bool isActive)
    {
        //View�� UI�� �����ְ�, ���� ���� �Ͻ������� GameManager�� ó��
        optionPanel.SetActive(isActive);
    }

    //���� ��ư�� ������, �ܺο� "�Ͻ����� ��û�� �߻��ߴ�!" ��� �˸�
    private void NotifyPauseRequested()
    {
        //���� �Ͻ����� ���� �ʰ�, ��û�� ������ ����
        OnPauseRequested?.Invoke();
    }
}
