using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    //Score �󺧷� �׷� �������
    [Header("Score")]
    [SerializeField] private GameObject scoreHp_Panel;
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

    [Header("Option")]
    //����â (�Ͻ����� ��)
    [SerializeField] private Button optionButton;
    [SerializeField] private GameObject optionPanel;

    [Header("PanelButtons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button homeButton;

    //[Header("Audio")]
    //[SerializeField] private Slider volumeSlider; // ���� �����̴�(���� �Ǹ� �ּ�ó�� �����ϰ� ����)

    //delegate�� �̺�Ʈ�� �ܺο� ������ �� �ִ� ����
    public delegate void PauseRequestedHandler();
    //OnpauseRequested�� ���� ��ư�� ���ȴ� ��� ��Ǹ� �ܺ� ViewModel���� �˸�
    public event PauseRequestedHandler OnPauseRequested;
    //StartScene���� ���ư��� �ʹ� ��� ��û�� �ܺο� �˸�
    public event System.Action OnReturnHomeRequested;

    //AudioManager���� ������ �����϶�� ��û
    // �����̴� ���� �ٲ� ������ �ش� �� ȣ�� => ���߿� SoundManager���� �ش� �� �޾Ƽ� ���� ����ǵ��� �����ؾ���.
    //public event System.Action<float> OnVolumeChanged; // (���� �Ǹ� �ּ�ó�� �����ϰ� ����)

    private void Awake()
    {
        optionPanel.SetActive(false);

        //���� ��ư�� Ŭ���ϸ� NotifyPauseRequested() �Լ��� ȣ��ǵ��� ����
        optionButton.onClick.AddListener(NotifyPauseRequested);
        //�Ͻ����� ��ư Ŭ�� �� �Ͻ����� ��û �̺�Ʈ �߻� (�����ڿ��� �˸�)
        OnPauseRequested += () => optionPanel.SetActive(true);

        //Resume��ư : �ɼ�â �ݰ�, �Ͻ����� ����
        resumeButton.onClick.AddListener(() =>
        {
            optionPanel.SetActive(false);
            NotifyPauseRequested();
        });

        //home��ư : StartScene���� ���ư���
        homeButton.onClick.AddListener(() =>
        {
            //������ �����ϰ�, ���� ȭ������ ���ư�����
            optionPanel.SetActive(false);
            OnReturnHomeRequested?.Invoke();
        });

        // ���� �����̴��� ���� �ٲ� ������ OnVolumeChanged �̺�Ʈ�� ȣ��
        //volumeSlider.onValueChanged.AddListener(value => OnVolumeChanged?.Invoke(value));
    }

    // �ɼ� ��ư ������ GameUI ��Ȱ��ȭ
    public void HideGameUI()
    {
        scoreHp_Panel.SetActive(false);
    }

    public void SetScore(int score, int bestScore)
    {
        // ���� ���� ���ڿ��� �ٲ㼭 ȭ�鿡 ǥ��
        currentScoreText.text = score.ToString();
        // �����߿��� �ְ� ���� ǥ�� ����
    }

    public void SetHealth(int currentHp)
    {
        // ������ ���� ����, ü�� UI ������Ʈ
        for (int i = 0; i < hpImages.Length; i++)
        {
            //�ϳ��� ��Ʈ�� 2ü�� ����ϴϱ�
            //i��°�� ��Ʈ�� ���� ���� ü���� �󸶳� ǥ���ؾ� �ϴ��� ���
            int hpHeart = Mathf.Clamp(currentHp - i * 2, 0, 2);
            Sprite sprite = hpHeart switch
            {
                0 => heart_empty,
                1 => heart_half,
                _ => heart_full
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
