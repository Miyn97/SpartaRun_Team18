using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 배경 청크를 플레이어 위치에 따라 반복 생성/회수하는 클래스
public class BackGroundLooper : MonoBehaviour
{
    [Header("프리팹 및 설정")]
    [SerializeField] private int poolSizePerChunk = 5; // 각 프리팹당 풀링할 오브젝트 수
    [SerializeField] private float chunkWidth = 5f;     // 청크 하나의 너비
    [SerializeField] private float chunkSpacing = 0f;   // 청크 간 간격
    [SerializeField] private int preloadCount = 5;      // 최초 미리 생성할 청크 수

    [Header("참조 연결")]
    [SerializeField] private Transform player;          // 플레이어 위치 추적
    [SerializeField] private StageView stageView;       // 기존 StageManager 대신 StageView로 대체

    // 프리팹별 풀 저장소
    private Dictionary<GameObject, Queue<GameObject>> chunkPools = new();
    // 현재 활성화된 청크 큐
    private Queue<GameObject> activeChunks = new();
    // 다음 청크가 생성될 X 좌표
    private float nextSpawnX = 0f;

    void Start()
    {
        // 게임 시작 시 미리 preloadCount 수 만큼 청크 생성
        for (int i = 0; i < preloadCount; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        if (player == null) return; // 플레이어 미지정 시 스킵

        // 플레이어가 앞으로 나아가면 새로운 청크 생성 필요
        if (player.position.x + (preloadCount * chunkWidth) > nextSpawnX)
        {
            SpawnNextChunk();              // 새 청크 생성
            RemoveOldChunkIfNeeded();     // 오래된 청크 제거
        }
    }

    // 새 청크를 생성해서 배치
    void SpawnNextChunk()
    {
        GameObject prefab = stageView.GenNextChunkPrefab(); // ✅ ViewModel에 위임된 메서드 호출
        GameObject chunk = GetChunkFromPool(prefab);        // 풀에서 꺼내기

        chunk.transform.position = new Vector3(nextSpawnX, 0f, 0f); // 위치 배치
        chunk.SetActive(true);                                     // 활성화
        activeChunks.Enqueue(chunk);                               // 활성 큐에 추가

        nextSpawnX += chunkWidth + chunkSpacing; // 다음 생성 위치 갱신
    }

    // 너무 오래된 청크를 제거
    void RemoveOldChunkIfNeeded()
    {
        if (activeChunks.Count == 0) return;

        GameObject firstChunk = activeChunks.Peek(); // 가장 오래된 청크 확인
        float chunkEndX = firstChunk.transform.position.x + chunkWidth;

        // 플레이어가 해당 청크를 지나친 경우
        if (player.position.x - chunkEndX > chunkWidth * 2f)
        {
            GameObject oldChunk = activeChunks.Dequeue(); // 큐에서 제거
            ReturnChunkToPool(oldChunk);                  // 풀로 반환
        }
    }

    // 풀에서 오브젝트 꺼내기 또는 새로 생성
    GameObject GetChunkFromPool(GameObject prefab)
    {
        if (!chunkPools.ContainsKey(prefab))
        {
            // 해당 프리팹에 대한 풀을 새로 생성
            chunkPools[prefab] = new Queue<GameObject>();

            for (int i = 0; i < poolSizePerChunk; i++)
            {
                GameObject obj = Instantiate(prefab, transform); // 하위로 생성
                obj.SetActive(false);                            // 비활성화
                chunkPools[prefab].Enqueue(obj);                 // 풀에 추가
            }
        }

        Queue<GameObject> pool = chunkPools[prefab];

        // 풀에 남아 있는 경우 꺼내서 반환
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            Debug.LogWarning($"풀 부족: {prefab.name} 추가 생성");
            return Instantiate(prefab, transform); // GC 우려 있으나 예외 처리용
        }
    }

    // 청크를 풀로 반환
    void ReturnChunkToPool(GameObject chunk)
    {
        chunk.SetActive(false); // 비활성화 후

        GameObject prefab = stageView.FindOriginalPrefab(chunk); // ✅ 기존 stageManager → stageView

        if (prefab == null)
        {
            Debug.LogWarning("ReturnChunkToPool: 원본 프리팹을 찾지 못했습니다.");
            return;
        }

        // 해당 프리팹의 풀에 다시 삽입
        if (!chunkPools.ContainsKey(prefab))
        {
            chunkPools[prefab] = new Queue<GameObject>();
        }

        chunkPools[prefab].Enqueue(chunk);
    }
}
