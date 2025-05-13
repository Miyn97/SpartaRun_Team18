using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private MonsterModel model;
    private MonsterView view;
    private Transform player;

    private void Awake()
    {
        model = GetComponent<MonsterModel>();
        view = GetComponent<MonsterView>();
        player = GameObject.FindWithTag("Player")?.transform;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.gravityScale = 0;
    }

    private void Update()
    {
        if (player == null || model == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += new Vector3(direction.x, 0f, 0f) * model.chaseSpeed * Time.deltaTime;

        view?.SetChasing(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            view?.SetChasing(false);
            GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
        }
    }
}

