using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 오브젝트 풀을 관리하는 클래스 (MVVM의 Model 역할)
/// 아이템 생성/회수는 이 클래스에서만 담당
/// </summary>
public class ItemManager : MonoBehaviour
{
    [System.Serializable]
    public struct ItemPrefab
    {
        public ItemEnum type;        // 아이템 종류
        public GameObject prefab;    // 해당 종류에 대응되는 프리팹
    }

    [Header("아이템 설정")]
    [SerializeField] private List<ItemPrefab> itemPrefabs;   // 프리팹 등록 리스트
    [SerializeField] private int poolSize = 3;               // 타입별 풀 개수

    private Dictionary<ItemEnum, Queue<GameObject>> poolDict = new(); // 아이템 풀 딕셔너리

    private void Awake()
    {
        InitializePools(); // 풀 초기화
    }

    /// <summary>
    /// 타입별 아이템 풀을 초기화하고 생성
    /// </summary>
    private void InitializePools()
    {
        foreach (var item in itemPrefabs)
        {
            var queue = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, transform); // 부모는 ItemManager
                obj.SetActive(false); // 초기에는 비활성화
                queue.Enqueue(obj);   // 큐에 등록
            }

            poolDict[item.type] = queue; // 풀 등록
        }
    }

    /// <summary>
    /// 아이템 풀에서 꺼내기
    /// </summary>
    public GameObject Get(ItemEnum type)
    {
        if (poolDict.TryGetValue(type, out var queue) && queue.Count > 0)
        {
            var obj = queue.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        Debug.LogWarning($"[ItemManager] {type} 아이템이 풀에 없습니다.");
        return null;
    }

    /// <summary>
    /// 아이템 풀에 반환하기
    /// </summary>
    public void ReturnToPool(ItemEnum type, GameObject obj)
    {
        obj.SetActive(false);
        poolDict[type].Enqueue(obj);
    }
}
