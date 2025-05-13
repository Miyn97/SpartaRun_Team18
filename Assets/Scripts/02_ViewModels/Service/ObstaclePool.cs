using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ֹ� Ǯ�� ���� Ŭ����
/// - ��ֹ� �������� �̸� �����ؼ� ���� (GC �ּ�ȭ)
/// - �� ObstacleType�� Queue�� ����
/// </summary>
public class ObstaclePool
{
    private readonly Dictionary<ObstacleType, Queue<GameObject>> pool = new(); // Ÿ�Ժ� Ǯ
    private Dictionary<ObstacleType, GameObject> prefabMap; // Ÿ�Ժ� ������ ����
    private int countPerType = 5; // �⺻ ���� �� (�ʱⰪ)

    /// <summary>
    /// ������ �ʱ�ȭ ����
    /// </summary>
    public void Initialize(Dictionary<ObstacleType, GameObject> prefabDict, int countPerType = 5)
    {
        this.prefabMap = prefabDict;
        this.countPerType = countPerType;

        foreach (var kvp in prefabMap)
        {
            var type = kvp.Key;
            var prefab = kvp.Value;

            var queue = new Queue<GameObject>();
            for (int i = 0; i < countPerType; i++)
            {
                GameObject obj = UnityEngine.Object.Instantiate(prefab); // Ǯ�� �̸� ����
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            pool[type] = queue;
        }
    }

    /// <summary>
    /// ��ֹ� Ǯ���� ������ (����)
    /// </summary>
    public GameObject Get(ObstacleType type)
    {
        if (!pool.ContainsKey(type) || pool[type].Count == 0)
        {
            Debug.LogWarning($"Ǯ ����: {type} �߰� ����");
            var newObj = UnityEngine.Object.Instantiate(prefabMap[type]);
            newObj.SetActive(false);
            return newObj;
        }

        return pool[type].Dequeue();
    }

    /// <summary>
    /// ��� �� �ǵ����� (��Ȱ�� ���·� �ֱ�)
    /// </summary>
    public void Return(ObstacleType type, GameObject obj)
    {
        obj.SetActive(false); // ���� ���� ��Ȱ��ȭ
        pool[type].Enqueue(obj);
    }
}
