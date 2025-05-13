using System.Collections.Generic;
using UnityEngine;

// ViewModel 성격의 클래스: 맵 데이터를 기반으로 랜덤 청크를 선택하는 로직을 담당
public class MapChunkSelector
{
    // 내부에서 사용할 청크 리스트 (읽기 전용)
    private readonly List<ChunkModel> _chunks;

    // 총 가중치를 사전 계산하여 저장해두기 (GC 최소화를 위해)
    private readonly float _totalWeight;

    // 생성자에서 chunk 리스트를 전달받고 총 가중치를 계산함
    public MapChunkSelector(List<ChunkModel> chunks)
    {
        _chunks = chunks; // 전달받은 청크 리스트 저장
        _totalWeight = CalculateTotalWeight(_chunks); // 총 가중치 계산 및 캐싱
    }

    // 외부에서 호출 가능한 함수: 랜덤한 청크 GameObject를 반환
    public GameObject GetRandomChunk()
    {
        float random = Random.Range(0f, _totalWeight); // 0부터 총 가중치 사이에서 랜덤 수 생성
        float cumulative = 0f; // 누적 가중치 변수

        // 순회하면서 누적 가중치를 비교하여 랜덤 수보다 커지면 해당 프리팹 반환
        foreach (var chunk in _chunks)
        {
            cumulative += chunk.weight; // 가중치 누적
            if (random <= cumulative) // 누적값이 랜덤보다 크면 해당 프리팹 반환
            {
                return chunk.chunkPrefab;
            }
        }

        // 예외 처리: 리스트가 비어있지 않으면 첫 번째 프리팹, 비어있으면 null
        return _chunks.Count > 0 ? _chunks[0].chunkPrefab : null;
    }

    // 총 가중치를 계산하는 내부 함수
    private float CalculateTotalWeight(List<ChunkModel> chunks)
    {
        float total = 0f; // 초기화
        foreach (var c in chunks) // 모든 청크를 순회하며
            total += c.weight; // 가중치를 더함
        return total;
    }
}
