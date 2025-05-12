using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    //���� ���� ���丮�� ������
    [SerializeField] private TextMeshProUGUI storyText;
    //���� �ٷ� �ѱ��
    [SerializeField] private Button nextButton;
    //��ü ���丮�� �����ϰ� ����������
    [SerializeField] private Button skipButton;
    //���丮 ������ ���� ���� ��û�� ��ư
    [SerializeField] private Button startButton;

    [TextArea(3, 10)]
    //�ν����Ϳ��� ���� ���� �ؽ�Ʈ�� ���ϰ� �Է��� �� �ֵ��� ����
    //storyLines : ������� ��µ� ���丮 �迭
    [SerializeField] private string[] storyLines;

    //View > ViewModel�� �̺�Ʈ �����ϴ� ���
    //Start��ư�� ������ �� �ܺ�(GameManager)���� ��Ʈ�� ������! �ϰ� �˷��ִ� ����
    public event System.Action OnIntroComplete;

    //���� �� ��° ���� ���� �ִ��� ����
    private int currentLineIndex = 0;
    //���� ���� ���� �ؽ�Ʈ Ÿ�� ȿ�� �ڷ�ƾ ���� (�ߺ� ���� ����)
    private Coroutine typingCoroutine;

    private void Awake()
    {
        //������ ��ư Ŭ�� �� ������ �Լ� ����
        nextButton.onClick.AddListener(ShowNextLine);
        skipButton.onClick.AddListener(SkipIntro);
        //��ŸƮ ��ư�� Ŭ�� �� OnIntroComplete �̺�Ʈ ȣ�� > �ܺη� �˸�
        startButton.onClick.AddListener(() => OnIntroComplete?.Invoke());
        //��Ʈ�� ������ �� ���� ���� ��ư ������ �ʰ� ����
        startButton.gameObject.SetActive(false);
    }

    //������Ʈ�� SetActive(true)�� �� �� �����
    private void OnEnable()
    {
        currentLineIndex = 0;
        //���丮�� ù �ٺ��� ��� ����
        ShowLine();
    }

    //���� ���� ����ϴ� �Լ�
    private void ShowLine()
    {
        //������ Ÿ���� ���̴� �ڷ�ƾ�� ������ �ߴ�
        if (typingCoroutine != null || storyLines.Length == 0)
            StopCoroutine(typingCoroutine);

        //���� ���� ���� Ÿ���� ����
        typingCoroutine = StartCoroutine(TypeLine(storyLines[currentLineIndex]));
    }

    //�ؽ�Ʈ�� �� ���ھ� ����ϸ� ������ ��
    private IEnumerator TypeLine(string line)
    {
        storyText.text = "";
        foreach (char c in line)
        {
            storyText.text += c;
            //Ÿ�ڱ� ȿ��
            yield return new WaitForSeconds(0.05f);
        }
    }

    //����ڰ� ���� ��ư�� ������ �� ȣ���
    private void ShowNextLine()
    {
        //���� �� �ε����� ������Ŵ
        currentLineIndex++;

        //���� ���� ���� �ִٸ� ���� �� ���
        if (currentLineIndex < storyLines.Length)
        {
            //���� ���� ���� �ִٸ� ���� �� ���
            ShowLine();
        }
        else
        {
            //��� ���� �� ������ "���� ����" ��ư�� Ȱ��ȭ
            startButton.gameObject.SetActive(true);
            //����/��ŵ ��ư�� ����
            nextButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
        }
    }

    private void SkipIntro()
    {
        currentLineIndex = storyLines.Length;
        storyText.text = storyLines[^1]; //������ �� �ٷ� ������
        startButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
    }
}
