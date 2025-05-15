using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템의 생성 위치 검사 및 시간 기반 랜덤 스폰을 함께 담당하는 클래스
/// </summary>
public class ItemSpawnController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private ItemManager itemManager; // 아이템 풀링 매니저
    [SerializeField] private Transform player;         // 플레이어 위치 기준
    [SerializeField] private LayerMask obstacleLayer;  // 장애물 감지용 레이어

    [Header("스폰 설정")]
    [SerializeField] private float spawnOffsetX = 15f; // 플레이어 기준 생성 위치 X
    [SerializeField] private float overlapRadius = 1f; // 장애물 감지 범위
    [SerializeField] private Vector2 fallbackOffset = new(2f, 2f); // 장애물 있을 때 보정 위치

    private Coroutine spawnRoutine;

    public void StartSpawn()
    {
        spawnRoutine = StartCoroutine(SpawnCoinRoutine(2f));
    }

    public void StopSpawn()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    private readonly float maxCoolTime = 30f; // 랜덤 생성 최대 간격 제한

    /// <summary>
    /// 외부에서 직접 특정 아이템을 스폰하고자 할 때 사용
    /// </summary>
    public void SpawnItem(ItemEnum type)
    {
        if (itemManager == null || itemManager.Equals(null)) //방어 코드
        {
            //Debug.LogWarning("ItemManager가 존재하지 않아 SpawnItem 취소됨");
            return;
        }

        var item = itemManager.Get(type);
        if (item == null) return;

        Vector3 spawnPos = player.position + Vector3.right * spawnOffsetX;

        if (Physics2D.OverlapCircle(spawnPos, overlapRadius, obstacleLayer))
        {
            spawnPos += new Vector3(fallbackOffset.x, fallbackOffset.y);

            if (Physics2D.OverlapCircle(spawnPos, overlapRadius, obstacleLayer))
            {
                //Debug.Log("아이템 생성 위치에 장애물이 존재하여 취소됨");
                itemManager.ReturnToPool(type, item);
                return;
            }
        }

        item.transform.position = spawnPos;
        item.SetActive(true);
    }

    /// <summary>
    /// 주기적으로 코인을 생성하는 코루틴
    /// </summary>
    public IEnumerator SpawnCoinRoutine(float interval)
    {
        while (true)
        {
            yield return YieldCache.WaitForSeconds(interval); // GC 최소화 캐싱

            if (itemManager == null || itemManager.Equals(null))
            {
                //Debug.LogWarning("ItemManager가 파괴되어 SpawnCoinRoutine을 중단합니다.");
                yield break;
            }
            SpawnItem(ItemEnum.Coin);
        }
    }

    /// <summary>
    /// 일정 주기로 랜덤 아이템을 생성하는 코루틴
    /// </summary>
    public IEnumerator SpawnRandomItemRoutine(float startCoolTime)
    {
        float playTime = 0f;
        float nowCoolTime = startCoolTime;

        while (true)
        {
            yield return YieldCache.WaitForSeconds(nowCoolTime);

            // Coin 제외한 나머지 아이템들에서 랜덤 선택
            ItemEnum randomItem = (ItemEnum)Random.Range(1, System.Enum.GetValues(typeof(ItemEnum)).Length);
            SpawnItem(randomItem);

            playTime += nowCoolTime;
            nowCoolTime = Mathf.Min(startCoolTime + playTime * 0.05f, maxCoolTime);
        }
    }
}
