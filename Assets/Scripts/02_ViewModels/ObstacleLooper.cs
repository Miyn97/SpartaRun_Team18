using UnityEngine;

/// <summary>
/// 뒤쪽에 배치된 Trigger(보이지 않는 영역)에 장애물이 닿으면
/// ObstacleManager에 “숨김 처리”를 요청합니다.
/// </summary>
public class ObstacleLooper : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("ObstacleGround"))
        {
            ObstacleManager.Instance.OnObstacleHidden(col.gameObject);
        }
    }
}