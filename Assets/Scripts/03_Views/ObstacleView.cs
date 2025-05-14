using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장애물 외형을 처리하는 View 계층 클래스
/// - 모델 정보에 따라 Sprite를 설정
/// - 장애물 외형은 오직 ObstacleModel의 Type으로 결정됨
/// </summary>
public class ObstacleView : MonoBehaviour
{
    [Header("장애물 스프라이트")]
    [SerializeField] private Sprite spriteSyntaxError;          // 높은 장애물 Sprite
    [SerializeField] private Sprite spriteCompileError;         // 위쪽 장애물 Sprite
    [SerializeField] private Sprite spriteRedLine;              // 낮은 장애물 Sprite
    [SerializeField] private Sprite spriteUnhandledException;   // 낙사 장애물 Sprite

    private SpriteRenderer spriteRenderer;                      // 장애물 외형 표현 컴포넌트
    private static Dictionary<ObstacleType, Sprite> spriteMap;  // Sprite 매핑 테이블 (정적 캐시)

    private void Awake()
    {
        // SpriteRenderer가 없다면 자동으로 검색
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("[ObstacleView] SpriteRenderer가 없습니다.");

        // 정적 딕셔너리 초기화 (최초 1회만 수행)
        if (spriteMap == null)
        {
            spriteMap = new Dictionary<ObstacleType, Sprite>
            {
                { ObstacleType.RedLineTrap, spriteRedLine },
                { ObstacleType.SyntaxErrorBox, spriteSyntaxError },
                { ObstacleType.CompileErrorWall, spriteCompileError },
                { ObstacleType.UnhandledExceptionBox, spriteUnhandledException }
            };
        }
    }

    /// <summary>
    /// 모델의 Type을 기반으로 외형(SPRITE) 설정
    /// </summary>
    public void SetupView(ObstacleModel model)
    {
        if (spriteRenderer == null || model == null)
            return;

        // 모델 타입에 따라 스프라이트 설정 (딕셔너리 기반)
        if (spriteMap.TryGetValue(model.Type, out var sprite))
        {
            spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"[ObstacleView] 알 수 없는 ObstacleType: {model.Type}");
        }
    }
}
