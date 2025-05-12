using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLooper : MonoBehaviour
{
    // ObstacleLooper에 장애물이 닿으면 장애물을 앞으로 옮겨주기
    private void OnTriggerEnter2D(Collider2D collision) // 장애물에 닿으면 호출되는 함수 
    {
        if (collision.CompareTag("Obstacle"))   // ObstacleLooper 에 닿은 게임 오브젝트의 태그가 Obstacle이라면 
        {
            ObstacleManager.Instance.RepositionObstacle(collision.gameObject); // 장애물을 앞으로 다시 보내는 함수 호출
        }
    }
}
