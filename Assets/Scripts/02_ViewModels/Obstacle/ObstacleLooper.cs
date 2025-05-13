using UnityEngine;

/// <summary>
/// ��ֹ��� Ư�� ������ ����� ObstacleSpawner�� ���� ��û
/// </summary>
public class ObstacleLooper : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // ��ֹ� ���� �±��� ��츸 ó�� (�±� 2�� ���� ����)
        if (col.CompareTag("Obstacle") || col.CompareTag("ObstacleGround"))
        {
            var spawner = FindObjectOfType<ObstacleSpawner>(); // ���� DI�� ��ü ����
            if (spawner != null)
                spawner.OnObstacleHidden(col.gameObject);
        }
    }
}
