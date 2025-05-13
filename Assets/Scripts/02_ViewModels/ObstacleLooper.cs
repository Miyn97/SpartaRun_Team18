using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLooper : MonoBehaviour
{
    // ObstacleLooper�� ��ֹ��� ������ ��ֹ��� ������ �Ű��ֱ�
    private void OnTriggerEnter2D(Collider2D collision) // ��ֹ��� ������ ȣ��Ǵ� �Լ� 
    {
        if (collision.CompareTag("Obstacle"))   // ObstacleLooper �� ���� ���� ������Ʈ�� �±װ� Obstacle�̶�� 
        {
            ObstacleManager.Instance.RepositionObstacle(collision.gameObject); // ��ֹ��� ������ �ٽ� ������ �Լ� ȣ��
        }
    }
}
