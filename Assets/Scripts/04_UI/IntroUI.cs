using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    //현재 줄의 스토리를 보여줌
    [SerializeField] private TextMeshProUGUI storyText;
    //다음 줄로 넘기기
    [SerializeField] private Button nextButton;
    //전체 스토리를 생략하고 마지막으로
    [SerializeField] private Button skipButton;
    //스토리 끝나고 게임 시작 요청용 버튼
    [SerializeField] private Button startButton;

    [TextArea(3, 10)]
    //인스펙터에서 여러 줄의 텍스트를 편하게 입력할 수 있도록 해줌
    //storyLines : 순서대로 출력될 스토리 배열
    [SerializeField] private string[] storyLines;

    //View > ViewModel로 이벤트 전달하는 통로
    //Start버튼이 눌렸을 때 외부(GameManager)에게 인트로 끝났음! 하고 알려주는 역할
    public event System.Action OnIntroComplete;

    //현재 몇 번째 줄을 보고 있는지 추적
    private int currentLineIndex = 0;
    //현재 실행 중인 텍스트 타자 효과 코루틴 저장 (중복 실행 방지)
    private Coroutine typingCoroutine;

    private void Awake()
    {
        //각각의 버튼 클릭 시 실행할 함수 지정
        nextButton.onClick.AddListener(ShowNextLine);
        skipButton.onClick.AddListener(SkipIntro);
        //스타트 버튼은 클릭 시 OnIntroComplete 이벤트 호출 > 외부로 알림
        startButton.onClick.AddListener(() => OnIntroComplete?.Invoke());
        //인트로 시작할 때 게임 시작 버튼 보이지 않게 설정
        startButton.gameObject.SetActive(false);
    }

    //오브젝트가 SetActive(true)가 될 때 실행됨
    private void OnEnable()
    {
        currentLineIndex = 0;
        //스토리의 첫 줄부터 출력 시작
        ShowLine();
    }

    //현재 줄을 출력하는 함수
    private void ShowLine()
    {
        //기존에 타이핑 중이던 코루틴이 있으면 중단
        if (typingCoroutine != null || storyLines.Length == 0)
            StopCoroutine(typingCoroutine);

        //새로 현재 줄을 타이핑 시작
        typingCoroutine = StartCoroutine(TypeLine(storyLines[currentLineIndex]));
    }

    //텍스트를 한 글자씩 출력하며 간격을 둠
    private IEnumerator TypeLine(string line)
    {
        storyText.text = "";
        foreach (char c in line)
        {
            storyText.text += c;
            //타자기 효과
            yield return new WaitForSeconds(0.05f);
        }
    }

    //사용자가 다음 버튼을 눌렸을 때 호출됨
    private void ShowNextLine()
    {
        //현재 줄 인덱스를 증가시킴
        currentLineIndex++;

        //아직 줄이 남아 있다면 다음 줄 출력
        if (currentLineIndex < storyLines.Length)
        {
            //아직 줄이 남이 있다면 다음 줄 출력
            ShowLine();
        }
        else
        {
            //모든 줄을 다 봤으면 "게임 시작" 버튼을 활성화
            startButton.gameObject.SetActive(true);
            //다음/스킵 버튼은 숨김
            nextButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
        }
    }

    private void SkipIntro()
    {
        currentLineIndex = storyLines.Length;
        storyText.text = storyLines[^1]; //마지막 줄 바로 보여줌
        startButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
    }
}
