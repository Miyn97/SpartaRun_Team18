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




    // Start is called before the first frame update
    void Start()
    {
        SpawnRandomObstacle();  // ��ֹ� ���� �޼��带 ȣ��
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRandomObstacle()                                                              // ��ֹ� ���� �޼���
    {
        ObstacleType type = GetRandomObstacleType();                                        // �������� ��ֹ� Ÿ���� ����

        ObstacleModel model = new ObstacleModel(type);                                      // ��ֹ� ��ü ����

        GameObject prefab = GetPrefabByType(type);                                          // ��ֹ� ������ �´� ������ ��������

        Instantiate(prefab, spawnPoint.position, Quaternion.identity);                      // ������ ��ֹ� ������Ʈ�� ����

        Debug.Log($"Spawned: {model.Type}, Damage: {model.Damage}, Avoid: {model.Avoid}");  // �׽�Ʈ�� �ܼ� ��� �޽���
    }

    ObstacleType GetRandomObstacleType()                                                    // ��ֹ� ���� ���� �޼��� (ObstacleType enum�� �ִ� ��ֹ� �� �������� �ϳ��� ����)
    {
        int count = System.Enum.GetValues(typeof(ObstacleType)).Length;                     // ObstacleType num�� ���ǵ� ��� ��ֹ� ������ ������ �����ͼ� ����

        int randomIndex = Random.Range(0, count);                                           // 0~��ֹ� ���� ������ ���� �߿��� �������� �ε��� ���� ���� (���� ��ֹ� ������ 3���̴� 0~2 ��)

        return (ObstacleType)randomIndex;                                                   // ������ ���� �ε��� ���� ObstacleType enum���� ��ȯ�ؼ� ����
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
