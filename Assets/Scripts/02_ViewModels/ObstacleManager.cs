using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)                  // ��ֹ��� �ٸ� ���� ������Ʈ�� �浹���� ��
    {
        PlayerModel player = collision.GetComponent<PlayerModel>();     // �ε�ģ ���� ������Ʈ(���⼭�� �÷��̾�)�� ������Ʈ ��
                                                                        // "PlayerModel.cs" ������Ʈ�� �޾Ƽ� player ������ ����


        /*        if (player == true)*/                                             // �ε�ģ ���� ������Ʈ�� �±װ� �÷��̾���
        {
            Debug.Log("�÷��̾�� �浹");                               // �浹 �޼��� �۵� Ȯ�ο� �޽���


            if (player != null)                                         // ���� ó�� (�ε�ģ ���� ������Ʈ(�÷��̾�)�� PlayerModel ��ũ��Ʈ�� �ִ��� Ȯ��)
            {
                //float originalSpeed = player.speed;                     // ���� ������ �ӵ��� �����ϴ� ���� 
                //player.hp -= damage;                                    // hp ����
                /*                StartCoroutine(SpeedDown(player, originalSpeed)); */      // SpeedDown �ڷ�ƾ ȣ��
            }
        }
    }

    //IEnumerator SpeedDown(PlayerModel player, float originalSpeed)     // ��ֹ��� �ε����� �� ��õ��� �ӵ��� ���� ��Ű�� �ڷ�ƾ �Լ� 
    //{                                                                  // �ӵ��� ���� �ƴٰ� ������ ���� �ӵ��� ���ƿ´�.
    //    Debug.Log("�ӵ� ���� �ڷ�ƾ");                                 // �ӵ� ���� �޼��� �۵� Ȯ�ο� �޽���
    //    // player.speed = 0.5f;
    //    yield return new WaitForSeconds(0.5f);
    //    // player.speed = originalSpeed;
    //}
}
