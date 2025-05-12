using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ش� ��ũ��Ʈ�� ���̴� ���� Unity�� �ڵ����� �� ���ӿ�����Ʈ�� Collider2D ������Ʈ�� �߰��ϵ��� �����ϱ�
[RequireComponent(typeof(Collider2D))]
public class ObstacleDamage : MonoBehaviour
{
    private int damage;    // ��ֹ��� �÷��̾�� �� ���ط��� ������ ����


    private void Awake()
    {
        // ��ֹ� ���� ������Ʈ�� �������� �� damage�� �� ��ֹ� Ÿ�Կ� �´� Damage �־��ֱ� 
        if (gameObject.name.Contains("RedLine"))
            damage = new ObstacleModel(ObstacleType.RedLineTrap).Damage;
        else if (gameObject.name.Contains("Syntax"))
            damage = new ObstacleModel(ObstacleType.SyntaxErrorBox).Damage;
        else if (gameObject.name.Contains("Compile"))
            damage = new ObstacleModel(ObstacleType.CompileErrorWall).Damage;
        else
            damage = 1; // ��ֹ� ������ ������ 1�� ó��
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ��� ������Ʈ�� Player �±װ� �پ� �ִ��� Ȯ��
        if (collision.collider.CompareTag("Player"))
        {
            // �±װ� Player��� �ش� ������Ʈ�� PlayerController ������Ʈ�� ��������
            PlayerController playerCtrl = collision.collider.GetComponent<PlayerController>();
            if (playerCtrl != null) // ���� ó�� 
            {
                // PlayerController�� TakeDamage(int) �޼��带 ȣ���� �������� ����
                playerCtrl.TakeDamage(damage);
            }
        }
    }
}
