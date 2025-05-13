using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
/*
[RequireComponent]란?
이 스크립트가 붙어 있는 GameObject에는
반드시 특정 컴포넌트가 함께 붙어 있어야 한다는 것을 Unity에게 알려주는 기능
즉, 이 PlayerView 스크립트를 GameObject에 붙이면,
해당 오브젝트에는 무조건 Rigidbody2D와 Animator도 같이 붙는다.
= 에디터에서 실수하지 않게 + NullReference를 피하기위해
*/

public class PlayerView : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;

    // 슬라이드용 콜라이더 설정값
    [SerializeField] private Vector2 slideColliderSize = new Vector2(1.6f, 0.8f);
    [SerializeField] private Vector2 slideColliderOffset = new Vector2(-0.5f, 0.4f);

    // 원래 콜라이더 설정값
    private Vector2 defaultColliderSize;
    private Vector2 defaultColliderOffset;


    //리지드바디 (물리적 이동, 속도, 점프) 컴포넌트
    private Rigidbody2D _rigidbody;
    //애니메이션 실행을 위한 컴포넌트
    private Animator animator;
    

    // 슬라이드용 크기 조절에 사용
    [SerializeField] private Transform spriteTransform;
    //슬라이드할 때 줄어들 Y 비율 (기본 높이의 절반으로 줄이겠다는 의미)
    [SerializeField] private float slideScaleY = 0.5f;

    //플레이어의 기본 Y 크기를 저장해두는 변수 (슬라이드 후 복구할 때 사용)
    private float defaultScaleY;

    private void Awake()
    {
        //오브젝트에 붙은 리지드바디 + 애니메이터 가져옴
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //SpriteTransform이 있다면
        if (spriteTransform != null)
            //현재 Y 크기를 저장 = 슬라이드 후 원래 크기로 되돌릴 때 사용
            defaultScaleY = spriteTransform.localScale.y;

        //boxCollider가 있다면
        if (boxCollider != null)
        {
            defaultColliderSize = boxCollider.size;
            defaultColliderOffset = boxCollider.offset;
        }
    }

    //자동 이동 처리
    public void Move(float speed)
    {
        //x축으로 Speed만큼 이동시킴 (y축은 속도 유지)
        _rigidbody.velocity = new Vector2(speed, _rigidbody.velocity.y);

        //애니메이터에게 속도 전달
        animator.SetFloat("Speed", speed);
    }

    //점프 처리
    public void Jump(float jumpForce)
    {
        //이번엔 반대로 y축으로 점프 힘을 가함 (x축은 속도 유지)
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
        //애니메이터에서 Jump라는 트리거 실행 + 재생
        animator.SetTrigger("Jump"); //애니메이션 생성 시 주석처리 해제
    }

    //착지 했는지에 대한 애니메이션 전용 메서드
    public void SetGrounded(bool value)
    {
        animator.SetBool("isGrounded", value);
    }

    //슬라이드 시작 처리
    public void Slide()
    {
        //SpriteTransform이 있다면
        if (spriteTransform != null)
        {
            //시각적으로 몸을 낮추기 위해 Sprite의 Y 스케일을 줄임
            spriteTransform.localScale = new Vector3(1f, slideScaleY, 1f);

            //위치 보정 (Y축 위로 올림, 예: +0.25f)
            spriteTransform.localPosition = new Vector3(0f, 0.25f, 0f);

        }
        //Animator에서 IsSliding을 true로 설정
        animator.SetBool("IsSliding", true); //애니메이션 생성 시 주석처리 해제

        // 중력에 의한 낙하 방지
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);

        //콜라이더 변경
        if (boxCollider != null)
        {
            boxCollider.size = slideColliderSize;
            boxCollider.offset = slideColliderOffset;
        }
    }

    //슬라이드 종료 처리
    public void EndSlide()
    {
        //SpriteTransform이 있다면
        if (spriteTransform != null)
        {
            //슬라이드 종료 시 원래 크기로 되돌림
            spriteTransform.localScale = new Vector3(1f, defaultScaleY, 1f);
            spriteTransform.localPosition = Vector3.zero; //원래 위치 복귀
        }
        //IsSliding 애니메이션 Bool을 꺼서 애니메이션 종료
        animator.SetBool("IsSliding", false); //애니메이션 생성 시 주석처리 해제

        //콜라이더 복원
        if (boxCollider != null)
        {
            boxCollider.size = defaultColliderSize;
            boxCollider.offset = defaultColliderOffset;
        }

    }

    //사망 애니메이션 재생
    public void PlayDeathAnimation()
    {
        //죽었을 때 실행되는 애니메이션 트리거 "Die"를 Animator에게 전달
        //예로들어서 넘어짐, 폭발, 비틀거림 등 설정된 애니메이션 재생
        animator.SetTrigger("Die");
    }

    //애니메이션 정지용 메서드
    public void StopMovementAnimation()
    {
        animator.SetFloat("Speed", 0); // ← 움직임 애니메이션 정지
        _rigidbody.velocity = Vector2.zero; // 혹시나 모를 물리 이동도 강제로 정지
    }


}
