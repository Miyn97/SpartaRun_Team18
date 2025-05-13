using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [Header("MainMenu")]
    //���� ����
    [SerializeField] private Button startButton;
    //�ɼ�â ����
    [SerializeField] private Button optionButton;
    //���� ���� ��û
    [SerializeField] private Button exitButton;

    //�ɼ�â �г�
    [Header("Option")]
    public GameObject optionPanel;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button applyButton;
    public Button cancelButton;


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

        //�ɼ�â �г��� ��Ȱ��ȭ ���·� ����
        optionPanel.SetActive(false);

        applyButton.onClick.AddListener(OnClickApplyButton);
        cancelButton.onClick.AddListener(OnClickCancleButton);
    }

    public void ShowOptionPanel()
    {
        optionPanel.SetActive(true);

        // ����� ���� �� �ҷ�����, "***Volume" �̶�� �̸��� Ű�� ����� ���� ������. ������ �⺻��.
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.2f); // �⺻�� 0.2f
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.2f);

        // ���� ����: 
        // ������ ����, �����ϴ� ��, ���ӸŴ������� GetFloat("***Volume")�� �ҷ���
        // SoundManager��ũ��Ʈ���� Awake()���� PlayerPrefs.GetFloat("BGMVolume")�� �ҷ��� - �⺻�� 0.2f
    }


    // ���� ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnClickApplyButton()
    {
        // �����̴��� ���� ���� PlayerPrefs�� ����. Ű���� "***Volume"�� ���� ***Slider�� ���� ������ ����.
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        // ���� ���� �����ϴ� ��, SoundManager���� SetVolume �޼��� ȣ��

        // PlayerPrefs�� ����� ���� ��� �����ϱ� ���� Save ȣ��.
        PlayerPrefs.Save();

        // �ɼ�â �ݱ�
        optionPanel.SetActive(false);
    }

    // �ɼ�â �ݱ� ��ư Ŭ�� �� ȣ��Ǵ� �޼��� (���� ���ϰ� �׳� ����)
    public void OnClickCancleButton()
    {
        optionPanel.SetActive(false);
    }
}
