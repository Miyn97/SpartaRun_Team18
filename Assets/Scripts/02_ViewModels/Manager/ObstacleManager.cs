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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

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
