using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ֹ� ������ ó���ϴ� View ���� Ŭ����
/// - �� ������ ���� Sprite�� ����
/// - ��ֹ� ������ ���� ObstacleModel�� Type���� ������
/// </summary>
public class ObstacleView : MonoBehaviour
{
    [Header("��ֹ� ��������Ʈ")]
    [SerializeField] private Sprite spriteSyntaxError;          // ���� ��ֹ� Sprite
    [SerializeField] private Sprite spriteCompileError;         // ���� ��ֹ� Sprite
    [SerializeField] private Sprite spriteRedLine;              // ���� ��ֹ� Sprite
    [SerializeField] private Sprite spriteUnhandledException;   // ���� ��ֹ� Sprite

    private SpriteRenderer spriteRenderer;                      // ��ֹ� ���� ǥ�� ������Ʈ
    private static Dictionary<ObstacleType, Sprite> spriteMap;  // Sprite ���� ���̺� (���� ĳ��)

    private void Awake()
    {
        // SpriteRenderer�� ���ٸ� �ڵ����� �˻�
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("[ObstacleView] SpriteRenderer�� �����ϴ�.");

        // ���� ��ųʸ� �ʱ�ȭ (���� 1ȸ�� ����)
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
    /// ���� Type�� ������� ����(SPRITE) ����
    /// </summary>
    public void SetupView(ObstacleModel model)
    {
        if (spriteRenderer == null || model == null)
            return;

        // �� Ÿ�Կ� ���� ��������Ʈ ���� (��ųʸ� ���)
        if (spriteMap.TryGetValue(model.Type, out var sprite))
        {
            spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"[ObstacleView] �� �� ���� ObstacleType: {model.Type}");
        }
    }
}
