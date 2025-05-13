using System.Collections.Generic;
using UnityEngine;

// ViewModel ������ Ŭ����: �� �����͸� ������� ���� ûũ�� �����ϴ� ������ ���
public class MapChunkSelector
{
    // ���ο��� ����� ûũ ����Ʈ (�б� ����)
    private readonly List<ChunkModel> _chunks;

    // �� ����ġ�� ���� ����Ͽ� �����صα� (GC �ּ�ȭ�� ����)
    private readonly float _totalWeight;

    // �����ڿ��� chunk ����Ʈ�� ���޹ް� �� ����ġ�� �����
    public MapChunkSelector(List<ChunkModel> chunks)
    {
        _chunks = chunks; // ���޹��� ûũ ����Ʈ ����
        _totalWeight = CalculateTotalWeight(_chunks); // �� ����ġ ��� �� ĳ��
    }

    // �ܺο��� ȣ�� ������ �Լ�: ������ ûũ GameObject�� ��ȯ
    public GameObject GetRandomChunk()
    {
        float random = Random.Range(0f, _totalWeight); // 0���� �� ����ġ ���̿��� ���� �� ����
        float cumulative = 0f; // ���� ����ġ ����

        // ��ȸ�ϸ鼭 ���� ����ġ�� ���Ͽ� ���� ������ Ŀ���� �ش� ������ ��ȯ
        foreach (var chunk in _chunks)
        {
            cumulative += chunk.weight; // ����ġ ����
            if (random <= cumulative) // �������� �������� ũ�� �ش� ������ ��ȯ
            {
                return chunk.chunkPrefab;
            }
        }

        // ���� ó��: ����Ʈ�� ������� ������ ù ��° ������, ��������� null
        return _chunks.Count > 0 ? _chunks[0].chunkPrefab : null;
    }

    // �� ����ġ�� ����ϴ� ���� �Լ�
    private float CalculateTotalWeight(List<ChunkModel> chunks)
    {
        float total = 0f; // �ʱ�ȭ
        foreach (var c in chunks) // ��� ûũ�� ��ȸ�ϸ�
            total += c.weight; // ����ġ�� ����
        return total;
    }
}
