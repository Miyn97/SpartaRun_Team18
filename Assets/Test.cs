using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameManager gameManager;


    void Update()
    {
        // �����̽��� ������ +10��
        if (Input.GetKeyDown(KeyCode.Space))
            gameManager.AddScore(10);

        if (Input.GetKeyDown(KeyCode.H))
        {
            gameManager.TakeDamage(1);
        }
    }
}
