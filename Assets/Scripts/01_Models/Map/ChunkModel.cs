using System;
using UnityEngine;

// [System.Serializable]�� �ٿ��� �ν����Ϳ��� ����Ʈ ���·� ���� �����ϵ��� ����
[Serializable]
public class ChunkModel
{
    // �ش� ûũ�� ������ ������Ʈ
    public GameObject chunkPrefab;

    // �� �������� ���õ� Ȯ���� ������ �ִ� ����ġ. �⺻�� 1�� ����
    public float weight = 1f;
}
