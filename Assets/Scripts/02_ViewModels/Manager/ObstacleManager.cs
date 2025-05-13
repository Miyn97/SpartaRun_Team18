using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance { get; private set; }

    //── [풀 설정 (Model)] ──────────────────────────────────────────
    [Header("풀 설정")]
    [Tooltip("장애물 종류별 초기 풀 크기")]
    public int countPerType = 5;
    public GameObject redLinePrefab;           // RedLineTrap 프리팹
    public GameObject syntaxErrorBoxPrefab;    // SyntaxErrorBox 프리팹
    public GameObject compileErrorWallPrefab;  // CompileErrorWall 프리팹
    public GameObject groundWithGapPrefab;     // 지형+구멍 프리팹

    //── [패턴 설정 (ViewModel)] ─────────────────────────────────────
    [Header("패턴 설정")]
    [Tooltip("한 묶음에 뿌릴 장애물 개수 (패턴 길이)")]
    public int batchSize = 3;
    [Tooltip("숨김 처리된 장애물을 재생성할 때 기준이 될 개수")]
    public int hiddenThreshold = 3;
    [Tooltip("묶음 간 X 간격")]
    public float groupSpacing = 7;
    [Tooltip("패턴 내부 장애물 간격")]
    public float innerSpacing = 6f;

    //── [위치 설정 (ViewModel)] ─────────────────────────────────────
    [Header("위치 설정")]
    [Tooltip("플레이어 Transform (Inspector에서 드래그)")]
    public Transform player;
    [Tooltip("플레이어 앞으로 최소 이만큼 앞에서 뿌리기")]
    public float spawnAhead = 20f;
    public float groundObstacleY = -3f;        // 땅형 장애물 Y
    public float airObstacleY = 2.6f;       // 공중형 장애물 Y

    //── 내부 데이터 구조 ────────────────────────────────────────────
    // 1) 풀: 타입별 비활성 오브젝트 보관
    private readonly Dictionary<ObstacleType, Queue<GameObject>> pool
        = new Dictionary<ObstacleType, Queue<GameObject>>();

    // 2) 숨긴 오브젝트를 쌓아두는 큐
    private readonly Queue<GameObject> hiddenQueue = new Queue<GameObject>();

    // 3) 미리 정의된 6가지 3개짜리 패턴
    private readonly List<ObstacleType[]> patterns = new()
    {
        new[] { ObstacleType.RedLineTrap,      ObstacleType.SyntaxErrorBox,    ObstacleType.CompileErrorWall },
        new[] { ObstacleType.SyntaxErrorBox,   ObstacleType.RedLineTrap,       ObstacleType.CompileErrorWall },
        new[] { ObstacleType.CompileErrorWall, ObstacleType.CompileErrorWall,  ObstacleType.RedLineTrap },
        new[] { ObstacleType.CompileErrorWall, ObstacleType.RedLineTrap,       ObstacleType.SyntaxErrorBox },
        new[] { ObstacleType.RedLineTrap,      ObstacleType.RedLineTrap,       ObstacleType.CompileErrorWall },
        new[] { ObstacleType.SyntaxErrorBox,   ObstacleType.CompileErrorWall,  ObstacleType.RedLineTrap },
        new[] { ObstacleType.GroundWithGap, ObstacleType.GroundWithGap, ObstacleType.CompileErrorWall } // 지형 + 구멍 프리팹
    };

    // 4) 마지막으로 배치된 장애물의 X 좌표
    private float lastSpawnX;

    //──────────────────────────────────────────────────────────────────
    private void Awake()
    {
        Instance = this;    // ← 싱글톤 Instance 초기화
        // ① 풀(Pool) 초기화: 각 타입당 countPerType 개 Instantiate
        foreach (ObstacleType type in Enum.GetValues(typeof(ObstacleType)))
        {
            var q = new Queue<GameObject>();
            GameObject prefab = GetPrefab(type);

            for (int i = 0; i < countPerType; i++)
            {
                var obj = Instantiate(prefab);
                obj.SetActive(false);   // 시작 땐 모두 숨김
                q.Enqueue(obj);
            }

            pool[type] = q;
        }

        // ② 시작 기준 X = 플레이어 현재 X
        lastSpawnX = player.position.x;
    }

    private void Start()
    {
        // ── 1) 게임 시작 시, 플레이어 앞으로 spawnAhead만큼의 거리를
        //       패턴 묶음으로 자동 채워 주도록 반복 호출
        while (lastSpawnX < player.position.x + spawnAhead)
        {
            SpawnNextBatch();
        }

        // ── 2) (원래 있던 주석 처리된 코드 예시)
        // while (lastSpawnX < player.position.x + spawnAhead)
        //     SpawnNextBatch();
    }

    private void Update()
    {
        // hiddenThreshold 기준만 사용 → 거리에 상관없이 “숨김 개수 ≥ 3” 일 때
        if (hiddenQueue.Count >= hiddenThreshold)
        {
            SpawnNextBatch();             // 3개 묶음 패턴 뿌리기
            for (int i = 0; i < batchSize; i++)
                hiddenQueue.Dequeue();    // 해당 수만큼 숨김 큐에서도 제거
        }
    }

    /// <summary>
    /// ObstacleLooper에서 호출: 
    /// 트리거된 Obstacle을 숨기고 재활용 대기열로 보낸다.
    /// </summary>
    public void OnObstacleHidden(GameObject obstacle)
    {
        obstacle.SetActive(false);
        hiddenQueue.Enqueue(obstacle);
    }

    /// <summary>
    /// 패턴 하나(3개)를 뽑아 순서대로 앞쪽에 배치
    /// </summary>
    private void SpawnNextBatch()
    {
        // 1) 무작위 패턴 선택
        var pat = patterns[UnityEngine.Random.Range(0, patterns.Count)];

        // 2) 묶음 시작 X: 마지막 배치 + 패턴 간격
        float startX = lastSpawnX + groupSpacing;

        for (int i = 0; i < pat.Length; i++)
        {
            var type = pat[i];

            // 3) 풀에서 하나 꺼내기 → 순환 큐처럼 사용
            var obj = pool[type].Dequeue();

            // 4) 위치 결정: 묶음 시작점 + 내부 간격 × i
            float x = startX + innerSpacing * i;
            float y = (type == ObstacleType.CompileErrorWall) ? airObstacleY : groundObstacleY;
            obj.transform.position = new Vector3(x, y, 0f);

            // 5) 활성화해서 화면에 보이기
            obj.SetActive(true);

            // 6) 다시 풀 맨 뒤로 넣기 (재활용 준비)
            pool[type].Enqueue(obj);
        }

        // 7) 마지막 배치 X 갱신 (묶음의 마지막 장애물)
        lastSpawnX = startX + innerSpacing * (batchSize - 1);
    }

    //──────────────────────────────────────────────────────────────────
    private GameObject GetPrefab(ObstacleType type) => type switch
    {
        ObstacleType.RedLineTrap => redLinePrefab,
        ObstacleType.SyntaxErrorBox => syntaxErrorBoxPrefab,
        ObstacleType.CompileErrorWall => compileErrorWallPrefab,
        ObstacleType.GroundWithGap => groundWithGapPrefab,
        _ => throw new ArgumentOutOfRangeException()
    };
}