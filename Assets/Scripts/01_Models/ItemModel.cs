using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
    Coin,
    HealPotion,
    SpeedPotion,
    GiantPotion,
    Magnet
    //Bomb
}

public class ItemModel : MonoBehaviour
{
    private ItemManager pool;
    private void Awake()
    {
        pool = FindObjectOfType<ItemManager>();
        playerController = FindAnyObjectByType<PlayerController>();
    }
    UIManager UIManager;

    public ItemEnum itemEnum;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerModel model;

    public void ApplyEffect()//아이템을 먹으면
    {
        Debug.Log("아이템 효과 실행됨: " + itemEnum);
        GameManager.Instance.AddScore(100);

        switch (itemEnum)
        {
            case ItemEnum.Coin:
                GameManager.Instance.AddScore(100);
                pool.ReturnToPool(itemEnum, gameObject);
                break;
            case ItemEnum.HealPotion:
                HealPotion(2);
                break;
            case ItemEnum.SpeedPotion:
                StartCoroutine(SpeedPotion(3));
                break;
            case ItemEnum.GiantPotion:
                StartCoroutine(GiantPotion(3));
                break;
            case ItemEnum.Magnet:
                StartCoroutine(Magnet(5));
                break;
                //case ItemEnum.Bomb:
                //    Bomb();
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
            yield return new WaitForSeconds(0.1f); // 0.3초 대기

            sr.color = originalColor;              // 원래 색으로
            yield return new WaitForSeconds(0.1f); // 또 0.3초 대기
        }
    }
    
    private IEnumerator SpeedPotion(int duration)//광?속질주
    {
        playerController.SetInvincible(true); ;//무적
        playerController.SetSpeed(13) ;       //이동속도 증가, 빨라지는 동안 파티클처럼 이펙트 나오면 좋?을듯, 속도 적당한지 실험필요

        yield return new WaitForSeconds(duration);//발동되면 위에거 적용 후 지속시간(duration)후에 밑에거 적용

        playerController.SetSpeed(8.5f);//다시 감소, 무적은 1.5초후에 풀림
        Coroutine blink = StartCoroutine(BlinkEffect());//깜빡이는 이펙트 적용

        yield return new WaitForSeconds(1.5f);//1.5초후 다시 무적해제 

        StopCoroutine(blink);          // 깜빡이기 종료
        playerController.GetComponentInChildren<SpriteRenderer>().color = Color.white; // 색상 원상복구
        playerController.SetInvincible(false); ;//무적 해제

        pool.ReturnToPool(itemEnum, gameObject);//디스트로이, SetActiveFalse 대신 아이템 풀링 풀에 반환.ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    }


    private IEnumerator GiantPotion(int duration)//커져라~
    {
        Debug.Log("자이언트 코루틴 시작됨");
        BoxCollider2D collider = playerController.GetComponent<BoxCollider2D>();
        playerController.SetInvincible(true); ;//무적
        //collider.size *= 1f;    // 박스 콜라이더 크기 2배
        //collider.offset *= 1f;  // 위치도 같이 맞춰줘야 한다고 지피티가 그랬어요
        playerController.transform.localScale *= 2f;//플레이어의 크기 2배
        
        yield return new WaitForSeconds(duration);//지속시간 후에 다음거 적용

        Debug.Log("자이언트 코루틴 끝남");
        //collider.size /= 1f;    // 박스 콜라이더 크기 원래대로
        //collider.offset /= 1f;  // 위치
        playerController.transform.localScale /= 2f;//플레이어의 크기 원래대로
        Coroutine blink = StartCoroutine(BlinkEffect());

        Debug.Log("블링크 시작");

        yield return new WaitForSeconds(1.5f);

        Debug.Log("블링크 끝");

        StopCoroutine(blink);          // 깜빡이기 종료
        playerController.GetComponentInChildren<SpriteRenderer>().color = Color.white; // 색상 원상복구
        playerController.SetInvincible(false); ;//무적 해제

        pool.ReturnToPool(itemEnum, gameObject);
    }

    private IEnumerator Magnet(int duration)
    {
        float radius = 10f;           // 자석 범위 반경
        float pullSpeed = 15f;       // 끌려오는 속도
        float timer = 0f;            // 타이머 초기화


        while (timer < duration)//duration 만큼 
        {
            //Physics2D.OverlapCircleAll(특정위치, 반지름의 크기) = 특정 위치를 중심으로 radius만큼의 원형 범위 안에 있는 모든 Collider2D를 찾아줌.
            Collider2D[] items = Physics2D.OverlapCircleAll(playerController.transform.position, radius);//플레이어 중심 radius범위 안에 콜라이더들을 검사

            foreach (Collider2D item in items)
            {
                if (item.CompareTag("Collectable"))//콜렉터블 이라면
                {
                    //Vector dir = ( 타겟위치 - 현재위치).normalized.     normalized = 벡터를 길이 1짜리 벡터로 바꿈
                    Vector3 dir = (playerController.transform.position - item.transform.position).normalized;// 이동 방향을 결정
                    item.transform.position += dir * pullSpeed * Time.deltaTime;//아이템포지션을 바꿔서 끌어오기
                }
            }

            yield return null;         // 다음 프레임까지 대기
            timer += Time.deltaTime;    // 타이머 갱신
        }
        pool.ReturnToPool(itemEnum, gameObject);
    }


    public void HealPotion(int heal)
    {
        //model.Heal(heal);
        //int updateHp = model.CurrentHealth;

        //playerController.Heal(heal);
        Debug.Log("힐 됨");
        GameManager.Instance.Heal(heal);

        //UIManager.Instance.UpdateHealth(updateHp); //UI 업데이트

        pool.ReturnToPool(itemEnum, gameObject);
    }
}



