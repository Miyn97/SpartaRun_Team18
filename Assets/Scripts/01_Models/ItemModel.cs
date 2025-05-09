using System.Collections;
using System.Collections.Generic;
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

public enum ItemEnum
{
    GiantPotion,
    SpeedPotion,
    Bomb,
    magnet,
    healpotion
}


public class ItemModel : MonoBehaviour
{
    public ItemEnum itemenum;
    public float duration; // 지속시간 (필요 없는 경우는 0)

    public int healAmount; // 회복량 (힐 전용)

    public int Score;
}
