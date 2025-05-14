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
public class DeathTrigger : MonoBehaviour
{
    private StageManager stageManager;

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.name == "DeathGround")
        {
            GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
        }
    }
}