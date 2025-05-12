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

    void SpawnRandomObstacle()  // 장애물 생성 메서드
    {
        ObstacleType type = GetRandomObstacleType();    // 랜덤으로 장애물 타입을 선택

        ObstacleModel model = new ObstacleModel(type);  // 장애물 객체 생성

        GameObject prefab = GetPrefabByType(type);      // 장애물 종류에 맞는 프리팹 가져오기

        Instantiate(prefab, spawnPoint.position, Quaternion.identity);  // 실제로 장애물 오브젝트를 생성
    }

    ObstacleType GetRandomObstacleType()
}
