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
        public float weight = 1f;
    }

    [Header("没农 橇府普 府胶飘")]
    public List<ChunkEntry> chunks = new();

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

