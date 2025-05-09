using System.Collections;
using System.Collections.Generic;
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

    SpriteRenderer spriteRenderer;  // 스프라이트 랜더러 컴포넌트를 받아올 변수

    public ObstacleType obstacleType; // 장애물 종류 변수
    public int damage;               // 장애물 피해량 변수

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    // 스프라이트 랜더러 컴포넌트 가져오기

        // 장애물 종류에 따라 장애물 이미지를 넣어주고 피해량 설정
        if (obstacleType == ObstacleType.SyntaxErrorBox)
        {
            spriteRenderer.sprite = spriteSyntaxError;
            damage = 1;
        }
        else if (obstacleType == ObstacleType.CompileErrorWall)
        {
            spriteRenderer.sprite = spriteCompileError;
            damage = 2;
        }
        else if (obstacleType == ObstacleType.RedLineTrap)
        {
            spriteRenderer.sprite = spriteRedLine;
            damage = 3;
        }
        else if (obstacleType == ObstacleType.UnhandledExceptionBox)
        {
            spriteRenderer.sprite = spriteUnhandledException;
            damage = 4;
        }

        Debug.Log("이 장애물의 종류는: " + obstacleType);   // 테스트 출력 메시지
        Debug.Log("데미지는: " + damage);

        

    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
