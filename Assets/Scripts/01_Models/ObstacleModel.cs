using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ObstacleType    // ��ֹ� ����
{
    SyntaxErrorBox,         // ���� ���� ��� ��ֹ�
    CompileErrorWall,       // ������ ���� ����� �� ��ֹ�
    RedLineTrap,            // ���� ���� ���� Ʈ�� ��ֹ�
    UnhandledExceptionBox   // ó������ ���� ���� �ڽ� ��ֹ�
}

//��ֹ� �ϳ��� �����͸� �����ϴ� Ŭ����
//�����Ϳ� ���̱� x, �ڵ�θ� ���
//MonoBehaviour ����
public class ObstacleModel : MonoBehaviour
{
    ////��ֹ��� ������ �����ϴ� �Ӽ�
    ////�����ڿ����� ���� ������ �� �־ ������ ������ ����
    //public ObstacleType Type {  get; private set; }
    ////�� ��ֹ��� �÷��̾�� �� ���ط�
    //public int Damage {  get; private set; }

    ////ObstacleModel�� ������ �� ��ֹ��� ������ �޾Ƽ� ���� �����͸� �ڵ����� �ʱ�ȭ
    ////��, �������� �̰� SyntaxErrorBox�Դϴ� ��� �ָ� ��~ �׷� �������� 1�̳� �ϰ� �ڵ� ���õǴ� ����
    //public ObstacleModel(ObstacleType obstacleType)
    //{
    //    //���޹��� type���� ������ Type ������Ƽ�� ����
    //    //���� �� �����͸� ������� ���� ����, ������ ���� � ��� ����
    //    obstacleType = Type;

    //    //��ֹ� ������ ���� �������� �ڵ����� �������ִ� �ڵ�
    //    Damage = Type switch
    //    {
    //        //ObstacleType�� ������ �������� 1�� ����
    //        ObstacleType.SyntaxErrorBox => 1,
    //        //2�� ����
    //        ObstacleType.CompileErrorWall => 2,
    //        //3���� ����
    //        ObstacleType.RedLineTrap => 3,
    //        //4�� ����
    //        ObstacleType.UnhandledExceptionBox => 4,
    //        _ => 0 //�������� ���� ���̸� 0���� �����ϰ� ó��
    //    };
    //}
    // ��������Ʈ �̹��� �޾ƿ���
    public Sprite spriteSyntaxError;         // SyntaxError �̹���
    public Sprite spriteCompileError;        // CompileError �̹���
    public Sprite spriteRedLine;             // RedLine �̹���
    public Sprite spriteUnhandledException;  // UnhandledException �̹���

    SpriteRenderer spriteRenderer;           // ��������Ʈ ������ ������Ʈ�� �޾ƿ� ����

    public ObstacleType obstacleType;        // ��ֹ� ���� ����
    public int damage;                       // ��ֹ� ���ط� ����

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();                 // ��������Ʈ ������ ������Ʈ ��������

        // ��ֹ� ������ ���� ��ֹ� �̹����� �־��ְ� ���ط� ����
        if (obstacleType == ObstacleType.SyntaxErrorBox)                 // ��ֹ��� SyntaxErrorBox �� ��
        {
            spriteRenderer.sprite = spriteSyntaxError;                   // �̹����� ���ط� ����
            damage = 1;
        }
        else if (obstacleType == ObstacleType.CompileErrorWall)          // ��ֹ��� CompileErrorWall �� ��
        {
            spriteRenderer.sprite = spriteCompileError;                  // �̹����� ���ط� ����
            damage = 2;
        }
        else if (obstacleType == ObstacleType.RedLineTrap)               // ��ֹ��� RedLineTrap �� ��
        {
            spriteRenderer.sprite = spriteRedLine;                       // �̹����� ���ط� ����
            damage = 3;
        }
        else if (obstacleType == ObstacleType.UnhandledExceptionBox)     // ��ֹ��� UnhandledExceptionBox �� ��
        {
            spriteRenderer.sprite = spriteUnhandledException;            // �̹����� ���ط� ����
            damage = 4;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)                  // ��ֹ��� �ٸ� ���� ������Ʈ�� �浹���� ��
    {
        PlayerModel player = collision.GetComponent<PlayerModel>();     // �ε�ģ ���� ������Ʈ(�÷��̾�)�� ������Ʈ ��
                                                                        // "PlayerModel.cs" ������Ʈ�� �޾Ƽ� player ������ ����


        //if (player == true)                                             // �ε�ģ ���� ������Ʈ�� �±װ� �÷��̾���
        //{
        //    Debug.Log("�÷��̾�� �浹");                               // �浹 �޼��� �۵� Ȯ�ο� �޽���


        //    if (player != null)                                         // ���� ó�� (�ε�ģ ���� ������Ʈ(�÷��̾�)�� PlayerModel ��ũ��Ʈ�� �ִ��� Ȯ��)
        //    {
        //        float originalSpeed = player.speed;                     // ���� ������ �ӵ��� �����ϴ� ���� 
        //        player.hp -= damage;                                    // hp ����
        //        StartCoroutine(SpeedDown(player, originalSpeed));       // SpeedDown �ڷ�ƾ ȣ��
        //    }
        //}
    }

    IEnumerator SpeedDown(PlayerModel player, float originalSpeed)     // ��ֹ��� �ε����� �� ��õ��� �ӵ��� ���� ��Ű�� �ڷ�ƾ �Լ� 
    {                                                                  // �ӵ��� ���� �ƴٰ� ������ ���� �ӵ��� ���ƿ´�.
        Debug.Log("�ӵ� ���� �ڷ�ƾ");                                 // �ӵ� ���� �޼��� �۵� Ȯ�ο� �޽���
        // player.speed = 0.5f;
        yield return new WaitForSeconds(0.5f);
        // player.speed = originalSpeed;
        ///
    }
}
