using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Obstacle
{
    SyntaxError,
    CompileError,
    RedLine,
    // �̻�� UnhandledException
}


public class ObstacleView : MonoBehaviour
{

    [Header("��ֹ� ��������Ʈ")]
    public Sprite spriteSyntaxError;         // SyntaxError �̹���
    public Sprite spriteCompileError;        // CompileError �̹���
    public Sprite spriteRedLine;             // RedLine �̹���
    // �̻�� public Sprite spriteUnhandledException;  // UnhandledException �̹���

    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)  //spriteRenderer�� ��� ����
        {
            Debug.Log("ObstacleView : spriteRenderer�� �����ϴ�.");
        }
    }


    // Obstacle(enum)�� ������� Sprite�� �����ϴ� �޼���
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
                Debug.LogWarning($"�˼� ���� ��ֹ� {obs}");
                break;
        }
    }

    // ObstacleModel  �� ObstacleType(enum)�� ������� Sprite�� �����ϴ� �޼���
    // View �������� �� ������ �޾� ��ֹ��� ������ �ʱ�ȭ�ϴ� ������ ����
    public void SetupView(ObstacleModel model)
    {
        if (spriteRenderer == null) // spriteRenderer�� ��� ����
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
