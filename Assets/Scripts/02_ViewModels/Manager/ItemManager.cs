using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ������Ʈ Ǯ�� �����ϴ� Ŭ���� (MVVM�� Model ����)
/// ������ ����/ȸ���� �� Ŭ���������� ���
/// </summary>
public class ItemManager : MonoBehaviour
{
    [System.Serializable]
    public struct ItemPrefab
    {
        public ItemEnum type;        // ������ ����
        public GameObject prefab;    // �ش� ������ �����Ǵ� ������
    }

    [Header("������ ����")]
    [SerializeField] private List<ItemPrefab> itemPrefabs;   // ������ ��� ����Ʈ
    [SerializeField] private int poolSize = 3;               // Ÿ�Ժ� Ǯ ����

    private Dictionary<ItemEnum, Queue<GameObject>> poolDict = new(); // ������ Ǯ ��ųʸ�

    private void Awake()
    {
        InitializePools(); // Ǯ �ʱ�ȭ
    }

    /// <summary>
    /// Ÿ�Ժ� ������ Ǯ�� �ʱ�ȭ�ϰ� ����
    /// </summary>
    private void InitializePools()
    {
        foreach (var item in itemPrefabs)
        {
            var queue = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, transform); // �θ�� ItemManager
                obj.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
                queue.Enqueue(obj);   // ť�� ���
            }

            poolDict[item.type] = queue; // Ǯ ���
        }
    }

    /// <summary>
    /// ������ Ǯ���� ������
    /// </summary>
    public GameObject Get(ItemEnum type)
    {
        if (poolDict.TryGetValue(type, out var queue) && queue.Count > 0)
        {
            var obj = queue.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        Debug.LogWarning($"[ItemManager] {type} �������� Ǯ�� �����ϴ�.");
        return null;
    }

    /// <summary>
    /// ������ Ǯ�� ��ȯ�ϱ�
    /// </summary>
    public void ReturnToPool(ItemEnum type, GameObject obj)
    {
        obj.SetActive(false);
        poolDict[type].Enqueue(obj);
    }
}
