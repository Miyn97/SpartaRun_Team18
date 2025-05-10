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

// 장애물 하나의 데이터만 저장하는 클래스
// 에디터에 붙이기 x, 코드로만 사용
// MonoBehaviour 제거
public class ObstacleModel
{
    public ObstacleType Type {  get; private set; }     // 장애물의 종류를 보관하는 속성
                                                        // 생성자에서만 값을 설정할 수 있어서 데이터 안정성 유지
    public int Damage {  get; private set; }            // 이 장애물이 플레이어에게 줄 피해량
    
    public ObstacleModel(ObstacleType obstacleType)     // !!생성자!! ObstacleModel이 생성될 때 장애물의 종류를 받아서 내부 데이터를 자동으로 초기화
    {                                                   // 즉, 누군가가 이건 SyntaxErrorBox입니다 라고 주면 아~ 그럼 데미지는 1이네 하고 자동 세팅되는 구조
        
        obstacleType = Type;                            // 전달받은 type값을 내부의 Type 프로퍼티에 저장
                                                        // 이후 이 데이터를 기반으로 외형 결정, 데미지 적용 등에 사용 가능

         Damage = Type switch                           // 장애물 종류에 따라 데미지를 자동으로 지정해주는 코드
         {

             ObstacleType.SyntaxErrorBox => 1,          // ObstacleType가 들어오면 데미지는 1로 저장

             ObstacleType.CompileErrorWall => 2,        // 2로 저장

             ObstacleType.RedLineTrap => 3,             // 3으로 저장

             ObstacleType.UnhandledExceptionBox => 4,   // 4로 저장
             _ => 0                                     // 예상하지 못한 값이면 0으로 안전하게 처리
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

    //IEnumerator SpeedDown(PlayerModel player, float originalSpeed)     // 장애물에 부딪쳤을 떄 잠시동안 속도를 감소 시키는 코루틴 함수 
    //{                                                                  // 속도가 감소 됐다가 서서히 원래 속도로 돌아온다.
    //    Debug.Log("속도 감소 코루틴");                                 // 속도 감소 메서드 작동 확인용 메시지
    //    // player.speed = 0.5f;
    //    yield return new WaitForSeconds(0.5f);
    //    // player.speed = originalSpeed;
    //}
}
