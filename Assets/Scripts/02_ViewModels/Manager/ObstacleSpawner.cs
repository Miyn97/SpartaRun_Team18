using System.Collections.Generic;
using UnityEngine;

namespace Game.ViewModels
{
    /// <summary>
    /// ViewModel: 부모 형태의 지형+장애물 프리팹을 풀링하고,
    /// 거리 기준 또는 숨김 개수 기준으로 재배치합니다.
    /// </summary>
    public class ObstacleSpawner : MonoBehaviour
    {
        [Header("■ 풀(Pool) 설정")]
        [Tooltip("각 프리팹 종류별 초기 풀 크기")]
        public int countPerPrefab = 5;
        [Tooltip("Terrain+장애물 부모 프리팹 리스트 (3종)")]
        public List<GameObject> prefabList;

        [Header("■ 스폰 정책")]
        [Tooltip("플레이어 앞쪽 최소 보장 거리")]
        public float spawnAhead = 50f;     // 대략 groupSpacing*2 정도 추천
        [Tooltip("부모 프리팹 간 X 간격")]
        public float groupSpacing = 27f;   // 땅 스케일 ≈27
        [Tooltip("Y 위치 (통일)")]
        public float spawnY = -2.5f;

        [Header("■ 숨김 기준 재사용")]
        [Tooltip("숨겨진 오브젝트가 이 개수 이상 모이면 재생성")]
        public int hiddenThreshold = 3;
        [Tooltip("거리 기준 스폰 함께 사용 여부")]
        public bool useDistanceCheck = true;

        [Header("■ 참조")]
        [Tooltip("플레이어 Transform (씬의 Player 오브젝트)")]
        public Transform player;

        // ────────────────────────────────────────────────────────────

        // ① 풀: prefabList 순서와 동일하게 Queue 저장
        private List<Queue<GameObject>> poolList;
        // ② 숨김 대기 리스트: ObstacleLooper에서 수집
        private List<GameObject> hiddenList = new List<GameObject>();
        // ③ 마지막으로 배치된 X 좌표
        private float lastSpawnX;

        //─────────────────────────────────────────────────────────────
        private void Awake()
        {
            // 1) 풀 초기화
            poolList = new List<Queue<GameObject>>();
            foreach (var prefab in prefabList)
            {
                var q = new Queue<GameObject>();
                for (int i = 0; i < countPerPrefab; i++)
                {
                    var obj = Instantiate(prefab);
                    obj.SetActive(false);         // 처음엔 모두 숨김
                    q.Enqueue(obj);
                }
                poolList.Add(q);
            }

            // 2) 기준 X = 플레이어 현재 위치
            if (player == null)
                Debug.LogError("[ObstacleSpawner] Player Transform이 할당되지 않음!");
            else
                lastSpawnX = player.position.x;
        }

        private void Start()
        {
            // 게임 시작 시, spawnAhead 거리만큼 미리 채워 두기
            if (useDistanceCheck)
            {
                while (lastSpawnX < player.position.x + spawnAhead)
                    SpawnFromPool();
            }
        }

        private void Update()
        {
            // 3) 숨김 기준: hiddenThreshold 이상 모이면 하나 랜덤 재배치
            if (hiddenList.Count >= hiddenThreshold)
            {
                SpawnFromHidden();
            }

            // 4) 거리 기준: 플레이어 앞으로 spawnAhead까지 채우기
            if (useDistanceCheck)
            {
                while (lastSpawnX < player.position.x + spawnAhead)
                    SpawnFromPool();
            }
        }

        //─────────────────────────────────────────────────────────────

        /// <summary>
        /// ObstacleLooper가 호출: 부모 오브젝트(ObstacleGround 태그)를 숨김 리스트에 보관
        /// </summary>
        public void OnObstacleHidden(GameObject groundParent)
        {
            groundParent.SetActive(false);
            hiddenList.Add(groundParent);
        }

        /// <summary>
        /// 숨겨진 리스트에서 랜덤 하나 꺼내 재배치
        /// </summary>
        private void SpawnFromHidden()
        {
            int idx = Random.Range(0, hiddenList.Count);
            var obj = hiddenList[idx];
            hiddenList.RemoveAt(idx);

            float x = lastSpawnX + groupSpacing;
            obj.transform.position = new Vector3(x, spawnY, 0f);
            obj.SetActive(true);

            lastSpawnX = x;
        }

        /// <summary>
        /// 풀(pooled Queue)에서 무작위 프리팹 꺼내서 배치
        /// </summary>
        private void SpawnFromPool()
        {
            // 1) 랜덤한 풀 인덱스
            int poolIdx = Random.Range(0, poolList.Count);
            var q = poolList[poolIdx];

            // 2) 순환 큐처럼 꺼내고 다시 넣기
            var obj = q.Dequeue();
            q.Enqueue(obj);

            // 3) 위치 계산 & 활성화
            float x = lastSpawnX + groupSpacing;
            obj.transform.position = new Vector3(x, spawnY, 0f);
            obj.SetActive(true);

            // 4) X 기준 갱신
            lastSpawnX = x;
        }
    }
}
