using UnityEngine;

/// <summary>
/// ������ ���� �ΰ��� ������ �����տ� �ٴ� ������Ʈ
/// �������� ����, �� ������, ȿ�� Ʈ���� ���� ������
/// </summary>
public class ItemEntity : MonoBehaviour
{
    [SerializeField] private ItemEnum itemType; // ������ ����

    private ItemModel model; // ���� ������ �� ����
    private ItemEffectService effectService; // ȿ�� ���� ����
    private ItemManager itemManager; // Ǯ ��ȯ�� ����

    private void Awake()
    {
        // Manager, Service�� �̸� ����Ǿ��ų� �ڵ� Ž��
        itemManager = FindObjectOfType<ItemManager>();
        effectService = FindObjectOfType<ItemEffectService>();
    }

    private void OnEnable()
    {
        // �����۸��� �´� �� �����͸� ���� �Ǵ� ����
        model = CreateModel(itemType);
    }

    private ItemModel CreateModel(ItemEnum type)
    {
        switch (type)
        {
            case ItemEnum.Coin: return new ItemModel(type, 0, 0);
            case ItemEnum.HealPotion: return new ItemModel(type, 0, 2);
            case ItemEnum.SpeedPotion: return new ItemModel(type, 3f, 13f);
            case ItemEnum.GiantPotion: return new ItemModel(type, 3f, 0);
            case ItemEnum.Magnet: return new ItemModel(type, 5f, 0);
            default: return new ItemModel(type);
        }
    }

    /// <summary>
    /// �÷��̾�� �浹���� �� ȿ���� �ߵ���Ŵ
    /// </summary>
    public void Apply()
    {
        Debug.Log($"������ ȿ�� �����: {itemType}");
        GameManager.Instance.AddScore(100);
        effectService.ApplyEffect(model); // ȿ�� ����
        itemManager.ReturnToPool(itemType, gameObject); // Ǯ�� ��ȯ
    }
}
