using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ObstacleType    // 장애물 종류
{
    SyntaxErrorBox,         // 문법 오류 모양 장애물
    CompileErrorWall,       // 컴파일 에러 모양의 벽 장애물
    RedLineTrap,            // 빨간 줄이 쳐진 트랩 장애물
    UnhandledExceptionBox   // 처리되지 않은 예외 박스 장애물
}


public class ObstacleModel : MonoBehaviour
{
    // 스프라이트 이미지 받아오기
    public Sprite spriteSyntaxError;         // SyntaxError 이미지
    public Sprite spriteCompileError;        // CompileError 이미지
    public Sprite spriteRedLine;             // RedLine 이미지
    public Sprite spriteUnhandledException;  // UnhandledException 이미지

    SpriteRenderer spriteRenderer;           // 스프라이트 랜더러 컴포넌트를 받아올 변수

    public ObstacleType obstacleType;        // 장애물 종류 변수
    public int damage;                       // 장애물 피해량 변수

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();                 // 스프라이트 랜더러 컴포넌트 가져오기

        // 장애물 종류에 따라 장애물 이미지를 넣어주고 피해량 설정
        if (obstacleType == ObstacleType.SyntaxErrorBox)                 // 장애물이 SyntaxErrorBox 일 때
        {
            spriteRenderer.sprite = spriteSyntaxError;                   // 이미지와 피해량 설정
            damage = 1;
        }
        else if (obstacleType == ObstacleType.CompileErrorWall)          // 장애물이 CompileErrorWall 일 때
        {
            spriteRenderer.sprite = spriteCompileError;                  // 이미지와 피해량 설정
            damage = 2;
        }
        else if (obstacleType == ObstacleType.RedLineTrap)               // 장애물이 RedLineTrap 일 때
        {
            spriteRenderer.sprite = spriteRedLine;                       // 이미지와 피해량 설정
            damage = 3;
        }
        else if (obstacleType == ObstacleType.UnhandledExceptionBox)     // 장애물이 UnhandledExceptionBox 일 때
        {
            spriteRenderer.sprite = spriteUnhandledException;            // 이미지와 피해량 설정
            damage = 4;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)                  // 장애물이 다른 게임 오브젝트와 충돌했을 때
    {
        if (collision.CompareTag("Player"))                             // 부딪친 게임 오브젝트의 태그가 플레이어라면
        {
            Debug.Log("플레이어와 충돌");                               // 충돌 메서드 작동 확인용 메시지

            PlayerModel player = collision.GetComponent<PlayerModel>(); // 부딪친 게임 오브젝트(플레이어)의 컴포넌트 중
                                                                        // "PlayerModel.cs" 컴포넌트를 받아서 player 변수에 저장

            if (player != null)                                         // 예외 처리 (부딪친 게임 오브젝트(플레이어)에 PlayerModel 스크립트가 있는지 확인)
            {
                // float originalSpeed = player.speed;                     // 맞은 시점의 속도를 저장하는 변수 
                // player.hp -= damage;                                    // hp 감소
                // StartCoroutine(SpeedDown(player, originalSpeed));       // SpeedDown 코루틴 호출
            }
        }
    }

    IEnumerator SpeedDown(PlayerModel player, float originalSpeed)     // 장애물에 부딪쳤을 떄 잠시동안 속도를 감소 시키는 코루틴 함수 
    {                                                                  // 속도가 감소 됐다가 서서히 원래 속도로 돌아온다.
        Debug.Log("속도 감소 코루틴");                                 // 속도 감소 메서드 작동 확인용 메시지
        // player.speed = 0.5f;
        yield return new WaitForSeconds(0.5f);
        // player.speed = originalSpeed;

    }
}
