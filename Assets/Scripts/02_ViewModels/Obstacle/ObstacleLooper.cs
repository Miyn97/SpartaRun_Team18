using UnityEngine;

/// <summary>
/// 장애물이 특정 영역을 벗어나면 ObstacleSpawner에 숨김 요청
/// </summary>
public class ObstacleLooper : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // 장애물 관련 태그인 경우만 처리 (태그 2종 통합 가능)
        if (col.CompareTag("Obstacle") || col.CompareTag("ObstacleGround"))
        {
            var spawner = FindObjectOfType<ObstacleSpawner>(); // 추후 DI로 교체 가능
            if (spawner != null)
                spawner.OnObstacleHidden(col.gameObject);
        }
    }
}
