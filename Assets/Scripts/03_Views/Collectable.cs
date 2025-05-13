using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    //충돌시 효과 적용 및 파괴처리, 과제가 어렵다면 코드 참고함
    //아이템에 이 스크립트랑 아이템 모델 넣고 is trigger 켜두기
    //아이템 프리펩에 태그 Collectable
    private ItemManager pool;
    private void Awake()
    {
        pool = FindObjectOfType<ItemManager>();
    }

    // 플레이어가 트리거에 들어왔을 때 실행되는 함수
    private void OnTriggerEnter2D(Collider2D other)//OnTriggerEnter2D는 2D Trigger 충돌이 일어났을 때 자동으로 호출해주는 유니티 내장 함수
    {
        if (other.CompareTag("Player"))//만약 충돌한 게임오브젝트의 태그가 플레이어라면
        {
            ItemModel item = GetComponent<ItemModel>();
            if (item != null)
            {
                item.ApplyEffect();  // 아이템모델에 있는 효과 발동


                Vector3 pos = item.transform.position;
                pos.y += 30f;
                item.transform.position = pos;
            }
        }
    }
}
