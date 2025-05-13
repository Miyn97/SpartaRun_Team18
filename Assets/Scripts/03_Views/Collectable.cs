using UnityEngine;

/// <summary>
/// Player가 충돌했을 때 ItemEntity의 Apply()를 호출
/// (MVVM 구조에서는 View 역할만 함)
/// </summary>
[RequireComponent(typeof(ItemEntity))]
public class Collectable : MonoBehaviour
{
    private ItemEntity itemEntity;

    private void Awake()
    {
        itemEntity = GetComponent<ItemEntity>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            itemEntity.Apply(); // 효과 실행 (내부적으로 ItemEffectService 호출)
        }
    }
}
