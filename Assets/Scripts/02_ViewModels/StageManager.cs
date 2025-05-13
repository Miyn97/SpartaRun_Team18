using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private MapData testLevelData;

    public GameObject GenNextChunkPrefab()
    {
        return testLevelData.GetRandomChunk();
    }

    public GameObject FindOriginalPrefab(GameObject instance)
    {
        foreach (var entry in testLevelData.chunks)
        {
            if (instance.name.StartsWith(entry.chunkPrefab.name))
                return entry.chunkPrefab;
        }

        Debug.LogWarning("원본 프리팹을 찾지 못했습니다.");
        return null;
    }
}