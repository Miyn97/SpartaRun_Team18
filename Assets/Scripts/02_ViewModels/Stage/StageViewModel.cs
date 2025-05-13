using System.Collections.Generic;
using UnityEngine;

// 스테이지 로직을 담당하는 ViewModel 클래스
public class StageViewModel
{
    private readonly MapChunkSelector _selector; // 랜덤 청크 선택기
    private readonly List<ChunkModel> _chunks; // 청크 원본 리스트

    // 생성자에서 데이터와 선택기를 주입받음
    public StageViewModel(MapDataAsset mapData)
    {
        _chunks = mapData.chunks; // 원본 참조용
        _selector = new MapChunkSelector(_chunks); // 셀렉터 초기화
    }

    // 다음에 생성할 청크를 선택해서 반환
    public GameObject GenerateNextChunk()
    {
        return _selector.GetRandomChunk(); // ViewModel에서 랜덤 선택
    }

    // 인스턴스 이름으로 원본 프리팹을 찾음
    public GameObject FindOriginalPrefab(GameObject instance)
    {
        foreach (var chunk in _chunks)
        {
            if (instance.name.StartsWith(chunk.chunkPrefab.name)) // 이름 기반으로 비교
                return chunk.chunkPrefab; // 매칭되는 원본 반환
        }

        Debug.LogWarning("원본 프리팹을 찾지 못했습니다."); // 디버그 경고
        return null; // 실패 시 null 반환
    }
}
