using System.Collections.Generic;
using UnityEngine;

// ScriptableObject�� ����Ͽ� Unity �����Ϳ��� �ڻ����� ���� ����
[CreateAssetMenu(menuName = "Map/MapData")]
public class MapDataAsset : ScriptableObject
{
    // �����Ϳ��� �׸� ������ ���� ��� ǥ��
    [Header("ûũ ������ ����Ʈ")]

    // �ν����Ϳ��� ���� ���� ������ ûũ ����Ʈ (ChunkModel�� ����)
    public List<ChunkModel> chunks = new(); // = new() : ����Ʈ �ʱ�ȭ (C# �ֽ� ����)
}
