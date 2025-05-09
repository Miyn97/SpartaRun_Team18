using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/MapData")]
public class MapData : ScriptableObject
{
    [System.Serializable]
    public class ChunkEntry
    {
        public GameObject chunkPrefab;
        public float weight = 1f; //등장 확률(가중치)
    }

    [Header("청크 프리팹 리스트")]
    public List<ChunkEntry> chunks = new List<ChunkEntry>();

    //가중치 기반 무작위 청크 반환
    public GameObject GetRandomChunk()
    {
        float totalWeight = 0f;
        foreach (var chunk in chunks)
        {
            totalWeight += chunk.weight;
        }

        float random = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var chunk in chunks)
        {
            cumulative += chunk.weight;
            if (random <= cumulative)
            {
                return chunk.chunkPrefab;
            }
        }

        return chunks.Count > 0 ? chunks[0].chunkPrefab : null;
    }
}
