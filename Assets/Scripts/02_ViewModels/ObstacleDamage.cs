using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 스크립트를 붙이는 순간 Unity가 자동으로 그 게임오브젝트에 Collider2D 컴포넌트를 추가하도록 강제하기
[RequireComponent(typeof(Collider2D))]
public class ObstacleDamage : MonoBehaviour
{
    private int damage;    // 장애물이 플레이어에게 줄 피해량을 저장할 변수


    private void Awake()
    {
        // 장애물 게임 오브젝트가 생성됐을 때 damage에 각 장애물 타입에 맞는 Damage 넣어주기 
        if (gameObject.name.Contains("RedLine"))
            damage = new ObstacleModel(ObstacleType.RedLineTrap).Damage;
        else if (gameObject.name.Contains("Syntax"))
            damage = new ObstacleModel(ObstacleType.SyntaxErrorBox).Damage;
        else if (gameObject.name.Contains("Compile"))
            damage = new ObstacleModel(ObstacleType.CompileErrorWall).Damage;
        else
            damage = 1; // 장애물 종류에 없으면 1로 처리
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   
        // 닿은 게임 오브젝트가 Player 태그인지 확인
        if (other.CompareTag("Player"))
        {
            // 태그가 Player라면 해당 오브젝트의 PlayerController 컴포넌트를 가져오기
            var playerCtrl = other.GetComponent<PlayerController>();
            if (playerCtrl != null)
            {
                // PlayerController의 TakeDamage(int) 메서드를 호출해 데미지를 전달
                playerCtrl.TakeDamage(damage);
            }
        }
    }

}
