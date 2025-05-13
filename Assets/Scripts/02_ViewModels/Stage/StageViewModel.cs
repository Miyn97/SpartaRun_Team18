using System.Collections.Generic;
using UnityEngine;

// �������� ������ ����ϴ� ViewModel Ŭ����
public class StageViewModel
{
    private readonly MapChunkSelector _selector; // ���� ûũ ���ñ�
    private readonly List<ChunkModel> _chunks; // ûũ ���� ����Ʈ

    // �����ڿ��� �����Ϳ� ���ñ⸦ ���Թ���
    public StageViewModel(MapDataAsset mapData)
    {
        _chunks = mapData.chunks; // ���� ������
        _selector = new MapChunkSelector(_chunks); // ������ �ʱ�ȭ
    }

    // ������ ������ ûũ�� �����ؼ� ��ȯ
    public GameObject GenerateNextChunk()
    {
        return _selector.GetRandomChunk(); // ViewModel���� ���� ����
    }

    // �ν��Ͻ� �̸����� ���� �������� ã��
    public GameObject FindOriginalPrefab(GameObject instance)
    {
        foreach (var chunk in _chunks)
        {
            if (instance.name.StartsWith(chunk.chunkPrefab.name)) // �̸� ������� ��
                return chunk.chunkPrefab; // ��Ī�Ǵ� ���� ��ȯ
        }

        Debug.LogWarning("���� �������� ã�� ���߽��ϴ�."); // ����� ���
        return null; // ���� �� null ��ȯ
    }
}
