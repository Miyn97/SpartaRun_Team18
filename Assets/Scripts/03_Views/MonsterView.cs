using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterView : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetChasing(bool isChasing)
    {
        if (animator != null)
            animator.SetBool("IsChase", isChasing);
    }
}
