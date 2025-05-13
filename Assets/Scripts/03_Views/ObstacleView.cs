using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Obstacle
{
    SyntaxError,
    CompileError,
    RedLine,
    // 미사용 UnhandledException
}


public class ObstacleView : MonoBehaviour
{

    [Header("장애물 스프라이트")]
    public Sprite spriteSyntaxError;         // SyntaxError 이미지
    public Sprite spriteCompileError;        // CompileError 이미지
    public Sprite spriteRedLine;             // RedLine 이미지
    // 미사용 public Sprite spriteUnhandledException;  // UnhandledException 이미지

    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)  //spriteRenderer의 노쇼 방지
        {
            Debug.Log("ObstacleView : spriteRenderer가 없습니다.");
        }
    }


    // Obstacle(enum)을 기반으로 Sprite를 설정하는 메서드
    public void SetobstacleSprite(Obstacle obs)
    {
        switch (obs)
        {
            case Obstacle.SyntaxError:
                spriteRenderer.sprite = spriteSyntaxError;
                break;
            case Obstacle.CompileError:
                spriteRenderer.sprite = spriteCompileError;
                break;
            case Obstacle.RedLine:
                spriteRenderer.sprite = spriteRedLine;
                break;
            //case Obstacle.UnhandledException:
            //  spriteRenderer.sprite = spriteUnhandledException;
                break;
            default:
                Debug.LogWarning($"알수 없는 장애물 {obs}");
                break;
        }
    }

    // ObstacleModel  내 ObstacleType(enum)을 기반으로 Sprite를 설정하는 메서드
    // View 계층에서 모델 정보를 받아 장애물의 외형을 초기화하는 역할을 수행
    public void SetupView(ObstacleModel model)
    {
        if (spriteRenderer == null) // spriteRenderer의 노쇼 방지
            return;

        switch (model.Type)
        {
            case ObstacleType.RedLineTrap:
                spriteRenderer.sprite = spriteRedLine;
                break;
            case ObstacleType.SyntaxErrorBox:
                spriteRenderer.sprite = spriteSyntaxError;
                break;
            case ObstacleType.CompileErrorWall:
                spriteRenderer.sprite = spriteCompileError;
                break;
            default:
                Debug.LogWarning($"[ObstacleView] Unknown ObstacleType: {model.Type}");
                break;
        }
    }

}
