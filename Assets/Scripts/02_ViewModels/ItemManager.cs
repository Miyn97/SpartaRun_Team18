using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // ������ �����հ� Ÿ���� �����ϱ� ���� ����ü
    [System.Serializable]//[SerializeField]�� ��������� [SerializeField]�� ����, [System.Serializable]�� ����ü�� Ŭ���� ���� ����
    public struct ItemPrefab
    {
        public ItemEnum type;        // ������ ���� (HealPotion, SpeedPotion ��)
        public GameObject prefab;    // �ش� Ÿ�Կ� �����ϴ� ������
    }

    // �ν����Ϳ��� ������ Ÿ�Ժ� �������� ������ �� �ֵ��� ����Ʈ�� ����
    [SerializeField] private List<ItemPrefab> itemPrefabs;

    // �� Ÿ�Ժ��� �� ���� �̸� Ǯ�� ������ ������ ����
    [SerializeField] private int poolSize = 3;

    [SerializeField] private Transform player;

    // ������ ����(enum)���� ������ Ǯ(Queue)�� �����ϴ� ��ųʸ�. Ű = enum, �� = queue
    private Dictionary<ItemEnum, Queue<GameObject>> poolDict = new Dictionary<ItemEnum, Queue<GameObject>>();
    //Queue�� ���Լ��� ����� �ڷᱸ��

    private void Awake()
    {
        // ���� ���� �� �� ������ Ÿ�Կ� ���� Ǯ�� �ʱ�ȭ
        foreach (var item in itemPrefabs)
        {
            Queue<GameObject> queue = new Queue<GameObject>();

            // poolSize��ŭ ������Ʈ�� �����ؼ� ť�� �ֱ�
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, transform); // �������� �ڽ����� ����
                obj.SetActive(false); // ��Ȱ��ȭ�ؼ� ȭ�鿡 �� ���̰�
                queue.Enqueue(obj);   // ť�� ����
            }

            // ��ųʸ��� ���� Ÿ�԰� �� ť�� ���
            poolDict[item.type] = queue;
        }
    }


    // �ܺο��� Ư�� Ÿ���� ������Ʈ�� ���� �� �� ȣ��
    public GameObject Get(ItemEnum type)
    {
        // ��û�� Ÿ���� ��ųʸ��� �ִ��� Ȯ��
        if (poolDict.TryGetValue(type, out Queue<GameObject> queue))
        {
            if (queue.Count > 0)
            {
                GameObject obj = queue.Dequeue(); // ť���� �ϳ� ����
                obj.SetActive(true);              // ��� �����ϰ� Ȱ��ȭ
                return obj;
            }
        }

        // ť�� ����ְų� Ÿ���� �������� ������ ��� ���
        Debug.LogWarning($"Ǯ�� {type} �������� �����մϴ�.");
        return null;
    }

    // ������Ʈ ��� �� �ٽ� Ǯ�� ��ȯ�� �� ȣ��
    public void ReturnToPool(ItemEnum type, GameObject obj)
    {
        obj.SetActive(false);              // ȭ�鿡�� �� ���̰� ��Ȱ��ȭ
        poolDict[type].Enqueue(obj);       // �ٽ� ť�� �־ ���� �����ϰ� ��
    }


    public void SpawnItem(ItemEnum type)
    {
        GameObject item = Get(type); // Ǯ���� ������ ��������

        if (item != null)
        {
            Vector3 spawnPosition = player.position;//�÷��̾��� ������ �����ͼ�
            spawnPosition.x += 15f;//���������� + 15�Ѱ� ����

            item.transform.position = spawnPosition;//�����Ѱ��� ������ ��ȯ
            item.SetActive(true);
        }
    }


}

/*   ���ӸŴ����� �߰��� �͵�
using System.Collections;

ItemManager itemManager;




update��         StartCoroutine(SpawnRandomItem(15f));//�����Ҷ� �ڷ�ƾ�� ����, 15�� �Ŀ� ����


    private IEnumerator SpawnRandomItem(float startTime)//()�� �ڷ�ƾ �����Ҷ� �����ϴ� �ð�
    {
        float playTime = 0f;//�÷���Ÿ�� ����ؼ� 
        float nowCoolTime = startTime;//ó�� ��Ÿ���� �ڷ�ƾ �����Ҷ� �Է��Ѱ�
        float maxCoolTime = 30f;//�ִ� ��Ÿ�� 30��

        while (true)
        {
            yield return new WaitForSeconds(nowCoolTime);//������Ÿ�Ӹ�ŭ ��ٸ���

            ItemEnum randomItem = (ItemEnum)Random.Range(0, System.Enum.GetValues(typeof(ItemEnum)).Length);//��ȯ�� �������� ���ϰ� 
            itemManager.SpawnItem(randomItem);//������ �Ŵ����� ����������

            playTime += nowCoolTime;//�÷���Ÿ�ӿ� ���� ��Ÿ�Ӹ�ŭ ���ؼ� �÷���Ÿ���� ����.
            nowCoolTime = Mathf.Min(startTime + playTime * 0.05f, maxCoolTime);//���� ��Ÿ���� �÷���Ÿ�� ��� ����, �ִ� 30f 
            //Mathf.Min �� ( a , b ) ���߿� �������� ��ȯ�ϴ� �Լ�
        }
    }
*/