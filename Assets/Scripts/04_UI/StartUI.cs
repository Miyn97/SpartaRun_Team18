using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    //게임 시작
    [SerializeField] private Button startButton;
    //옵션창 열기
    [SerializeField] private Button optionButton;
    //게임 종료 요청
    [SerializeField] private Button exitButton;

    //외부로 전달할 이벤트 선언 델리게이트 기반
    //Action은 매개변수 없는 델리게이트 타입
    //버튼이 눌렸음을 알리기만 하고, 실제로 뭘 할지는 외부(GameManager)가 결정
    public event System.Action OnStartRequested;
    public event System.Action OnOptionRequested;
    public event System.Action OnExitRequested;

    private void Awake()
    {
        //Start버튼이 클릭되면 OnStartRequested 이벤트를 호출함 = 게임 시작 요청
        startButton.onClick.AddListener(() => OnStartRequested?.Invoke());
        //Option버튼이 클릭되면 OnOptionRequested 이벤트를 호출함 = 옵션 열기
        optionButton.onClick.AddListener(() => OnOptionRequested?.Invoke());
        //Exit버튼이 클릭되면 OnExitRequested 이벤트를 호출함 = 게임 종료 요청
        exitButton.onClick.AddListener(() => OnExitRequested?.Invoke());
    }

}
