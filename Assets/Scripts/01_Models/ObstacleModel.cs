using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType    // ��ֹ� ����
{
    SyntaxErrorBox,         // ���� ���� ��� ��ֹ�
    CompileErrorWall,       // ������ ���� ����� �� ��ֹ�
    RedLineTrap,            // ���� ���� ���� Ʈ�� ��ֹ�
    UnhandledExceptionBox   // ó������ ���� ���� �ڽ� ��ֹ�

}


public class ObstacleModel : MonoBehaviour
{
    // ��������Ʈ �̹��� �޾ƿ���
    public Sprite spriteSyntaxError;         // SyntaxError �̹���
    public Sprite spriteCompileError;        // CompileError �̹���
    public Sprite spriteRedLine;             // RedLine �̹���
    public Sprite spriteUnhandledException;  // UnhandledException �̹���

    SpriteRenderer spriteRenderer;  // ��������Ʈ ������ ������Ʈ�� �޾ƿ� ����

    public ObstacleType obstacleType; // ��ֹ� ���� ����
    public int damage;               // ��ֹ� ���ط� ����

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    // ��������Ʈ ������ ������Ʈ ��������

        // ��ֹ� ������ ���� ��ֹ� �̹����� �־��ְ� ���ط� ����
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

        Debug.Log("�� ��ֹ��� ������: " + obstacleType);   // �׽�Ʈ ��� �޽���
        Debug.Log("��������: " + damage);

        

    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
