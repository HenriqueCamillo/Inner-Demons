using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindBoss : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float minWaitBetweenAttacks;
    [SerializeField] float maxWaitBetweenAttacks;

    [Header("Ground Stomp")]
    [SerializeField] GameObject groundStompPrefab;
    [SerializeField] Vector2 groundStompVelocity;
    [SerializeField] int minStomps, maxStomps;
    [SerializeField] Transform leftHandSpawnOrigin, rightHandSpawnOrigin;

    enum Attack { GroundStomp, ProjectileRain, ParryableProjectile, TelegraphedHits }
    Attack[] attacks;

    private int stompLimit, stompCounter;
    private int StompCounter
    {
        get => stompCounter;
        set
        {
            stompCounter = value;
            if(stompCounter >= stompLimit)
                StartIdle();
        }
    }

    private Animator _animator;
    bool isIdle = true;

    public void LeftStomp()
    {
        Instantiate(groundStompPrefab, leftHandSpawnOrigin.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = groundStompVelocity;
        StompCounter++;
    }

    public void RightStomp()
    {
        Instantiate(groundStompPrefab, rightHandSpawnOrigin.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = groundStompVelocity;
        StompCounter++;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        attacks = new Attack[4] {Attack.GroundStomp, Attack.ParryableProjectile, Attack.ProjectileRain, Attack.TelegraphedHits};
    }

    private void Start()
    {
        StartStomps();
    }

    private void StartStomps()
    {
        Debug.Log("Starting stopms");
        stompCounter = 0;
        stompLimit = Random.Range(minStomps, maxStomps + 1);
        _animator.Play("GroundStomp");
    }

    private void StartIdle()
    {
        _animator.Play("Idle");

        float wait = Random.Range(minWaitBetweenAttacks, maxWaitBetweenAttacks);

        Attack nextAttack = attacks[Random.Range(0, attacks.Length)];
        switch(nextAttack)
        {
            case Attack.GroundStomp:
                Invoke(nameof(StartStomps), wait);
                break;
            default:
                Invoke(nameof(StartStomps), wait);
                break;
        }
    }
}
