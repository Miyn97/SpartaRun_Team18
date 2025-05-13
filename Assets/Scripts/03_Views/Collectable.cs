using UnityEngine;

/// <summary>
/// Player�� �浹���� �� ItemEntity�� Apply()�� ȣ��
/// (MVVM ���������� View ���Ҹ� ��)
/// </summary>
[RequireComponent(typeof(ItemEntity))]
public class Collectable : MonoBehaviour
{
    private ItemEntity itemEntity;

    private void Awake()
    {
        itemEntity = GetComponent<ItemEntity>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            itemEntity.Apply(); // ȿ�� ���� (���������� ItemEffectService ȣ��)
        }
    }
}
