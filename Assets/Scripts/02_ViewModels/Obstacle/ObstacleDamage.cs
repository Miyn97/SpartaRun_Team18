using UnityEngine;

/// <summary>
/// ��ֹ��� �÷��̾�� �浹���� �� �������� �ִ� ViewModel ��ũ��Ʈ
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ObstacleDamage : MonoBehaviour
{
    private int damage = 1; // �⺻�� ����

    /// <summary>
    /// �ܺο��� ���� ���Թ޾� ������ ���� (MVVM ���)
    /// </summary>
    public void SetModel(ObstacleModel model)
    {
        damage = model.Damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var playerCtrl = other.GetComponent<PlayerController>();
        if (playerCtrl != null)
        {
            playerCtrl.TakeDamage(damage); // �÷��̾�� ������ ����
        }
    }
}
