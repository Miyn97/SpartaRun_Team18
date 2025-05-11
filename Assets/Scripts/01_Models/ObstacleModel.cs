using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ObstacleType    // ��ֹ� ���� enum
{
    RedLineTrap,            // ���� ���� ���� Ʈ�� ��ֹ�     (���� ��ֹ�)    (ȸ�� ���: ����)
    SyntaxErrorBox,         // ���� ���� ��� ��ֹ�          (���� ��ֹ�)    (ȸ�� ���: ���� ����)
    // UnhandledExceptionBox,  // ó������ ���� ���� �ڽ� ��ֹ� (������)         (ȸ�� ���: ����)
    CompileErrorWall,       // ������ ���� ����� �� ��ֹ�   (���� ��ֹ�)    (ȸ�� ���: �����̵�)

}

public enum AvoidType       // ��ֹ� ȸ�� ��� eum
{
    Jump,                   // ������ ���ϴ� ��ֹ�
    DoubleJump,             // ���� ������ ���ϴ� ��ֹ�
    Slide                   // �����̵�� ���ϴ� ��ֹ�
}  

// ��ֹ� �ϳ��� �����͸� �����ϴ� Ŭ����
// �����Ϳ� ���̱� x, �ڵ�θ� ���
// MonoBehaviour ����
public class ObstacleModel
{
    public ObstacleType Type {  get; private set; }     // ��ֹ��� ������ �����ϴ� �Ӽ�
                                                        // �����ڿ����� ���� ������ �� �־ ������ ������ ����

    public AvoidType Avoid {  get; private set; }       // ��ֹ��� ȸ�� ����� �����ϴ� �Ӽ�

    public int Damage {  get; private set; }            // �� ��ֹ��� �÷��̾�� �� ���ط�
    
    public ObstacleModel(ObstacleType obstacleType)     // !!������!! ObstacleModel�� ������ �� ��ֹ��� ������ �޾Ƽ� ���� �����͸� �ڵ����� �ʱ�ȭ
    {                                                   // ��, �������� �̰� SyntaxErrorBox�Դϴ� ��� �ָ� ��~ �׷� �������� 1�̳� �ϰ� �ڵ� ���õǴ� ����
        
        Type = obstacleType;                            // ���޹��� type���� ������ Type ������Ƽ�� ����
                                                        // ���� �� �����͸� ������� ���� ����, ������ ���� � ��� ����

         Damage = Type switch                           // ��ֹ� ������ ���� �������� �ڵ����� �������ִ� �ڵ�
         {

             ObstacleType.RedLineTrap => 1,                             // ���� ��ֹ�       (������: 1)

             ObstacleType.SyntaxErrorBox => 1,                          // ���� ��ֹ�       (������: 2)

             // ObstacleType.UnhandledExceptionBox => 4,                // ������ ��ֹ�     (������: 4 (���))

             ObstacleType.CompileErrorWall => 1,                        // ���� ��ֹ�       (������: 1)
             _ => 0                                                     // �������� ���� ���̸� 0���� �����ϰ� ó��
         };

        Avoid = Type switch
        {
            
            ObstacleType.RedLineTrap => AvoidType.Jump,                 // ���� ��ֹ�       (ȸ�� ���: ����)

            ObstacleType.SyntaxErrorBox => AvoidType.DoubleJump,        // ���� ��ֹ�       (ȸ�� ���: ���� ����)

            // ObstacleType.UnhandledExceptionBox => AvoidType.Jump,    // ������ ��ֹ�     (ȸ�� ���: ����)

            ObstacleType.CompileErrorWall => AvoidType.Slide,           // ���� ��ֹ�       (ȸ�� ���: �����̵�)
            _ => 0                                                      // �������� ���� ���̸� 0���� �����ϰ� ó��

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
