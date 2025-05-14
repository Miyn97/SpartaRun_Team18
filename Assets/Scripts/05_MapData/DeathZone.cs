using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float deathZoneY = -10f;

    private void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, deathZoneY, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("낙사 판정: 플레이어 감지됨");
            var dead = other.GetComponent<PlayerController>();
            if (dead != null)
            {
                dead.Die(); // 플레이어 사망처리 메서드 호출
            }
        }
    }
}
