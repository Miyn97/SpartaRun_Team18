using System;
using UnityEngine;

// [System.Serializable]을 붙여서 인스펙터에서 리스트 형태로 편집 가능하도록 설정
[Serializable]
public class ChunkModel
{
    // 해당 청크의 프리팹 오브젝트
    public GameObject chunkPrefab;

    // 이 프리팹이 선택될 확률에 영향을 주는 가중치. 기본값 1로 설정
    public float weight = 1f;
}
