using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private MapData testLevelData;

    //���� ���� ���������� �����ϵ��� Ȯ��
    public GameObject GenNextChunkPrefab()
    {
        return testLevelData.GetRandomChunk();
    }
}
