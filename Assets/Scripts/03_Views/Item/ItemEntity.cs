using UnityEngine;

/// <summary>
/// 씬에서 실제 인게임 아이템 프리팹에 붙는 컴포넌트
/// 아이템의 종류, 모델 데이터, 효과 트리거 등을 연결함
/// </summary>
public class ItemEntity : MonoBehaviour
{
    [SerializeField] private ItemEnum itemType; // 아이템 종류

    private ItemModel model; // 순수 데이터 모델 참조
    private ItemEffectService effectService; // 효과 적용 서비스
    private ItemManager itemManager; // 풀 반환용 참조

    private void Awake()
    {
        // Manager, Service는 미리 연결되었거나 자동 탐색
        itemManager = FindObjectOfType<ItemManager>();
        effectService = FindObjectOfType<ItemEffectService>();
    }

    private void OnEnable()
    {
        // 아이템마다 맞는 모델 데이터를 생성 또는 주입
        model = CreateModel(itemType);
    }

    private ItemModel CreateModel(ItemEnum type)
    {
        switch (type)
        {
            case ItemEnum.Coin: return new ItemModel(type, 0, 0);
            case ItemEnum.HealPotion: return new ItemModel(type, 0, 2);
            case ItemEnum.SpeedPotion: return new ItemModel(type, 3f, 13f);
            case ItemEnum.GiantPotion: return new ItemModel(type, 3f, 0);
            case ItemEnum.Magnet: return new ItemModel(type, 5f, 0);
            default: return new ItemModel(type);
        }
    }

    /// <summary>
    /// 플레이어와 충돌했을 때 효과를 발동시킴
    /// </summary>
    public void Apply()
    {
        Debug.Log($"아이템 효과 적용됨: {itemType}");
        GameManager.Instance.AddScore(100);
        effectService.ApplyEffect(model); // 효과 실행
        itemManager.ReturnToPool(itemType, gameObject); // 풀로 반환
    }
}
