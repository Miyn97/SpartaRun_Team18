using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLooper : MonoBehaviour
{
    // ObstacleLooper�� ��ֹ��� ������ ��ֹ��� ������ �Ű��ֱ�
    private void OnTriggerEnter2D(Collider2D collision)     
    {
        if (collision.CompareTag("Obstacle"))
        {
            ObstacleManager.Instance.RepositionObstacle(collision.gameObject);
        }
    }
}
