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

// ��ֹ� �ϳ��� �����͸� �����ϴ� Ŭ����
// �����Ϳ� ���̱� x, �ڵ�θ� ���
// MonoBehaviour ����
public class ObstacleModel
{
    public ObstacleType Type {  get; private set; }     // ��ֹ��� ������ �����ϴ� �Ӽ�
                                                        // �����ڿ����� ���� ������ �� �־ ������ ������ ����
    public int Damage {  get; private set; }            // �� ��ֹ��� �÷��̾�� �� ���ط�
    
    public ObstacleModel(ObstacleType obstacleType)     // !!������!! ObstacleModel�� ������ �� ��ֹ��� ������ �޾Ƽ� ���� �����͸� �ڵ����� �ʱ�ȭ
    {                                                   // ��, �������� �̰� SyntaxErrorBox�Դϴ� ��� �ָ� ��~ �׷� �������� 1�̳� �ϰ� �ڵ� ���õǴ� ����
        
        obstacleType = Type;                            // ���޹��� type���� ������ Type ������Ƽ�� ����
                                                        // ���� �� �����͸� ������� ���� ����, ������ ���� � ��� ����

         Damage = Type switch                           // ��ֹ� ������ ���� �������� �ڵ����� �������ִ� �ڵ�
         {

             ObstacleType.SyntaxErrorBox => 1,          // ObstacleType�� ������ �������� 1�� ����

             ObstacleType.CompileErrorWall => 2,        // 2�� ����

             ObstacleType.RedLineTrap => 3,             // 3���� ����

             ObstacleType.UnhandledExceptionBox => 4,   // 4�� ����
             _ => 0                                     // �������� ���� ���̸� 0���� �����ϰ� ó��
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

    //IEnumerator SpeedDown(PlayerModel player, float originalSpeed)     // ��ֹ��� �ε����� �� ��õ��� �ӵ��� ���� ��Ű�� �ڷ�ƾ �Լ� 
    //{                                                                  // �ӵ��� ���� �ƴٰ� ������ ���� �ӵ��� ���ƿ´�.
    //    Debug.Log("�ӵ� ���� �ڷ�ƾ");                                 // �ӵ� ���� �޼��� �۵� Ȯ�ο� �޽���
    //    // player.speed = 0.5f;
    //    yield return new WaitForSeconds(0.5f);
    //    // player.speed = originalSpeed;
    //}
}
