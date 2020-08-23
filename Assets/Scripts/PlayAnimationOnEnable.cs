using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnEnable : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();   
    }

    void OnEnable()
    {
        animator.Play("Spawn");
    }
}
