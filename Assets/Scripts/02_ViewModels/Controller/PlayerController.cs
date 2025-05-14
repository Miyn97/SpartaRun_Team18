using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(PlayerView))]
public class PlayerController : MonoBehaviour
{
    //플레이어 애니메이션 + 물리처리 등을 게이머가 조작할 수 있도록 연결
    //[SerializeField] 잠시 주석처리
    private PlayerView playerView;

    //플레이어가 속도 up, 데미지 입음 등을 판단하면 여기다가 반영
    private PlayerModel model;

    //플레이어 최대 체력
    //initialHealth는 초기값을 [결정]해서 [전달]하는 구조
    //PlayerModel에 MaxHealth는 초기값을 [보유]하는 구조
    //한마디로 외부에서 초기값을 주기위해 = 스테이지별로 체력이 다를 수 있다는걸 고려
    [Header("Model 초기값")]
    [SerializeField] private int initialHealth = 6; // 체력 6칸 _ryang
    //기본 이동 속도
    [SerializeField] private float initialSpeed = 5f;

    [Header("Jump/Slide")]
    //점프할 때 위로 가해지는 힘
    [SerializeField] private float jumpForce = 13f;
    //슬라이드 지속 시간
    [SerializeField] private float slideDuration = 1f;

    private bool isGround = true; // 땅에 있는 상태
    private bool isDoubleJump = false; //더블 점프 가능 상태

    //슬라이딩을 했는가?
    private bool isSliding = false;
    //남은 슬라이드 시간 저장용 변수
    private float slideTimer = 0f;
    public bool IsInvincible { get; private set; }

    private void Awake() //update보다 먼저 실행
    {
        // 플레이어 뷰를 가져옴
        playerView = GetComponent<PlayerView>();

        // 모델 생성 및 초기화 (체력과 속도를 설정)
        model = new PlayerModel(initialHealth, initialSpeed);
    }

    private void Update()
    {
        //GameOver 상태면 입력 막기
        if (GameManager.Instance.CurrentState == GameManager.GameState.GameOver)
            return;
        //키 입력 처리 확인
        HandleInput();
    }

    //자동 이동을 위한 FixedUpdate
    private void FixedUpdate()
    {
        //GameOver 상태일 땐 자동 이동 막기
        if (GameManager.Instance.CurrentState == GameManager.GameState.GameOver)
            return;
        // View에 이동 요청
        playerView.Move(model.Speed);
    }

    //키 입력 처리하는 메서드
    private void HandleInput()
    {
        //점프 입력 (슬라이드 중일 땐 점프 금지)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isSliding) // 슬라이드 중이 아닐 때만 점프 허용
            {
                Jump(); // 점프키는 Space
            }
        }

        //슬라이드 시작
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!isSliding && isGround) // 지면일 때만 슬라이드 허용
            {
                Slide(); // Shift를 누른 순간 슬라이드 시작
            }
        }
        else
        {
            if (isSliding)
            {
                EndSlide(); // Shift에서 손 뗐을 때 슬라이드 종료
            }
        }


        //체력감소 테스트
        if (Input.GetKeyDown(KeyCode.H))
        {
            GameManager.Instance.TakeDamage(1); //체력 감소 테스트
        }
        //점수 증가 테스트
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.Instance.AddScore(36); //점수 증가 테스트
        }
    }

    //점프 조작
    private void Jump()
    {
        if (isGround)
        {
            playerView.Jump(jumpForce); //점프 애니메이션 요청
            playerView.SetGrounded(false); // 점프 직후 공중 상태 전달
            // 뷰에게 점프 애니메이션/물리 적용 명령
            isGround = false; //점프!
            isDoubleJump = true; //더블 점프 가능
        }
        else if (isDoubleJump)
        {
            playerView.DoubleJump(jumpForce); //더블 점프 애니메이션 요청
            isDoubleJump = false; //더블 점프!
            playerView.SetGrounded(false); // 더블 점프 직후 공중 상태 전달
        }
        else
        {
            Debug.Log("더블 점프까지만 가능합니다.");
        }
    }

    //슬라이드 조작
    private void Slide()
    {
        Debug.Log("슬라이드 중입니다.");
        isSliding = true; //슬라이드 중 상태
        slideTimer = slideDuration; //슬라이드 지속 시간 설정
        playerView.Slide(); //슬라이드 애니메이션 요청
    }

    //슬라이드 종료
    private void EndSlide()
    {
        Debug.Log("슬라이드 종료");
        isSliding = false;
        playerView.EndSlide(); // 애니메이션 종료
    }

    // 땅에 닿았는지 확인
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGround = true; //땅에 닿았을 때
            isDoubleJump = false; //더블 점프 초기화
            playerView.SetGrounded(true); //착지했는지를 PlayerView에게 알림
        }
    }

    // 데미지를 받을 경우
    public void TakeDamage(int damage)
    {

        if (IsInvincible == true)
        {
            Debug.Log("무적임");
            return;
        }
        else
        {
            //데미지를 받았을 때 체력 감소
            model.TakeDamage(damage);
            int updateHp = model.CurrentHealth;

            //UIManager.Instance.UpdateHealth(updateHp); //UI 업데이트

            //체력이 0 이하이면
            if (model.CurrentHealth == 0)
            {
                Debug.Log("죽었습니다.");
                //죽었을 때 애니메이션 실행
                //playerView.PlayDeathAnimation(); //애니메이션 생성 시 주석처리 해제
                //사망처리 메서드 호출
                Die();
            }
            else
            {
                Debug.Log("체력 : " + model.CurrentHealth);
            }
        }

    }

    //사망처리 메서드
    private void Die()
    {
        //playerView.SetAnimatorSpeed(0f);
        //죽었을 때 애니메이션 실행
        playerView.PlayDeathAnimation(); //애니메이션 생성 시 주석처리 해제
        playerView.StopMovementAnimation(); // 움직임 강제 정지
        //View는 죽는 연출 + 게임매니저는 상태변화
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
        playerView.StopMovementAnimation();
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

        GameManager.Instance.Heal(amount);
    }

    //속도아이템 관련
    public void SetSpeed(float newSpeed)
    {
        //속도 아이템을 먹었을 때 현재 이동 속도 변경
        model.SetSpeed(newSpeed);
    }
    public void SetInvincible(bool value)
    {
        IsInvincible = value;
    }
}
