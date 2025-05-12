using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    // ������ ��ֹ��� �����ϴ� �������� �־�� ������ (Inspector���� ���� ����)
    public GameObject redLinePrefab;            // RedLineTrap ������
    public GameObject syntaxErrorBoxPrefab;     // SyntaxErrorBox ������
    public GameObject compileErrorWallPrefab;   // CompileErrorWall ������

    public Transform spawnPoint;                // ��ֹ��� ������ ��ġ�� ���� ����


    private List<GameObject> obstaclePool = new List<GameObject>();     // ��ֹ��� �������� ���� �� ������ ��Ƶ� ����Ʈ
    private Vector3 obstacleLastPosition = Vector3.zero;                // ��ֹ��� ��� ������ ������ ��ġ�� ����� �� ���� 
    public int countPerType = 3;                                        // ��ֹ��� �������� ��� �������� ���� ���� (������ 3����)


    public float groundObstacleY = 0f;  // ����/���� ���� ��ֹ��� Y��ġ
    public float airObstacleY = 2.5f;   // �����̵�� ��ֹ��� Y��ġ 

    public float minXpadding = 4f;      // ��ֹ� �� �ּ� ����
    public float maxXPadding = 7f;      // ��ֹ� �� �ִ� ����


    private List<ObstacleType[]> obstaclePatterns = new List<ObstacleType[]>(); //ObstacleType enum ����Ʈ�� ������
    private int currentPatternIndex = 0; // ��ֹ� ���� ����
    private int currentObstacleInPattern = 0; // ��ֹ� ���� ����(�� ��° ��ֹ��� ���ǰ� �ִ���)

    public float patternSpawnInterval = 2f; // ���� �� �ð� ����
    private float patternTimer = 0f; // ������ �۵��ϴ� �ð�



    // Start is called before the first frame update
    void Start()
    {
        // ObstacleType enum �� �ִ� ��� Ÿ��(��� ��ֹ� ����)�� �迭�� ��������
        ObstacleType[] types = (ObstacleType[])System.Enum.GetValues(typeof(ObstacleType));


        foreach (var type in types)                                                 // �迭�� �ִ� �� ��ֹ����� �������ֱ�
        {
            for (int i = 0; i < countPerType; i++)                                  // countPerType���� ������ ������ŭ �ݺ��ؼ� ����
            {
                ObstacleModel model = new ObstacleModel(type);                      // ���� �����ؼ� ��ֹ��� ������ ����(�忡�� ����, ������, ȸ�� ���)
                GameObject prefab = GetPrefabByType(type);                          // ��ֹ� ������ �´� ������ ��������
                GameObject instance = Instantiate(prefab);                          // ������ �����ϱ�

                // ��ֹ��� X ��ġ�� ���� ��ֹ� ��ġ + ������ �Ÿ��� ����
                float randomX = Random.Range(minXpadding, maxXPadding);                         // randomX�� ���� �ּ�~�ִ� ���� �� ������ ������ ����
                Vector3 placePosition = obstacleLastPosition + new Vector3(randomX, 0f, 0f);    // ��ֹ� ��ġ ������ �� ������ ���� randomX������ ����

                // ��ֹ� ������ ���� Y�� ����
                switch (model.Type)
                {
                    // ������ �Ѵ� ��ֹ��� ��� Y���� �������� ����
                    case ObstacleType.RedLineTrap:
                    case ObstacleType.SyntaxErrorBox:
                        placePosition.y = groundObstacleY;
                        break;

                    // �����̵�� ���ϴ� ��ֹ��� ��� Y���� �������� ����
                    case ObstacleType.CompileErrorWall:
                        placePosition.y = airObstacleY;
                        break;
                }

                //ObstacleView view = instance.GetComponent<ObstacleView>();
                //if (view != null)
                //{
                //    view.SetupView(model); // View�� �� ���� (View���� �����ؾ� �ϴ� �κ�)
                //}

                instance.transform.position = placePosition;        // ��ġ�� ������ ��������ֱ�
                obstacleLastPosition = placePosition;               // ���� ��ֹ� ��ġ�� ������ �� ������ ��ġ ����


                obstaclePool.Add(instance); // ������ ��ֹ��� ����Ʈ�� ����
            }
        }

        InitPatterns(); // ��ֹ� ���� ����

        SpawnNextPatternObstacle(); // ù ��ֹ� ������ ���� ���� �� ���� ������ �������� SpawnNextPatternObstacle�� ���� �˻� �� ���� ���� ����

    }

    void Update()
    {
        patternTimer += Time.deltaTime;

        if (patternTimer >= patternSpawnInterval) // ���� ���� �ð��� patternSpawnInterval�� ���� 2�� �Ѱ��� ���
        {
            SpawnNextPatternObstacle(); // ���� ���� ����
            patternTimer = 0; // SpawnNextPatternObstacle�� ���� �� patternTimer�� ���� �ʱ�ȭ �Ͽ� 
        }
    }



    private void InitPatterns() // ��ֹ� ���� ����
    {
        //���Ϻ� ���� ����
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
        if (obstaclePatterns.Count == 0) return; // InitPatterns �ȿ� �����ϴ� ��ֹ� ������ �� ������ ��� ����

        ObstacleType[] currentPattern = obstaclePatterns[currentPatternIndex]; // ��ֹ� ���� ������ ���� currentPatternIndex �� ������ �ִ� ���� ���� ��ֹ� ���� ���� (ex: currentPatternIndex == 0 �̶�
                                                                               // ObstacleType.CompileErrorWall,ObstacleType.RedLineTrap,ObstacleType.SyntaxErrorBox �� ������ ������

        ObstacleType typeToSpawn = currentPattern[currentObstacleInPattern]; // ���� �� ��° ��ֹ� ������ ���� ������ ���� �� �� ���Ͽ� ���� Ÿ���� ������

        //SpawnObstacle(typeToSpawn); // SpawnObstacle �߰� �� �� ���� ��

        currentObstacleInPattern++; // ���� ���� �� currentObstacleInPattern�� ���� ����

        if (currentObstacleInPattern >= currentPattern.Length) // ���� ������ ��ֹ��� ��� �������� ��
        {
            currentPatternIndex = (currentPatternIndex + 1) % obstaclePatterns.Count; //currentPatternIndex�� ���� 1 �����ϰ� ���� ������ ���� (% obstaclePatterns.Count �� �ϴ� ������ ���� ����� ���� �������� ��� ���� ����� ó������ �ٽ� ����)
            currentObstacleInPattern = 0; // currentObstacleInPattern�� ���� 0���� �ʱ�ȭ �� �� ó�� ������ ����� �� ���� ������ ù��° ��ֹ��� �������� ����
        }
    }

    //private void SpawnObstacle(ObstacleType type) ����Ƽ �� �� start�� �ִ� ���� Obstacle ������ SpawnObstacle�� ���� ����� ������ �ҷ����� ��
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

    GameObject GetPrefabByType(ObstacleType type)                                           // �������� ��ȯ�ϴ� �޼���
    {
        return type switch
        {
            ObstacleType.RedLineTrap => redLinePrefab,                                      // RedLineTrap ��ֹ� ������
            ObstacleType.SyntaxErrorBox => syntaxErrorBoxPrefab,                            // SyntaxErrorBox ��ֹ� ������
            ObstacleType.CompileErrorWall => compileErrorWallPrefab,                        //CompileErrorWall ��ֹ� ������
            _ => null // Ȥ�ö� ���� �߸� ������ null ��ȯ (���� ó����)
        };
    }


}
