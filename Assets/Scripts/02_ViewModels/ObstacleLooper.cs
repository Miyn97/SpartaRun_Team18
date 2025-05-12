using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLooper : MonoBehaviour
{
    // ObstacleLooper에 장애물이 닿으면 장애물을 앞으로 옮겨주기
    private void OnTriggerEnter2D(Collider2D collision)     
    {
        if (collision.CompareTag("Obstacle"))
        {
            ObstacleManager.Instance.RepositionObstacle(collision.gameObject);
        }
    }
}
