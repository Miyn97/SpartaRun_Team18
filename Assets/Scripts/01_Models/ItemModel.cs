using System;
using UnityEngine;

/*
아이템 종류, 효과수치 등
아이템 종류 밑 임의수치
거대화               일정시간동안 무적 및 크기가 커진다. 5초동안 거대화?
질주                 일정시간 동안 무적상태로 빠른속도로 달린다. 3초동안 speed + 3f?
폭죽=코인매직        전방 일정범위의 장애물을 무력화? 무효화 하고 점수를 얻을 수 있는 요소 생성 
자석                 일정시간동안 플레이어 주변에 collectable << 아이템, 코인 등을 빨아들인다 플레이어 로컬좌표의 
체력물약             체력 회복. 일단은 한칸회복

코인도 여기 들어가나?
플레이어 무적 필요, 장애물 파괴? 비활성화? 필요, 

 */

// MonoBehaviour 상속 제거 → 순수 데이터 클래스로 변경
// 코루틴 및 Unity 의존 로직 제거
// Awake(), FindObjectOfType(), Coroutine 제거
// itemEnum 필드를 ItemType으로 명확히 명명
// Duration, Value 필드를 통해 확장성 확보 (예: GiantPotion의 지속시간, Heal 수치 등)


/// <summary>
/// 아이템의 종류를 정의하는 열거형
/// </summary>
public enum ItemEnum
{
    Coin,         // 점수 획득
    HealPotion,   // 체력 회복
    SpeedPotion,  // 일정 시간 동안 속도 증가 + 무적
    GiantPotion,  // 일정 시간 동안 크기 증가 + 무적
    Magnet        // 일정 시간 동안 주변 Collectable 끌어당김
    // Bomb: 향후 추가 가능
}

/// <summary>
/// 아이템의 순수 데이터 모델 클래스 - ViewModel이나 Manager에서 사용할 목적
/// MonoBehaviour를 상속하지 않고, 순수 데이터만 보관하도록 설계
/// </summary>
[Serializable]
public class ItemModel
{
    /// <summary>
    /// 아이템의 종류
    /// </summary>
    public ItemEnum ItemType;

    /// <summary>
    /// 지속시간이 필요한 경우의 시간 (초)
    /// 예: GiantPotion 3초, Magnet 5초 등
    /// </summary>
    public float Duration;

    /// <summary>
    /// 수치 기반 아이템의 수치 값
    /// 예: HealPotion 회복량, SpeedPotion 속도 수치 등
    /// </summary>
    public float Value;

    /// <summary>
    /// 생성자 - 인스펙터 또는 코드 상에서 초기화 가능
    /// </summary>
    public ItemModel(ItemEnum itemType, float duration = 0f, float value = 0f)
    {
        ItemType = itemType;  // 아이템 종류 설정
        Duration = duration;  // 효과 지속 시간 설정
        Value = value;        // 수치 설정
    }

    /// <summary>
    /// 디버그용 ToString 오버라이드 - 디버깅이나 로그 확인용
    /// </summary>
    public override string ToString()
    {
        return $"ItemType: {ItemType}, Duration: {Duration}, Value: {Value}";
    }
}