using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장애물 풀링 전담 클래스
/// - 장애물 프리팹을 미리 생성해서 재사용 (GC 최소화)
/// - 각 ObstacleType별 Queue를 보관
/// </summary>
public class ObstaclePool
{
    private readonly Dictionary<ObstacleType, Queue<GameObject>> pool = new(); // 타입별 풀
    private Dictionary<ObstacleType, GameObject> prefabMap; // 타입별 프리팹 매핑
    private int countPerType = 5; // 기본 생성 수 (초기값)

    /// <summary>
    /// 프리팹 초기화 설정
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
                GameObject obj = UnityEngine.Object.Instantiate(prefab); // 풀에 미리 생성
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            pool[type] = queue;
        }
    }

    /// <summary>
    /// 장애물 풀에서 꺼내기 (재사용)
    /// </summary>
    public GameObject Get(ObstacleType type)
    {
        if (!pool.ContainsKey(type) || pool[type].Count == 0)
        {
            Debug.LogWarning($"풀 부족: {type} 추가 생성");
            var newObj = UnityEngine.Object.Instantiate(prefabMap[type]);
            newObj.SetActive(false);
            return newObj;
        }

        return pool[type].Dequeue();
    }

    /// <summary>
    /// 사용 후 되돌리기 (비활성 상태로 넣기)
    /// </summary>
    public void Return(ObstacleType type, GameObject obj)
    {
        obj.SetActive(false); // 재사용 전에 비활성화
        pool[type].Enqueue(obj);
    }
}
