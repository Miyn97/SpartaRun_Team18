using System.Collections.Generic;
using UnityEngine;

namespace Game.ViewModels
{
    /// <summary>
    /// ViewModel:
    /// - 시작할 때만 풀 크기(countPerPrefab × prefabList.Count)만큼 생성
    /// - 숨김 이벤트마다 풀에서 무작위 하나를 꺼내 와서 위치만 바꾸고 활성화
    /// </summary>
    public class ObstacleSpawner : MonoBehaviour
    {
        [Header("■ 풀(Pool) 설정")]
        [Tooltip("각 부모 프리팹 종류별로 몇 개를 미리 생성할지")]
        public int countPerPrefab = 2;
        [Tooltip("Terrain+장애물이 함께 들어있는 부모 프리팹 리스트 (3종)")]
        public List<GameObject> prefabList;

        [Header("■ 위치·간격 설정")]
        [Tooltip("부모 프리팹 간 X 간격")]
        public float groupSpacing = 27f;    // 지형 한 칸 길이 대략 27
        [Tooltip("부모 프리팹 전체 Y 좌표 (통일)")]
        public float spawnY = -2.5f;

        [Header("■ 거리 보충 여부")]
        [Tooltip("플레이어 앞으로 최소 이만큼의 거리를 채우기")]
        public bool useDistanceCheck = true;
        public float spawnAhead = 54f;      // groupSpacing × 2 정도

        [Header("■ 참조")]
        [Tooltip("씬에 있는 Player 오브젝트의 Transform")]
        public Transform player;

        // ─────────────────────────────────────────────────

        // 풀을 담을 리스트. prefabList 순서와 동일하게 Queue 저장
        private List<Queue<GameObject>> poolList;
        // 마지막으로 배치된 X 좌표
        private float lastSpawnX;

        //─────────────────────────────────────────────────
        private void Awake()
        {
            // 1) 풀 초기화: 각 프리팹당 countPerPrefab개 Instantiate 후 비활성화
            poolList = new List<Queue<GameObject>>();
            foreach (var prefab in prefabList)
            {
                var q = new Queue<GameObject>();
                for (int i = 0; i < countPerPrefab; i++)
                {
                    var obj = Instantiate(prefab);
                    obj.SetActive(false);
                    q.Enqueue(obj);
                }
                poolList.Add(q);
            }

            // 2) 기준 X를 플레이어 현재 위치로 설정
            if (player == null)
                Debug.LogError("[ObstacleSpawner] player Transform이 할당되지 않았습니다!");
            else
                lastSpawnX = player.position.x;
        }

        private void Start()
        {
            // 게임 시작 시, spawnAhead 만큼 사전 배치
            if (useDistanceCheck)
            {
                while (lastSpawnX < player.position.x + spawnAhead)
                    SpawnFromPool();
            }
        }

        private void Update()
        {
            // 플레이어 앞으로 spawnAhead 거리가 부족하면 계속 채워 줌
            if (useDistanceCheck)
            {
                while (lastSpawnX < player.position.x + spawnAhead)
                    SpawnFromPool();
            }
        }

        /// <summary>
        /// ObstacleLooper에서 호출:
        /// 부모 프리팹(지형+장애물)을 숨기고,
        /// 곧바로 풀에서 랜덤 하나를 꺼내 와서 배치
        /// </summary>
        public void OnObstacleHidden(GameObject groundParent)
        {
            // 1) 막 사라진 오브젝트 비활성화
            groundParent.SetActive(false);

            // 2) 즉시 무작위 한 개 스폰
            SpawnFromPool();
        }

        /// <summary>
        /// 풀 리스트 중 무작위로 하나의 Queue를 골라
        /// Dequeue → Enqueue 한 뒤 위치만 이동, 활성화
        /// </summary>
        private void SpawnFromPool()
        {
            // 1) 풀 리스트에서 랜덤 인덱스 선택
            int poolIdx = Random.Range(0, poolList.Count);
            var q = poolList[poolIdx];

            // 2) 순환 큐 사용: Dequeue → Enqueue
            var obj = q.Dequeue();
            q.Enqueue(obj);

            // 3) 위치 결정 & 활성화
            float x = lastSpawnX + groupSpacing;
            obj.transform.position = new Vector3(x, spawnY, 0f);
            obj.SetActive(true);

            // 4) 기준 X 갱신
            lastSpawnX = x;
        }
    }
}
