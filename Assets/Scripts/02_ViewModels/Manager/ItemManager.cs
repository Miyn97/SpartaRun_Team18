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
        public int preloadCount;     // 새로 추가
    }

    [Header("아이템 설정")]
    [SerializeField] private List<ItemPrefab> itemPrefabs;   // 프리팹 등록 리스트
    [SerializeField] private int poolSize = 8;               // 타입별 풀 개수

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
        if (poolDict.TryGetValue(type, out var queue))
        {
            while (queue.Count > 0)
            {
                var obj = queue.Dequeue();

                // Destroy 되었거나 존재하지 않으면 다음 객체 확인
                if (obj == null || obj.Equals(null))
                {
                    Debug.LogWarning($"[ItemManager] {type} 아이템이 Destroy 상태입니다. 스킵.");
                    continue;
                }

                obj.SetActive(true);
                return obj;
            }
        }

        //모든 객체가 Destroy 되었거나 없을 경우 새로 생성
        var prefab = itemPrefabs.Find(p => p.type == type).prefab;
        if (prefab != null)
        {
            var newObj = Instantiate(prefab, transform);
            newObj.SetActive(true);
            //Debug.LogWarning($"[ItemManager] {type} 새 객체를 생성했습니다. 풀 부족 대비.");
            return newObj;
        }

        Debug.LogError($"[ItemManager] {type} 프리팹을 찾을 수 없습니다!");
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
