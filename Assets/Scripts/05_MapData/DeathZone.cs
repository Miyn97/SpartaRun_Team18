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
            GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
        }
    }
}
