using UnityEngine;

namespace Game.Views
{
    /// <summary>
    /// ���� Trigger�� ���� 'ObstacleGround' �θ� ������Ʈ��
    /// ObstacleSpawner�� ���� ó�� ��û
    /// </summary>
    public class ObstacleLooper : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            // �θ�(ObstacleGround �±�)�� ó��
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
