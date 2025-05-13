using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ֹ� ��ġ ���� ���� �� ��ġ ����� �����ϴ� ViewModel ���� Ŭ����
/// SRP ��Ģ�� ���� ��ֹ� �迭�� ��ġ ��길 ó��
/// </summary>
public class ObstaclePatternService
{
    private readonly List<ObstacleType[]> patterns; // �̸� ���ǵ� ��ֹ� ���� ����Ʈ
    private readonly int batchSize;                 // ���� �� ��ֹ� ����
    private readonly float groupSpacing;            // ���� �� X ����
    private readonly float innerSpacing;            // ���� �� ��ֹ� ����

    public ObstaclePatternService(int batchSize, float groupSpacing, float innerSpacing)
    {
        this.batchSize = batchSize;
        this.groupSpacing = groupSpacing;
        this.innerSpacing = innerSpacing;

        // �̸� ���ǵ� ��ֹ� ������ ����
        patterns = new List<ObstacleType[]>
        {
            new[] { ObstacleType.RedLineTrap, ObstacleType.SyntaxErrorBox, ObstacleType.CompileErrorWall },
            new[] { ObstacleType.SyntaxErrorBox, ObstacleType.RedLineTrap, ObstacleType.CompileErrorWall },
            new[] { ObstacleType.CompileErrorWall, ObstacleType.CompileErrorWall, ObstacleType.RedLineTrap },
            new[] { ObstacleType.CompileErrorWall, ObstacleType.RedLineTrap, ObstacleType.SyntaxErrorBox },
            new[] { ObstacleType.RedLineTrap, ObstacleType.RedLineTrap, ObstacleType.CompileErrorWall }
        };
    }

    /// <summary>
    /// ���� ����Ʈ���� �������� �ϳ��� ��ȯ
    /// </summary>
    public ObstacleType[] GetRandomPattern()
    {
        var pattern = new ObstacleType[batchSize];
        for (int i = 0; i < batchSize; i++)
        {
            pattern[i] = GetRandomType();
        }
        return pattern;
        //return patterns[Random.Range(0, patterns.Count)];
    }

    private ObstacleType GetRandomType()
    {
        var values = System.Enum.GetValues(typeof(ObstacleType));
        return (ObstacleType)values.GetValue(Random.Range(0, values.Length));
    }

    /// <summary>
    /// ���� ���� ������ ���� X ��ǥ ���
    /// </summary>
    public float GetStartX(float lastSpawnX)
    {
        return lastSpawnX + groupSpacing;
    }

    /// <summary>
    /// �ش� ��ֹ��� ��ġ ��� (���� ���� X, �ε���, Ÿ��, Y��ġ)
    /// </summary>
    public Vector3 GetSpawnPosition(float startX, int index, ObstacleType type, float groundY, float airY)
    {
        float x = startX + index * innerSpacing; // X�� ���� ���ݸ�ŭ ������
        float y = (type == ObstacleType.CompileErrorWall) ? airY : groundY; // Y�� Ÿ�Կ� ���� �޶���
        return new Vector3(x, y, 0f);
    }

    /// <summary>
    /// ���� ������ ������ ��ֹ� X ��ġ ��ȯ (���� ��ġ�� ���� ����)
    /// </summary>
    public float GetBatchEndX(float startX)
    {
        return startX + (batchSize - 1) * innerSpacing;
    }
}
