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



    // Start is called before the first frame update
    void Start()
    {
        // ObstacleType enum 에 있는 모든 타입(모든 장애물 종류)을 배열로 가져오기
        ObstacleType[] types = (ObstacleType[])System.Enum.GetValues(typeof(ObstacleType));


        foreach (var type in types)                                                 // 배열에 있는 각 장애물마다 설정해주기
        {
            for (int i = 0; i < countPerType; i++)                                  // countPerType에서 설정된 개수만큼 반복해서 생성
            {
                ObstacleModel model = new ObstacleModel(type);                      // 모델을 생성해서 장애물의 정보를 저장(장에물 종류, 데미지, 회피 방법)
                GameObject prefab = GetPrefabByType(type);                          // 장애물 종류에 맞는 프리팹 가져오기
                GameObject instance = Instantiate(prefab);                          // 프리팹 생성하기

                // 장애물의 X 위치는 이전 장애물 위치 + 랜덤한 거리로 설정
                float randomX = Random.Range(minXpadding, maxXPadding);                         // randomX의 값을 최소~최대 간격 중 랜덤한 값으로 설정
                Vector3 placePosition = obstacleLastPosition + new Vector3(randomX, 0f, 0f);    // 장애물 위치 정보로 쓸 변수의 값을 randomX값으로 설정

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

                //ObstacleView view = instance.GetComponent<ObstacleView>();
                //if (view != null)
                //{
                //    view.SetupView(model); // View에 모델 전달 (View에서 구현해야 하는 부분)
                //}

                instance.transform.position = placePosition;        // 위치를 실제로 적용시켜주기
                obstacleLastPosition = placePosition;               // 다음 장애물 위치를 지정할 때 참고할 위치 정보


                obstaclePool.Add(instance); // 생성된 장애물을 리스트에 저장
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

        //SpawnObstacle(typeToSpawn); // SpawnObstacle 추가 할 시 들어가야 됨

        currentObstacleInPattern++; // 패턴 시전 후 currentObstacleInPattern의 값이 증가

        if (currentObstacleInPattern >= currentPattern.Length) // 현재 패턴의 장애물이 모두 생성됬을 시
        {
            currentPatternIndex = (currentPatternIndex + 1) % obstaclePatterns.Count; //currentPatternIndex의 값이 1 증가하고 다음 패턴이 등장 (% obstaclePatterns.Count 를 하는 이유는 패턴 목록의 끝에 도달했을 경우 패턴 목록의 처음부터 다시 시작)
            currentObstacleInPattern = 0; // currentObstacleInPattern의 값을 0으로 초기화 한 후 처음 패턴이 종료된 후 다음 패턴의 첫번째 장애물이 나오도록 유도
        }
    }

    //private void SpawnObstacle(ObstacleType type) 지피티 햄 왈 start에 있던 기존 Obstacle 로직을 SpawnObstacle로 따로 만들어 로직을 불러오라 함
    //{
    //    ObstacleModel model = new ObstacleModel(type);
    //    GameObject prefab = GetPrefabByType(type);
    //    GameObject instance = Instantiate(prefab);

    //    float randomX = Random.Range(minXpadding, maxXPadding);
    //    Vector3 placePosition = obstacleLastPosition + new Vector3(randomX, 0f, 0f);

    //    switch (model.Type)
    //    {
    //        case ObstacleType.RedLineTrap:
    //        case ObstacleType.SyntaxErrorBox:
    //            placePosition.y = groundObstacleY;
    //            break;
    //        case ObstacleType.CompileErrorWall:
    //            placePosition.y = airObstacleY;
    //            break;
    //    }

    //    instance.transform.position = placePosition;
    //    obstacleLastPosition = placePosition;

    //    obstaclePool.Add(instance);
    //}

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
