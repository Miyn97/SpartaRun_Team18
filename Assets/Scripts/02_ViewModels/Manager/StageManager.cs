using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private MapData testLevelData;

    //추후 여러 스테이지에 대응하도록 확장
    public GameObject GenNextChunkPrefab()
    {
        return testLevelData.GetRandomChunk();
    }
}
