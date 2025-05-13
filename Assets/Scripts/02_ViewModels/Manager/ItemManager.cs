using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 아이템 프리팹과 타입을 연결하기 위한 구조체
    [System.Serializable]//[SerializeField]와 비슷하지만 [SerializeField]는 변수, [System.Serializable]는 구조체나 클래스 위에 붙임
    public struct ItemPrefab
    {
        public ItemEnum type;        // 아이템 종류 (HealPotion, SpeedPotion 등)
        public GameObject prefab;    // 해당 타입에 대응하는 프리팹
    }

    // 인스펙터에서 아이템 타입별 프리팹을 지정할 수 있도록 리스트로 선언
    [SerializeField] private List<ItemPrefab> itemPrefabs;

    // 각 타입별로 몇 개씩 미리 풀에 생성할 것인지 설정
    [SerializeField] private int poolSize = 3;

    [SerializeField] private Transform player;

    [SerializeField] private LayerMask obstacleLayer;//LayerMask = 특정 레이어들만 선택적으로 감지하거나 검사할때 쓰는 데이터타입

    // 아이템 종류(enum)별로 각각의 풀(Queue)을 관리하는 딕셔너리. 키 = enum, 값 = queue
    private Dictionary<ItemEnum, Queue<GameObject>> poolDict = new Dictionary<ItemEnum, Queue<GameObject>>();
    //Queue는 선입선출 방식의 자료구조

    private void Awake()
    {
        // 게임 시작 시 각 아이템 타입에 대한 풀을 초기화
        foreach (var item in itemPrefabs)
        {
            Queue<GameObject> queue = new Queue<GameObject>();

            // poolSize만큼 오브젝트를 생성해서 큐에 넣기
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, transform); // 프리팹을 자식으로 생성
                obj.SetActive(false); // 비활성화해서 화면에 안 보이게
                queue.Enqueue(obj);   // 큐에 저장
            }

            // 딕셔너리에 현재 타입과 그 큐를 등록
            poolDict[item.type] = queue;
        }
    }


    // 외부에서 특정 타입의 오브젝트를 꺼내 쓸 때 호출
    public GameObject Get(ItemEnum type)
    {
        // 요청한 타입이 딕셔너리에 있는지 확인
        if (poolDict.TryGetValue(type, out Queue<GameObject> queue))
        {
            if (queue.Count > 0)
            {
                GameObject obj = queue.Dequeue(); // 큐에서 하나 꺼냄
                obj.SetActive(true);              // 사용 가능하게 활성화
                return obj;
            }
        }

        // 큐가 비어있거나 타입이 존재하지 않으면 경고 출력
        Debug.LogWarning($"풀에 {type} 아이템이 부족합니다.");
        return null;
    }

    // 오브젝트 사용 후 다시 풀에 반환할 때 호출
    public void ReturnToPool(ItemEnum type, GameObject obj)
    {
        obj.SetActive(false);              // 화면에서 안 보이게 비활성화
        poolDict[type].Enqueue(obj);       // 다시 큐에 넣어서 재사용 가능하게 함
    }


    public void SpawnItem(ItemEnum type)
    {
        GameObject item = Get(type); // 풀에서 아이템 가져오기

        if (item != null)
        {
            Vector3 spawnPosition = player.position;//플레이어의 포지션 가져와서
            spawnPosition.x += 15f;//오른쪽으로 + 15한곳 저장

            //Physics2D.OverlapCircle = 영역을 검사해서 그 안에 있는 콜라이더를 감지하는 2D 물리 함수 ( 검사위치, 원의 반지름, 검사하는 레이어의 종류)
            if (Physics2D.OverlapCircle(spawnPosition, 0.5f, obstacleLayer))
            {
                spawnPosition.x += 1f; // 1만큼 옆으로 밀기
                spawnPosition.y += 1f;//위로도 한칸

                if (Physics2D.OverlapCircle(spawnPosition, 0.5f, obstacleLayer))
                {
                    Debug.Log("장애물 때문에 아이템 생성 취소");
                    ReturnToPool(type, item);
                    return;
                }
            }
            item.transform.position = spawnPosition;//저장한곳에 아이템 소환
            item.SetActive(true);
        }
    }
    public IEnumerator SpawnRandomItem(float startTime)//()는 코루틴 시작할때 설정하는 시간
    {
        float playTime = 0f;//플레이타임 비례해서 
        float nowCoolTime = startTime;//처음 쿨타임은 코루틴 시작할때 입력한값
        float maxCoolTime = 30f;//최대 쿨타임 30초

        while (true)
        {
            Debug.Log("랜덤 아이템 생성됨");
            yield return new WaitForSeconds(nowCoolTime);//현재쿨타임만큼 기다리기

            ItemEnum randomItem = (ItemEnum)Random.Range(1, System.Enum.GetValues(typeof(ItemEnum)).Length);//소환될 아이템을 정하고 
            SpawnItem(randomItem);//스폰아이템

            playTime += nowCoolTime;//플레이타임에 현재 쿨타임만큼 더해서 플레이타임을 갱신.
            nowCoolTime = Mathf.Min(startTime + playTime * 0.05f, maxCoolTime);//현재 쿨타임은 플레이타임 비례 증가, 최대 30f 
            //Mathf.Min 은 ( a , b ) 둘중에 작은값을 반환하는 함수
        }
    }
    public IEnumerator SpawnCoin(int Time)//코인소환
    {
        while (true)
        {
            yield return new WaitForSeconds(Time);
            SpawnItem(ItemEnum.Coin);
            Debug.Log("코인 생성됨");
        }
    }

}