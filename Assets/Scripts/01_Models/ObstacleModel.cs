using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ObstacleType    // 장애물 종류 enum
{
    RedLineTrap,            // 빨간 줄이 쳐진 트랩 장애물     (낮은 장애물)    (회피 방법: 점프)
    SyntaxErrorBox,         // 문법 오류 모양 장애물          (높은 장애물)    (회피 방법: 더블 점프)
    // UnhandledExceptionBox,  // 처리되지 않은 예외 박스 장애물 (구덩이)         (회피 방법: 점프)
    CompileErrorWall,       // 컴파일 에러 모양의 벽 장애물   (위쪽 장애물)    (회피 방법: 슬라이드)

}

public enum AvoidType       // 장애물 회피 방법 eum
{
    Jump,                   // 점프로 피하는 장애물
    DoubleJump,             // 더블 점프로 피하는 장애물
    Slide                   // 슬라이드로 피하는 장애물
}  

// 장애물 하나의 데이터만 저장하는 클래스
// 에디터에 붙이기 x, 코드로만 사용
// MonoBehaviour 제거
public class ObstacleModel
{
    public ObstacleType Type {  get; private set; }     // 장애물의 종류를 보관하는 속성
                                                        // 생성자에서만 값을 설정할 수 있어서 데이터 안정성 유지

    public AvoidType Avoid {  get; private set; }       // 장애물의 회피 방법을 보관하는 속성

    public int Damage {  get; private set; }            // 이 장애물이 플레이어에게 줄 피해량
    
    public ObstacleModel(ObstacleType obstacleType)     // !!생성자!! ObstacleModel이 생성될 때 장애물의 종류를 받아서 내부 데이터를 자동으로 초기화
    {                                                   // 즉, 누군가가 이건 SyntaxErrorBox입니다 라고 주면 아~ 그럼 데미지는 1이네 하고 자동 세팅되는 구조
        
        Type = obstacleType;                            // 전달받은 type값을 내부의 Type 프로퍼티에 저장
                                                        // 이후 이 데이터를 기반으로 외형 결정, 데미지 적용 등에 사용 가능

         Damage = Type switch                           // 장애물 종류에 따라 데미지를 자동으로 지정해주는 코드
         {

             ObstacleType.RedLineTrap => 1,                             // 낮은 장애물       (데미지: 1)

             ObstacleType.SyntaxErrorBox => 1,                          // 높은 장애물       (데미지: 2)

             // ObstacleType.UnhandledExceptionBox => 4,                // 구덩이 장애물     (데미지: 4 (즉사))

             ObstacleType.CompileErrorWall => 1,                        // 위쪽 장애물       (데미지: 1)
             _ => 0                                                     // 예상하지 못한 값이면 0으로 안전하게 처리
         };

        Avoid = Type switch
        {
            
            ObstacleType.RedLineTrap => AvoidType.Jump,                 // 낮은 장애물       (회피 방법: 점프)

            ObstacleType.SyntaxErrorBox => AvoidType.DoubleJump,        // 높은 장애물       (회피 방법: 더블 점프)

            // ObstacleType.UnhandledExceptionBox => AvoidType.Jump,    // 구덩이 장애물     (회피 방법: 점프)

            ObstacleType.CompileErrorWall => AvoidType.Slide,           // 위쪽 장애물       (회피 방법: 슬라이드)
            _ => 0                                                      // 예상하지 못한 값이면 0으로 안전하게 처리

        };
    }


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
