using UnityEngine;

/// <summary>
/// ���ʿ� ��ġ�� Trigger(������ �ʴ� ����)�� ��ֹ��� ������
/// ObstacleManager�� ������ ó������ ��û�մϴ�.
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