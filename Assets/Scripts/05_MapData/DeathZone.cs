using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Transform player;
    private void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y - 10f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("���� ����: �÷��̾� ������");
            GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
        }
    }
}
