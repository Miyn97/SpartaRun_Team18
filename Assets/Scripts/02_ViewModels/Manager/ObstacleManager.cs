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

    }

    // Update is called once per frame
    void Update()
    {
        
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
