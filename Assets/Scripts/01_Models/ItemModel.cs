using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    HealPotion,
    SpeedPotion,
    GiantPotion/*,
    Bomb,
    Magnet*/


}

public class ItemModel : MonoBehaviour
{
    private PlayerController playerController;//플레이어는 어디에 있는가. 교체필요?

    public ItemEnum itemEnum;

    public void ApplyEffect()//아이템을 먹으면
    {
        
        GameManager.Instance.AddScore(500);

        switch (itemEnum)
        {
            case ItemEnum.HealPotion:
                HealPotion();
                break;
            case ItemEnum.SpeedPotion:
                StartCoroutine(SpeedPotion(5));
                break;
            case ItemEnum.GiantPotion:
                StartCoroutine(GiantPotion(5));
                break;
                //case ItemEnum.Bomb:
                //    Bomb();
                //    break;
                //case ItemEnum.Magnet:
                //    Magnet();
                //    break;


        }

    }
    private IEnumerator BlinkEffect()//깜빡이는 코루틴. 지피티피셜이라 실험 필요
    {
        SpriteRenderer sr = playerController.GetComponentInChildren<SpriteRenderer>(); // 플레이어의 SpriteRenderer 가져옴
        Color originalColor = sr.color;            // 원래 색 저장

        while (true) // StopCoroutine 될 때까지 계속 반복
        {
            sr.color = new Color(1f, 1f, 1f, 0.3f); // 반투명
            yield return new WaitForSeconds(0.3f); // 0.3초 대기

            sr.color = originalColor;              // 원래 색으로
            yield return new WaitForSeconds(0.3f); // 또 0.3초 대기
        }
    }

    private IEnumerator SpeedPotion(int duration)//광?속질주
    {
        playerController.SetInvincible(true); ;//무적
        playerController.SetSpeed(15) ;       //이동속도 증가, 빨라지는 동안 파티클처럼 이펙트 나오면 좋?을듯, 속도 적당한지 실험필요

        yield return new WaitForSeconds(duration);//발동되면 위에거 적용 후 지속시간(duration)후에 밑에거 적용

        playerController.SetSpeed(10);//다시 감소, 무적은 1.5초후에 풀림
        Coroutine blink = StartCoroutine(BlinkEffect());//깜빡이는 이펙트 적용

        yield return new WaitForSeconds(1.5f);//1.5초후 다시 무적해제 

        StopCoroutine(blink);          // 깜빡이기 종료
        playerController.GetComponentInChildren<SpriteRenderer>().color = Color.white; // 색상 원상복구
        playerController.SetInvincible(false); ;//무적 해제

    }


    private IEnumerator GiantPotion(int duration)//커져라~
    {
        BoxCollider2D collider = playerController.GetComponent<BoxCollider2D>();
        playerController.SetInvincible(true); ;//무적
        collider.size *= 2f;    // 박스 콜라이더 크기 2배
        collider.offset *= 2f;  // 위치도 같이 맞춰줘야 한다고 지피티가 그랬어요
        playerController.transform.localScale *= 2f;//플레이어의 크기 2배

        yield return new WaitForSeconds(duration);//지속시간 후에 다음거 적용

        collider.size /= 2f;    // 박스 콜라이더 크기 원래대로
        collider.offset /= 2f;  // 위치
        playerController.transform.localScale /= 2f;//플레이어의 크기 원래대로
        Coroutine blink = StartCoroutine(BlinkEffect());

        yield return new WaitForSeconds(1.5f);

        StopCoroutine(blink);          // 깜빡이기 종료
        playerController.GetComponentInChildren<SpriteRenderer>().color = Color.white; // 색상 원상복구
        playerController.SetInvincible(false); ;//무적 해제


    }

    //public void Bomb()
    //{

    //}

    //public void Magnet()
    //{

    //}



    public void HealPotion()
    {
        playerController.Heal(1);//이런식으로 쓰면 되나?

        //+ UI의 하트도 하나 추가
    }
}

/*
PlayerController.cs에 추가할것? 추가하면 오류 없어짐...

public bool IsInvincible { get; set; } = false; // ----------추가 무적인지 확인용 bool


public void TakeDamage(int damage)
{
    if (IsInvincible)//-----------이 줄과 아랫줄 추가
        return; // 무적 상태면 리턴으로 데미지 무시

    model.TakeDamage(damage);

    if (model.CurrentHealth <= 0)
    {
        Die();
    }
}







*/




//public class ItemModel : MonoBehaviour//아래쪽은 지피티가 추천해준 코드
//{

//    public ItemEnum itemEnum;
//    public float duration; // 지속시간 (필요 없는 경우는 0)
//    public int healAmount; // 회복량 (힐 전용)
//    public int itemScore;//아이템 획득시 점수 추가?

//    public void ApplyEffect(PlayerController player)
//    {
//        switch (itemEnum)
//        {
//            case ItemEnum.GiantPotion:
//                StartCoroutine(GiantEffect(player));
//                break;
//            case ItemEnum.SpeedPotion:
//                StartCoroutine(SpeedEffect(player));
//                break;
//            case ItemEnum.HealPotion:
//                player.Heal(healAmount);
//                break;
//            case ItemEnum.Bomb:
//                StartCoroutine(BombEffect(player)); 
//                break;
//            case ItemEnum.Magnet:
//                StartCoroutine(MagnetEffect(player)); 
//                break;
//        }
//    }


//   private IEnumerator GiantEffect(PlayerController player)
//    {
//        player.IsInvincible = true;
//        player.transform.localScale *= 2f;
//        yield return new WaitForSeconds(duration);
//        player.transform.localScale /= 2f;
//        player.IsInvincible = false;
//    }

//    private IEnumerator SpeedEffect(PlayerController player)
//    {
//        player.IsInvincible = true;
//        player.speed += 3f;
//        yield return new WaitForSeconds(duration);
//        player.speed -= 3f;
//        player.IsInvincible = false;
//    }
//private IEnumerator BombEffect(PlayerController player)
//{
//    // 플레이어 전방 방향
//    Vector2 origin = player.transform.position;
//    Vector2 direction = player.transform.right;

//    // 일정 범위 내 모든 충돌체 감지 (전방 5f 거리, 반지름 1.5f)
//    RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, 1.5f, direction, 5f);

//    foreach (var hit in hits)
//    {
//        // 장애물 오브젝트인지 확인 (태그 "Obstacle" 필요)
//        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
//        {
//            Destroy(hit.collider.gameObject); // 장애물 파괴
//            player.AddScore(itemScore);       // 점수 추가
//        }
//    }

//    yield return null; // 코루틴 종료 (지연 없음)
//}
//private IEnumerator MagnetEffect(PlayerController player)
//{
//    float radius = 5f;           // 자석 범위 반경
//    float pullSpeed = 10f;       // 끌려오는 속도
//    float timer = 0f;            // 타이머 초기화

//    while (timer < duration)
//    {
//        // 주변에 있는 모든 콜렉터블 감지 (Collectable 태그 필요)
//        Collider2D[] items = Physics2D.OverlapCircleAll(player.transform.position, radius);

//        foreach (Collider2D item in items)
//        {
//            if (item.CompareTag("Collectable"))
//            {
//                // 플레이어를 향해 이동 (자석처럼 끌림)
//                Vector3 dir = (player.transform.position - item.transform.position).normalized;
//                item.transform.position += dir * pullSpeed * Time.deltaTime;
//            }
//        }

//        timer += Time.deltaTime;    // 타이머 갱신
//        yield return null;         // 다음 프레임까지 대기
//    }
//}
//}
