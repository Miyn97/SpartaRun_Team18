using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장애물 배치 및 게임 흐름에 따라 장애물을 배치하는 ViewModel 역할의 클래스
/// 책임: 패턴 생성 요청, 풀에서 꺼내기, 위치 계산 및 View에 전달
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [Header("풀 설정")]
    [SerializeField] private int countPerType = 5; // 각 장애물 타입마다 몇 개를 미리 풀링해둘지 설정
    [SerializeField] private GameObject redLinePrefab; // 낮은 장애물 프리팹 (점프 회피)
    [SerializeField] private GameObject syntaxErrorBoxPrefab; // 높은 장애물 프리팹 (더블 점프 회피)
    [SerializeField] private GameObject compileErrorWallPrefab; // 위쪽 장애물 프리팹 (슬라이드 회피)


    [Header("패턴 설정")]
    [SerializeField] private int batchSize = 3;         // 패턴 길이
    [SerializeField] private int hiddenThreshold = 3;   // 숨겨진 장애물 기준
    [SerializeField] private float groupSpacing = 7;    // 묶음 간 간격
    [SerializeField] private float innerSpacing = 6f;   // 패턴 내 간격

    [Header("위치 설정")]
    [SerializeField] private Transform player;          // 기준이 되는 플레이어 위치
    [SerializeField] private float spawnAhead = 20f;    // 플레이어 앞까지 생성
    [SerializeField] private float groundObstacleY = -2.5f;
    [SerializeField] private float airObstacleY = 2.5f;

    private ObstaclePatternService patternService;      // 장애물 패턴 및 위치 계산
    private ObstaclePool pool;                          // 장애물 풀링

    private readonly Queue<GameObject> hiddenQueue = new(); // 재사용 대기열
    private float spawnCooldown = 1.5f; // 패턴 생성 간격 (초)
    private float lastSpawnTime = 0f;
    private float lastSpawnX;                           // 마지막 생성 X

    private void Awake()
    {
        // ① 패턴 생성기 초기화: 배치 설정값 전달
        patternService = new ObstaclePatternService(batchSize, groupSpacing, innerSpacing);

        // ② 풀링 클래스 초기화
        pool = new ObstaclePool();

        // ③ 사용할 프리팹과 타입을 매핑한 Dictionary 생성
        var prefabDict = new Dictionary<ObstacleType, GameObject>
        {
            { ObstacleType.RedLineTrap, redLinePrefab },
            { ObstacleType.SyntaxErrorBox, syntaxErrorBoxPrefab },
            { ObstacleType.CompileErrorWall, compileErrorWallPrefab }
        };

        // ④ 풀 초기화: 프리팹 매핑과 개수 전달
        pool.Initialize(prefabDict, countPerType);

        // ⑤ 장애물 배치 시작 기준 좌표 설정
        lastSpawnX = player.position.x;
    }


    private void Start()
    {
        // 초기 거리만큼 장애물 배치
        while (lastSpawnX < player.position.x + spawnAhead)
        {
            SpawnNextBatch();
        }
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime >= 1.0f)
        {
            SpawnNextBatch();
            lastSpawnTime = Time.time;
        }
    }



    public void OnObstacleHidden(GameObject obstacle)
    {
        obstacle.SetActive(false);            // 비활성화 처리
        hiddenQueue.Enqueue(obstacle);        // 재사용 큐에 저장
    }

    private void SpawnNextBatch()
    {
        var pattern = patternService.GetRandomPattern();

        float minSpawnDistance = 20f;

        // lastSpawnX를 기반으로 다음 위치를 점진적으로 갱신
        float startX = Mathf.Max(
            lastSpawnX + groupSpacing,
            player.position.x + minSpawnDistance
        );

        for (int i = 0; i < pattern.Length; i++)
        {
            var type = pattern[i];
            var obstacle = pool.Get(type);

            var model = new ObstacleModel(type);
            obstacle.GetComponent<ObstacleDamage>()?.SetModel(model);
            obstacle.GetComponent<ObstacleView>()?.SetupView(model);

            Vector3 pos = patternService.GetSpawnPosition(
                startX, i, type, groundObstacleY, airObstacleY);
            pos.z = 0;

            obstacle.transform.position = pos;
            obstacle.SetActive(true);
        }

        // ✅ 가장 마지막으로 생성된 장애물 X 좌표 갱신
        lastSpawnX = startX + (pattern.Length - 1) * innerSpacing;
    }





}
