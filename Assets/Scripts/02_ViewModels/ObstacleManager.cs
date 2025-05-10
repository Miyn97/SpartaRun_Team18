using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)                  // 장애물이 다른 게임 오브젝트와 충돌했을 때
    {
        PlayerModel player = collision.GetComponent<PlayerModel>();     // 부딪친 게임 오브젝트(여기서는 플레이어)의 컴포넌트 중
                                                                        // "PlayerModel.cs" 컴포넌트를 받아서 player 변수에 저장


        /*        if (player == true)*/                                             // 부딪친 게임 오브젝트의 태그가 플레이어라면
        {
            Debug.Log("플레이어와 충돌");                               // 충돌 메서드 작동 확인용 메시지


            if (player != null)                                         // 예외 처리 (부딪친 게임 오브젝트(플레이어)에 PlayerModel 스크립트가 있는지 확인)
            {
                //float originalSpeed = player.speed;                     // 맞은 시점의 속도를 저장하는 변수 
                //player.hp -= damage;                                    // hp 감소
                /*                StartCoroutine(SpeedDown(player, originalSpeed)); */      // SpeedDown 코루틴 호출
            }
        }
    }

    //IEnumerator SpeedDown(PlayerModel player, float originalSpeed)     // 장애물에 부딪쳤을 떄 잠시동안 속도를 감소 시키는 코루틴 함수 
    //{                                                                  // 속도가 감소 됐다가 서서히 원래 속도로 돌아온다.
    //    Debug.Log("속도 감소 코루틴");                                 // 속도 감소 메서드 작동 확인용 메시지
    //    // player.speed = 0.5f;
    //    yield return new WaitForSeconds(0.5f);
    //    // player.speed = originalSpeed;
    //}
}
