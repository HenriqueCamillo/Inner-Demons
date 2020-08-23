using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleShowcase : MonoBehaviour
{
    [SerializeField] Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        float time = Random.Range(0f, 1f);
        animator.Play("Idle", 0, time);
    }
}
