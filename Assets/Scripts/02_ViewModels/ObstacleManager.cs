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




    // Start is called before the first frame update
    void Start()
    {
        SpawnRandomObstacle();  // 장애물 생성 메서드를 호출
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRandomObstacle()                                                              // 장애물 생성 메서드
    {
        ObstacleType type = GetRandomObstacleType();                                        // 랜덤으로 장애물 타입을 선택

        ObstacleModel model = new ObstacleModel(type);                                      // 장애물 객체 생성

        GameObject prefab = GetPrefabByType(type);                                          // 장애물 종류에 맞는 프리팹 가져오기

        Instantiate(prefab, spawnPoint.position, Quaternion.identity);                      // 실제로 장애물 오브젝트를 생성

        Debug.Log($"Spawned: {model.Type}, Damage: {model.Damage}, Avoid: {model.Avoid}");  // 테스트용 콘솔 출력 메시지
    }

    ObstacleType GetRandomObstacleType()                                                    // 장애물 랜덤 선택 메서드 (ObstacleType enum에 있는 장애물 중 랜덤으로 하나를 선택)
    {
        int count = System.Enum.GetValues(typeof(ObstacleType)).Length;                     // ObstacleType num에 정의된 모든 장애물 종류의 개수를 가져와서 저장

        int randomIndex = Random.Range(0, count);                                           // 0~장애물 개수 사이의 숫자 중에서 랜덤으로 인덱스 값을 설정 (현재 장애물 개수는 3개이니 0~2 값)

        return (ObstacleType)randomIndex;                                                   // 설정된 랜덤 인덱스 값을 ObstacleType enum으로 변환해서 리턴
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
