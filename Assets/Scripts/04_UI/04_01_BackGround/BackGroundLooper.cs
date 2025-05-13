using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLooper : MonoBehaviour
{
    [Header("프리팹 및 설정")]
    [SerializeField] private int poolSizePerChunk = 5;
    [SerializeField] private float chunkWidth = 5f;
    [SerializeField] private float chunkSpacing = 0f;
    [SerializeField] private int preloadCount = 5;

    [Header("참조 연결")]
    [SerializeField] private Transform player;
    [SerializeField] private StageManager stageManager;

    private Dictionary<GameObject, Queue<GameObject>> chunkPools = new();
    private Queue<GameObject> activeChunks = new();
    private float nextSpawnX = 0f;

    void Start()
    {
        for (int i = 0; i < preloadCount; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        if (player == null) return;

        if (player.position.x + (preloadCount * chunkWidth) > nextSpawnX)
        {
            SpawnNextChunk();
            RemoveOldChunkIfNeeded();
        }
    }

    void SpawnNextChunk()
    {
        GameObject prefab = stageManager.GenNextChunkPrefab();
        GameObject chunk = GetChunkFromPool(prefab);

        chunk.transform.position = new Vector3(nextSpawnX, 0f, 0f);
        chunk.SetActive(true);
        activeChunks.Enqueue(chunk);

        nextSpawnX += chunkWidth + chunkSpacing;
    }

    void RemoveOldChunkIfNeeded()
    {
        if (activeChunks.Count == 0) return;

        GameObject firstChunk = activeChunks.Peek();
        float chunkEndX = firstChunk.transform.position.x + chunkWidth;

        if (player.position.x - chunkEndX > chunkWidth * 2f)
        {
            GameObject oldChunk = activeChunks.Dequeue();
            ReturnChunkToPool(oldChunk);
        }
    }

    GameObject GetChunkFromPool(GameObject prefab)
    {
        if (!chunkPools.ContainsKey(prefab))
        {
            chunkPools[prefab] = new Queue<GameObject>();

            for (int i = 0; i < poolSizePerChunk; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                chunkPools[prefab].Enqueue(obj);
            }
        }

        Queue<GameObject> pool = chunkPools[prefab];

        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            Debug.LogWarning($"풀 부족: {prefab.name} 추가 생성");
            return Instantiate(prefab, transform);
        }
    }

    void ReturnChunkToPool(GameObject chunk)
    {
        chunk.SetActive(false);
        GameObject prefab = stageManager.FindOriginalPrefab(chunk);

        if (prefab == null)
        {
            Debug.LogWarning("ReturnChunkToPool: 원본 프리팹을 찾지 못했습니다.");
            return;
        }

        if (!chunkPools.ContainsKey(prefab))
        {
            chunkPools[prefab] = new Queue<GameObject>();
        }

        chunkPools[prefab].Enqueue(chunk);
    }
}

