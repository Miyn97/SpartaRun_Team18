using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� ���� ��ġ �˻� �� �ð� ��� ���� ������ �Բ� ����ϴ� Ŭ����
/// </summary>
public class ItemSpawnController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private ItemManager itemManager; // ������ Ǯ�� �Ŵ���
    [SerializeField] private Transform player;         // �÷��̾� ��ġ ����
    [SerializeField] private LayerMask obstacleLayer;  // ��ֹ� ������ ���̾�

    [Header("���� ����")]
    [SerializeField] private float spawnOffsetX = 15f; // �÷��̾� ���� ���� ��ġ X
    [SerializeField] private float overlapRadius = 0.5f; // ��ֹ� ���� ����
    [SerializeField] private Vector2 fallbackOffset = new(1f, 1f); // ��ֹ� ���� �� ���� ��ġ

    private readonly float maxCoolTime = 30f; // ���� ���� �ִ� ���� ����

    /// <summary>
    /// �ܺο��� ���� Ư�� �������� �����ϰ��� �� �� ���
    /// </summary>
    public void SpawnItem(ItemEnum type)
    {
        var item = itemManager.Get(type);
        if (item == null) return;

        Vector3 spawnPos = player.position + Vector3.right * spawnOffsetX;

        if (Physics2D.OverlapCircle(spawnPos, overlapRadius, obstacleLayer))
        {
            spawnPos += new Vector3(fallbackOffset.x, fallbackOffset.y);

            if (Physics2D.OverlapCircle(spawnPos, overlapRadius, obstacleLayer))
            {
                Debug.Log("������ ���� ��ġ�� ��ֹ��� �����Ͽ� ��ҵ�");
                itemManager.ReturnToPool(type, item);
                return;
            }
        }

        item.transform.position = spawnPos;
        item.SetActive(true);
    }

    /// <summary>
    /// �ֱ������� ������ �����ϴ� �ڷ�ƾ
    /// </summary>
    public IEnumerator SpawnCoinRoutine(float interval)
    {
        while (true)
        {
            yield return YieldCache.WaitForSeconds(interval); // GC �ּ�ȭ ĳ��
            SpawnItem(ItemEnum.Coin);
        }
    }

    /// <summary>
    /// ���� �ֱ�� ���� �������� �����ϴ� �ڷ�ƾ
    /// </summary>
    public IEnumerator SpawnRandomItemRoutine(float startCoolTime)
    {
        float playTime = 0f;
        float nowCoolTime = startCoolTime;

        while (true)
        {
            yield return YieldCache.WaitForSeconds(nowCoolTime);

            // Coin ������ ������ �����۵鿡�� ���� ����
            ItemEnum randomItem = (ItemEnum)Random.Range(1, System.Enum.GetValues(typeof(ItemEnum)).Length);
            SpawnItem(randomItem);

            playTime += nowCoolTime;
            nowCoolTime = Mathf.Min(startCoolTime + playTime * 0.05f, maxCoolTime);
        }
    }
}
