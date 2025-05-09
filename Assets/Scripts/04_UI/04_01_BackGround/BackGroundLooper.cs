using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLooper : MonoBehaviour
{
    [Header("프리팹 및 설정")]
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float chunkWidth = 20f;
    [SerializeField] private int preloadCount = 5;

    [Header("참조 연결")]
    [SerializeField] private Transform player;

    private Queue<GameObject> chunkPool = new Queue<GameObject>();
    private Queue<GameObject> activeChunks = new Queue<GameObject> ();
    private float nextSpawnX = 0f;

    void Start()
    {
        InitPool(); 

        for(int i = 0; i < preloadCount; i ++)
        {
            SpawnNextChunk();
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(player.position.x + (preloadCount * chunkWidth) > nextSpawnX)
        {
            SpawnNextChunk();
            RemoveOldChunkIfNeeded();
        }
    }

    void InitPool()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject chunk = Instantiate(chunkPrefab, transform);
            chunk.SetActive(false);
            chunkPool.Enqueue(chunk);
        }
    }

    void SpawnNextChunk()
    {
        GameObject chunk = GetChunkFromPool();
        chunk.transform.position = new Vector3(nextSpawnX, 0f, 0f);
        chunk.SetActive(true);
        activeChunks.Enqueue(chunk);
        nextSpawnX += chunkWidth;
    }

    void RemoveOldChunkIfNeeded()
    {
        if (activeChunks.Count > preloadCount)
        {
            GameObject oldChunk = activeChunks.Dequeue();
            ReturnChunkToPool(oldChunk);
        }
    }

    GameObject GetChunkFromPool()
    {
        if(chunkPool.Count > 0)
        {
            return chunkPool.Dequeue();
        }
        else
        {
            Debug.LogWarning("청크 풀이 부족해 새로 생성합니다.");
            return Instantiate(chunkPrefab);
        }
    }

    void ReturnChunkToPool(GameObject chunk)
    {
        chunk.SetActive(false);
        chunkPool.Enqueue(chunk);
    }
}
