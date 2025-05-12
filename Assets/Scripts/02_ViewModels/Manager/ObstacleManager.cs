using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    // 각각의 장애물에 대응하는 프리팹을 넣어둘 변수들 (Inspector에서 직접 연결)
    public GameObject redLinePrefab;            // RedLineTrap 프리팹
    public GameObject syntaxErrorBoxPrefab;     // SyntaxErrorBox 프리팹
    public GameObject compileErrorWallPrefab;   // CompileErrorWall 프리팹

    public Transform spawnPoint;                // 장애물이 생성될 위치를 담을 변수


    private List<GameObject> obstaclePool = new List<GameObject>();     // 장애물을 종류별로 여러 개 생성해 담아둘 리스트
    private Vector3 obstacleLastPosition = Vector3.zero;                // 장애물을 어디에 놓을지 마지막 위치를 기억해 둘 변수 
    public int countPerType = 3;                                        // 장애물을 종류별로 몇개씩 만들지에 대한 변수 (지금은 3개씩)


    public float groundObstacleY = 0f;  // 점프/더블 점프 장애물의 Y위치
    public float airObstacleY = 2.5f;   // 슬라이드용 장애물의 Y위치 

    public float minXpadding = 4f;      // 장애물 간 최소 간격
    public float maxXPadding = 7f;      // 장애물 간 최대 간격


    private List<ObstacleType[]> obstaclePatterns = new List<ObstacleType[]>(); //ObstacleType enum 리스트를 가져옴
    private int currentPatternIndex = 0; // 장애물 패턴 순서
    private int currentObstacleInPattern = 0; // 장애물 패턴 추적(몇 번째 장애물이 사용되고 있는지)

    public float patternSpawnInterval = 2f; // 패턴 간 시간 간격
    private float patternTimer = 0f; // 패턴이 작동하는 시간

    public static ObstacleManager Instance { get; private set; }    // 싱글톤 설정

    public void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ObstacleType enum 에 있는 모든 타입(모든 장애물 종류)을 배열로 가져오기
        ObstacleType[] types = (ObstacleType[])System.Enum.GetValues(typeof(ObstacleType));


        foreach (var type in types)                                                 // 배열에 있는 각 장애물마다 설정해주기
        {
            for (int i = 0; i < countPerType; i++)                                  // countPerType에서 설정된 개수만큼 반복해서 생성
            {
                SpawnObstacle(type);
            }
        }

        InitPatterns(); // 장애물 패턴 시작

        SpawnNextPatternObstacle(); // 첫 장애물 패턴이 종료 됐을 시 다음 패턴이 나오도록 SpawnNextPatternObstacle을 통해 검사 후 다음 패턴 시전

    }

    void Update()
    {
        patternTimer += Time.deltaTime;

        if (patternTimer >= patternSpawnInterval) // 패턴 지속 시간이 patternSpawnInterval의 값인 2를 넘겻을 경우
        {
            SpawnNextPatternObstacle(); // 다음 패턴 시전
            patternTimer = 0; // SpawnNextPatternObstacle이 시전 후 patternTimer의 값을 초기화 하여 
        }
    }



    private void InitPatterns() // 장애물 패턴 로직
    {
        //패턴별 예시 정의
        obstaclePatterns.Add(new ObstacleType[]{
            ObstacleType.CompileErrorWall,ObstacleType.RedLineTrap,ObstacleType.SyntaxErrorBox
        });

        obstaclePatterns.Add(new ObstacleType[]{
            ObstacleType.CompileErrorWall,ObstacleType.SyntaxErrorBox
        });

        obstaclePatterns.Add(new ObstacleType[]{
            ObstacleType.SyntaxErrorBox,ObstacleType.SyntaxErrorBox
        });
    }

    private void SpawnNextPatternObstacle()
    {
        if (obstaclePatterns.Count == 0) return; // InitPatterns 안에 존재하는 장애물 패턴이 다 사용됬을 경우 종료

        ObstacleType[] currentPattern = obstaclePatterns[currentPatternIndex]; // 장애물 패턴 시전시 현재 currentPatternIndex 가 가지고 있는 값에 따라 장애물 패턴 시전 (ex: currentPatternIndex == 0 이때
                                                                               // ObstacleType.CompileErrorWall,ObstacleType.RedLineTrap,ObstacleType.SyntaxErrorBox 이 패턴을 가져옴

        ObstacleType typeToSpawn = currentPattern[currentObstacleInPattern]; // 현재 몇 번째 장애물 패턴이 시전 중인지 추적 후 그 패턴에 대한 타입을 가져옴

        SpawnObstacle(typeToSpawn); // SpawnObstacle 추가 할 시 들어가야 됨 (완료)

        currentObstacleInPattern++; // 패턴 시전 후 currentObstacleInPattern의 값이 증가

        if (currentObstacleInPattern >= currentPattern.Length) // 현재 패턴의 장애물이 모두 생성됬을 시
        {
            currentPatternIndex = (currentPatternIndex + 1) % obstaclePatterns.Count; //currentPatternIndex의 값이 1 증가하고 다음 패턴이 등장 (% obstaclePatterns.Count 를 하는 이유는 패턴 목록의 끝에 도달했을 경우 패턴 목록의 처음부터 다시 시작)
            currentObstacleInPattern = 0; // currentObstacleInPattern의 값을 0으로 초기화 한 후 처음 패턴이 종료된 후 다음 패턴의 첫번째 장애물이 나오도록 유도
        }
    }

    private void SpawnObstacle(ObstacleType type)   // 장애물 생성 함수
    {
        ObstacleModel model = new ObstacleModel(type);                               // 모델 객체를 생성해서 장애물의 정보를 저장 (장애물 종류, 데미지, 회피 방법)
        GameObject prefab = GetPrefabByType(type);                                   // GetPrefabByType(type);
        GameObject instance = Instantiate(prefab);                                   // 프리팹 생성하기

        float randomX = Random.Range(minXpadding, maxXPadding);                      // randomX의 값을 min~max 범위에서 랜덤한 값으로 설정
        Vector3 placePosition = obstacleLastPosition + new Vector3(randomX, 0f, 0f); // 장애물 위치 변수의 값을 randomX값으로 설정

        // 장애물 종류에 따라 Y값 설정
        switch (model.Type)
        {
            // 점프로 넘는 장애물일 경우 Y값을 땅쪽으로 설정
            case ObstacleType.RedLineTrap:
            case ObstacleType.SyntaxErrorBox:
                placePosition.y = groundObstacleY;
                break;
            // 슬라이드로 피하는 장애물일 경우 Y값을 위쪽으로 설정
            case ObstacleType.CompileErrorWall:
                placePosition.y = airObstacleY;
                break;
        }
        // ObstacleView view = instance.GetComponent<ObstacleView>();
        // if (view != null)
        // {
        //    view.SetupView(model); // View에 모델 전달 (View에서 구현해야 하는 부분)
        // }

        instance.transform.position = placePosition;    // 장애물 오브젝트의 위치를 실제로 적용시켜주기
        obstacleLastPosition = placePosition;           // 다음 장애물 위치를 지정할 때 참고할 위치 저장

        obstaclePool.Add(instance);
    }

    public void RepositionObstacle(GameObject obstacle) // 장애물을 앞으로 다시 보내는 함수 
    {
        ObstacleType type = ObstacleType.RedLineTrap;   // 현재는 View가 없기 때문에 장애물 이름을 보고 어떤 종류인지 유추한다. 

        if (obstacle.name.Contains("RedLine"))
        {
            type = ObstacleType.RedLineTrap;
        }
        else if (obstacle.name.Contains("Syntax"))
        {
            type = ObstacleType.SyntaxErrorBox;
        }
        else if (obstacle.name.Contains("Compile"))
        {
            type = ObstacleType.CompileErrorWall;
        }

        // 전 장애물 위치에서 X축으로 일정 간격을 띄운 위치를 계산한다.
        float randomX = Random.Range(minXpadding, maxXPadding);                     // randomX 변수를 min ~max 범위에서 랜덤 간격 생성
        Vector3 newPosition = obstacleLastPosition + new Vector3(randomX, 0f, 0f);  // X만 이동

        // 장애물의 종류에 따라 Y 위치를 설정한다. 
        switch (type)
        {
            // 점프 또는 더블점프 장애물은 땅에 배치
            case ObstacleType.RedLineTrap:
            case ObstacleType.SyntaxErrorBox:
                newPosition.y = groundObstacleY;   
                break;

            // 슬라이드 장애물은 위쪽에 배치
            case ObstacleType.CompileErrorWall:
                newPosition.y = airObstacleY;      
                break;
        }

        obstacle.transform.position = newPosition;  // 장애물 오브젝트의 위치를 실제 적용시켜주기 
        obstacleLastPosition = newPosition;         // 다음 장애물 위치를 지정할 때 참고할 위치 저장
    }


    GameObject GetPrefabByType(ObstacleType type)                                           // 프리팹을 반환하는 메서드
    {
        return type switch
        {
            ObstacleType.RedLineTrap => redLinePrefab,                                      // RedLineTrap 장애물 프리팹
            ObstacleType.SyntaxErrorBox => syntaxErrorBoxPrefab,                            // SyntaxErrorBox 장애물 프리팹
            ObstacleType.CompileErrorWall => compileErrorWallPrefab,                        //CompileErrorWall 장애물 프리팹
            _ => null // 혹시라도 값이 잘못 들어오면 null 반환 (예외 처리용)
        };
    }


}
