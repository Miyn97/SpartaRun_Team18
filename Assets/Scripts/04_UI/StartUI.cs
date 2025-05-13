using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [Header("MainMenu")]
    //게임 시작
    [SerializeField] private Button startButton;
    //옵션창 열기
    [SerializeField] private Button optionButton;
    //게임 종료 요청
    [SerializeField] private Button exitButton;

    //옵션창 패널
    [Header("Option")]
    public GameObject optionPanel;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button applyButton;
    public Button cancelButton;


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

        //옵션창 패널은 비활성화 상태로 시작
        optionPanel.SetActive(false);

        applyButton.onClick.AddListener(OnClickApplyButton);
        cancelButton.onClick.AddListener(OnClickCancleButton);
    }

    public void ShowOptionPanel()
    {
        optionPanel.SetActive(true);

        // 저장된 볼륨 값 불러오기, "***Volume" 이라는 이름의 키로 저장된 값을 가져옴. 없으면 기본값.
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.2f); // 기본값 0.2f
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.2f);

        // 참고 사항: 
        // 볼륨값 저장, 관리하는 건, 게임매니져에서 GetFloat("***Volume")로 불러옴
        // SoundManager스크립트에서 Awake()에서 PlayerPrefs.GetFloat("BGMVolume")로 불러옴 - 기본값 0.2f
    }


    // 설정 저장 버튼 클릭 시 호출되는 메서드
    public void OnClickApplyButton()
    {
        // 슬라이더의 현재 값을 PlayerPrefs에 저장. 키값이 "***Volume"인 값을 ***Slider의 현재 값으로 설정.
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        // 직접 볼륨 적용하는 건, SoundManager에서 SetVolume 메서드 호출

        // PlayerPrefs에 저장된 값을 즉시 적용하기 위해 Save 호출.
        PlayerPrefs.Save();

        // 옵션창 닫기
        optionPanel.SetActive(false);
    }

    // 옵션창 닫기 버튼 클릭 시 호출되는 메서드 (저장 안하고 그냥 닫힘)
    public void OnClickCancleButton()
    {
        optionPanel.SetActive(false);
    }
}
