using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장애물 배치 패턴 생성 및 위치 계산을 전담하는 ViewModel 서비스 클래스
/// SRP 원칙에 따라 장애물 배열과 위치 계산만 처리
/// </summary>
public class ObstaclePatternService
{
    private readonly List<ObstacleType[]> patterns; // 미리 정의된 장애물 패턴 리스트
    private readonly int batchSize;                 // 패턴 내 장애물 개수
    private readonly float groupSpacing;            // 묶음 간 X 간격
    private readonly float innerSpacing;            // 패턴 내 장애물 간격

    public ObstaclePatternService(int batchSize, float groupSpacing, float innerSpacing)
    {
        this.batchSize = batchSize;
        this.groupSpacing = groupSpacing;
        this.innerSpacing = innerSpacing;

        // 미리 정의된 장애물 패턴을 구성
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
    /// 패턴 리스트에서 무작위로 하나를 반환
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
    /// 다음 패턴 묶음의 시작 X 좌표 계산
    /// </summary>
    public float GetStartX(float lastSpawnX)
    {
        return lastSpawnX + groupSpacing;
    }

    /// <summary>
    /// 해당 장애물의 위치 계산 (패턴 시작 X, 인덱스, 타입, Y위치)
    /// </summary>
    public Vector3 GetSpawnPosition(float startX, int index, ObstacleType type, float groundY, float airY)
    {
        float x = startX + index * innerSpacing; // X는 내부 간격만큼 오른쪽
        float y = (type == ObstacleType.CompileErrorWall) ? airY : groundY; // Y는 타입에 따라 달라짐
        return new Vector3(x, y, 0f);
    }

    /// <summary>
    /// 현재 패턴의 마지막 장애물 X 위치 반환 (다음 배치를 위한 기준)
    /// </summary>
    public float GetBatchEndX(float startX)
    {
        return startX + (batchSize - 1) * innerSpacing;
    }
}
