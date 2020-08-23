using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesRandomAnimationStart : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] MindBoss boss;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (boss == null)
            boss = BossesManager.instance.mindBoss.GetComponent<MindBoss>();

        float time = Random.Range(0f, 1f);
        animator.Play("TentaclesIdle", 0, time);

    }

    void OnEnable()
    {
        boss.OnPropWavesPreparation += Pause;
        boss.OnPropWavesStart += Accelerate;
        boss.OnPropWavesEnd += Reset;
    }

    void OnDisable()
    {
        boss.OnPropWavesPreparation -= Pause;
        boss.OnPropWavesStart -= Accelerate;
        boss.OnPropWavesEnd -= Reset;
    }

    void Pause()
    {
        animator.enabled = false;
    }

    void Accelerate()
    {
        animator.enabled = true;
        animator.SetFloat("speed", 2f);
    }

    void Reset()
    {
        animator.SetFloat("speed", 1f);
    }
}