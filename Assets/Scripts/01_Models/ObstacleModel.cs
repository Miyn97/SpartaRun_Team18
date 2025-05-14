using System.Collections.Generic; // Dictionary 사용을 위한 네임스페이스

// 장애물 종류 enum 정의
public enum ObstacleType
{
    RedLineTrap,          // 낮은 장애물 (점프 회피)
    SyntaxErrorBox,       // 높은 장애물 (더블 점프 회피)
    CompileErrorWall,     // 위쪽 장애물 (슬라이드 회피)
    UnhandledExceptionBox // 낙사 지형 (추후 확장 가능)
}

// 장애물 회피 방법 enum 정의
public enum AvoidType
{
    Jump,                 // 점프
    DoubleJump,           // 더블 점프
    Slide                 // 슬라이드
}

// 장애물 하나의 데이터만 담는 순수 모델 클래스 (MonoBehaviour 제거)
public class ObstacleModel
{
    public ObstacleType Type { get; private set; } // 장애물 종류 저장
    public AvoidType Avoid { get; private set; }   // 회피 방법 저장
    public int Damage { get; private set; }        // 피해량 저장

    // 장애물 종류별 회피 방법을 담은 static readonly Dictionary
    private static readonly Dictionary<ObstacleType, AvoidType> AvoidTypeMap = new()
    {
        { ObstacleType.RedLineTrap, AvoidType.Jump },          // 낮은 장애물
        { ObstacleType.SyntaxErrorBox, AvoidType.DoubleJump }, // 높은 장애물
        { ObstacleType.CompileErrorWall, AvoidType.Slide },    // 위쪽 장애물
        { ObstacleType.UnhandledExceptionBox, AvoidType.Jump } // 낙사 지형
    };

    // 장애물 종류별 피해량을 담은 static readonly Dictionary
    private static readonly Dictionary<ObstacleType, int> DamageMap = new()
    {
        { ObstacleType.RedLineTrap, 1 },           // 낮은 장애물 데미지
        { ObstacleType.SyntaxErrorBox, 1 },        // 높은 장애물 데미지
        { ObstacleType.CompileErrorWall, 1 },      // 위쪽 장애물 데미지
        { ObstacleType.UnhandledExceptionBox, 6 } // 낙사 지형 (즉사)
    };

    // 생성자 - 장애물 종류를 받아 데이터 자동 초기화
    public ObstacleModel(ObstacleType obstacleType)
    {
        Type = obstacleType; // 전달받은 타입 저장

        // Dictionary에서 매핑값 가져오기 - 정의되지 않은 경우 기본값 처리
        Damage = DamageMap.TryGetValue(Type, out int dmg) ? dmg : 0; // 존재하면 데미지, 아니면 0
        Avoid = AvoidTypeMap.TryGetValue(Type, out AvoidType at) ? at : 0; // 존재하면 회피 방법, 아니면 기본값
    }
}
