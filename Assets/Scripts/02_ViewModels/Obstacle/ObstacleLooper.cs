using UnityEngine;

namespace Game.Views
{
    /// <summary>
    /// 뒤쪽 Trigger에 닿은 'ObstacleGround' 부모 오브젝트를
    /// ObstacleSpawner에 숨김 처리 요청
    /// </summary>
    public class ObstacleLooper : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            // 부모(ObstacleGround 태그)만 처리
            if (col.CompareTag("ObstacleGround"))
            {
                Game.ViewModels.ObstacleSpawner spawner =
                    Game.ViewModels.ObstacleSpawner
                        .FindObjectOfType<Game.ViewModels.ObstacleSpawner>();
                spawner.OnObstacleHidden(col.gameObject);
            }
        }
    }
}
