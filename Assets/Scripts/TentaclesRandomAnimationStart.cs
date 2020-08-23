using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesRandomAnimationStart : MonoBehaviour
{
    [SerializeField] Animator animator;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        float time = Random.Range(0f, 1f);
        animator.Play("TentaclesIdle", 0, time);
    }
}