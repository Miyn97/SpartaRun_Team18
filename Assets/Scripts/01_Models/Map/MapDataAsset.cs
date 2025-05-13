using System.Collections.Generic;
using UnityEngine;

// ScriptableObject로 등록하여 Unity 에디터에서 자산으로 생성 가능
[CreateAssetMenu(menuName = "Map/MapData")]
public class MapDataAsset : ScriptableObject
{
    // 에디터에서 항목 구분을 위해 헤더 표시
    [Header("청크 프리팹 리스트")]

    // 인스펙터에서 직접 편집 가능한 청크 리스트 (ChunkModel로 구성)
    public List<ChunkModel> chunks = new(); // = new() : 리스트 초기화 (C# 최신 문법)
}
