using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //플레이어 애니메이션 + 물리처리 등을 게이머가 조작할 수 있도록 연결
    [SerializeField] private PlayerView playerView;

    //플레이어가 속도 up, 데미지 입음 등을 판단하면 여기다가 반영
    private PlayerModel model;

    //플레이어 최대 체력
    //initialHealth는 초기값을 [결정]해서 [전달]하는 구조
    //PlayerModel에 MaxHealth는 초기값을 [보유]하는 구조
    //한마디로 외부에서 초기값을 주기위해 = 스테이지별로 체력이 다를 수 있다는걸 고려
    [SerializeField] private int initialHealth = 100;
    //기본 이동 속도
    [SerializeField] private float initialSpeed = 5f;
    //점프할 때 위로 가해지는 힘
    [SerializeField] private float jumpForce = 8f;
    //슬라이드 지속 시간
    [SerializeField] private float slideDuration = 1f; 

    //슬라이딩을 했는가?
    private bool isSliding = false;
    //남은 슬라이드 시간 저장용 변수
    private float slideTimer = 0f; 

    private void Start()
    {
        // 모델 생성 및 초기화 (체력과 속도를 설정)
        model = new PlayerModel(initialHealth, initialSpeed);
    }

    private void Update()
    {
        //키 입력 처리 확인
        HandleInput();
        //슬라이드 지속 시간 처리 확인
        HandleSlide(); 
    }

    //키 입력 처리하는 메서드
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(); //점프키는 Space
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Slide(); //슬라이드 키는 Shift
        }
    }

    //점프
    private void Jump()
    {
        // 뷰에게 점프 애니메이션/물리 적용 명령
        //playerView.Jump(jumpForce); //애니메이션 생성 시 주석처리 해제
    }

    //슬라이드
    private void Slide()
    {
        //슬라이드 중이 아닐 때
        if (!isSliding) 
        {
            //슬라이드 true
            isSliding = true;
            //슬라이드 타이머 설정
            slideTimer = slideDuration;
            //뷰에게 애니메이션 요청
            //playerView.Slide(); //애니메이션 생성 시 주석처리 해제
        }
    }

    //슬라이드 조작
    private void HandleSlide()
    {
        if (isSliding)
        {
            //프레임 시간만큼 슬라이드 시간 감소
            slideTimer -= Time.deltaTime;
            //슬라이드 시간이 끝나면
            if (slideTimer <= 0) 
            {
                //슬라이드 false
                isSliding = false;
                //뷰에게 종료 애니메이션 요청
                //playerView.EndSlide(); //애니메이션 생성 시 주석처리 해제
            }
        }
    }

    // 데미지를 받을 경우
    public void TakeDamage(int damage)
    {
        //데미지를 받았을 때 체력 감소
        model.TakeDamage(damage); 

        //체력이 0 이하이면
        if (model.CurrentHealth <= 0)
        {
            Die(); //사망
        }
    }

    //사망처리 메서드
    private void Die()
    {
        //죽었을 때 애니메이션 실행
        //playerView.PlayDeathAnimation(); //애니메이션 생성 시 주석처리 해제
        //View는 죽는 연출 + 게임매니저는 상태변화
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
    }

    // 아이템 획득 등 외부에서 호출
    public void AddScore(int score)
    {
        //모델에 점수를 더함 
        model.AddScore(score);
        //게임매니저 AddScore도 업데이트
        GameManager.Instance.AddScore(score);
    }

    //회복아이템 관련
    public void Heal(int amount)
    {
        //회복 아이템을 먹었을 때 회복 처리
        model.Heal(amount);
    }

    //속도아이템 관련
    public void SetSpeed(float newSpeed)
    {
        //속도 아이템을 먹었을 때 현재 이동 속도 변경
        model.SetSpeed(newSpeed);
    }
}
