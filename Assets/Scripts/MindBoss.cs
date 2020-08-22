using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindBoss : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float minWaitBetweenAttacks;
    [SerializeField] float maxWaitBetweenAttacks;
    enum Attack { GroundStomp, ProjectileRain, ParryableProjectile, TelegraphedHits }
    Attack[] attacks;

    [Header("Ground Stomp")]
    [SerializeField] GameObject groundStompPrefab;
    [SerializeField] Vector2 groundStompVelocity;
    [SerializeField] int minStomps, maxStomps;
    [SerializeField] Transform leftHandSpawnOrigin, rightHandSpawnOrigin;
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

    [Header("Telegraphed Areas")]
    [SerializeField] GameObject[] propPrefabs;
    [SerializeField] float propSpawnInterval = 1f, parryableRate = 0.1f;
    [SerializeField] Vector2 propVelocity;
    [SerializeField] float propAngularVelocity;
    [SerializeField] Transform topSpawnMapLimit, bottomSpawnMapLimit;
    [SerializeField] int minTelegraphed, maxTelegraphed;
    enum Area { Top, Center, Bottom }
    Area[] areas;
    private int telegraphedLimit, telegraphedCounter;
    int telegraphedCountCurrentWaveLimit, currentTelegraphedCountCurrentWave;



    private Animator _animator;
    bool isIdle = true;

    public void LeftStomp()
    {
        Instantiate(groundStompPrefab, leftHandSpawnOrigin.position, Quaternion.identity, this.transform).GetComponent<Rigidbody2D>().velocity = groundStompVelocity;
        StompCounter++;
    }

    public void RightStomp()
    {
        Instantiate(groundStompPrefab, rightHandSpawnOrigin.position, Quaternion.identity, this.transform).GetComponent<Rigidbody2D>().velocity = groundStompVelocity;
        StompCounter++;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        attacks = new Attack[4] {Attack.GroundStomp, Attack.ParryableProjectile, Attack.ProjectileRain, Attack.TelegraphedHits};
        areas = new Area[3] {Area.Top, Area.Center, Area.Bottom};
    }

    private void Start()
    {
        StartIdle();
    }

    private void StartStomps()
    {
        stompCounter = 0;
        stompLimit = Random.Range(minStomps, maxStomps + 1);
        _animator.Play("GroundStomp", 0);
    }

    private void StartIdle()
    {
        _animator.Play("Idle", 0);

        float wait = Random.Range(minWaitBetweenAttacks, maxWaitBetweenAttacks);

        Attack nextAttack = attacks[Random.Range(0, attacks.Length)];
        switch(nextAttack)
        {
            case Attack.GroundStomp:
                Invoke(nameof(StartStomps), wait);
                break;
            case Attack.TelegraphedHits:
                Invoke(nameof(StartTelegraphed), wait);
                break;
            default:
                Invoke(nameof(StartTelegraphed), wait);
                break;
        }
    }

    private void SpawnProp()
    {
        Vector2 spawnPos = new Vector2(topSpawnMapLimit.position.x, Random.Range(bottomSpawnMapLimit.position.y, topSpawnMapLimit.position.x));
        GameObject propPrefab = propPrefabs[Random.Range(0, propPrefabs.Length)];

        GameObject spawnedProp = Instantiate(propPrefab, spawnPos, Quaternion.identity, this.transform);
        Rigidbody2D propRigid = spawnedProp.GetComponent<Rigidbody2D>();

        if(Random.Range(0f, 1f) <= parryableRate)
        {
            spawnedProp.layer = LayerMask.NameToLayer("Parryable Prop");
            spawnedProp.tag = "Parryable Prop";
            //!! TEMP
            spawnedProp.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            spawnedProp.layer = LayerMask.NameToLayer("Prop");
            spawnedProp.tag = "Prop";
        }

        propRigid.velocity = propVelocity;
        propRigid.angularVelocity = propAngularVelocity;

    }

    private void StartTelegraphed()
    {
        telegraphedCounter = 0;
        telegraphedLimit = Random.Range(minTelegraphed, maxTelegraphed + 1);
        InvokeRepeating(nameof(SpawnProp), 0f, propSpawnInterval);

        TelegraphedAttack();
    }
    private void EndTelegraph()
    {
        currentTelegraphedCountCurrentWave++;
        if(currentTelegraphedCountCurrentWave >= telegraphedCountCurrentWaveLimit)
        {
            telegraphedCounter++;
            TelegraphedAttack();
        }
    }

    private void TelegraphedAttack()
    {
        if(telegraphedCounter >= telegraphedLimit)
        {
            StartIdle();
            CancelInvoke(nameof(SpawnProp));
            return;
        }

        currentTelegraphedCountCurrentWave = 0;
        _animator.Play("Inactive", 3);
        _animator.Play("Inactive", 2);

        Area tel1Area, tel2Area;
        tel1Area = areas[Random.Range(0, areas.Length)];
        tel2Area = areas[Random.Range(0, areas.Length)];

        switch(tel1Area)
        {
            case Area.Top:
                _animator.Play("Telegraph Up", 2, 0f);
                break;
            case Area.Center:
                _animator.Play("Telegraph Center", 2, 0f);
                break;
            case Area.Bottom:
                _animator.Play("Telegraph Down", 2, 0f);
                break;
        }

        if(Mathf.Abs(tel1Area - tel2Area) > 1)
        {
            switch(tel2Area)
            {
                case Area.Top:
                    _animator.Play("Telegraph Up", 3, 0f);
                    break;
                case Area.Center:
                    _animator.Play("Telegraph Center", 3, 0f);
                    break;
                case Area.Bottom:
                    _animator.Play("Telegraph Down", 3, 0f);
                    break;
            }
            telegraphedCountCurrentWaveLimit = 2;
        }
        else
        {
            telegraphedCountCurrentWaveLimit = 1;
            _animator.Play("Inactive", 3);
        }
    }
}
