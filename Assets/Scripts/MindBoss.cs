using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindBoss : Boss
{
    public event System.Action OnPropWavesPreparation;
    public event System.Action OnPropWavesStart;
    public event System.Action OnPropWavesEnd;

    [Header("General")]
    [SerializeField] float minWaitBetweenAttacks;
    [SerializeField] float maxWaitBetweenAttacks;
    [SerializeField] Transform spawnsParent;
    enum Attack { GroundStomp, PropWaves, TentacleFrenzy, TelegraphedHits }
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
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Vector2 projectileVelocity;
    [SerializeField] float projectileSpawnInterval = 1f;
    [SerializeField] float propAngularVelocity;
    [SerializeField] Transform topSpawnMapLimit, bottomSpawnMapLimit;
    [SerializeField] int minTelegraphed, maxTelegraphed;
    enum Area { Top, Center, Bottom }
    Area[] areas;
    private int telegraphedLimit, telegraphedCounter;
    int telegraphedCountCurrentWaveLimit, currentTelegraphedCountCurrentWave;

    [Header("Prop Waves")]
    [SerializeField] GameObject[] propPrefabs;
    [SerializeField] float propSpawnInterval = 1f;
    [SerializeField] Vector2 propVelocity;
    [SerializeField] int maxParryablePropDistance, propQuantity;
    [SerializeField] int minWaves, maxWaves;
    private int lastParryableIndex;
    private int waveLimit, waveCounter;
    private int WaveCounter
    {
        get => waveCounter;
        set
        {
            waveCounter = value;
            if(waveCounter >= waveLimit)
            {
                OnPropWavesEnd?.Invoke();
                CancelInvoke(nameof(SpawnWave));
                StartIdle();
            }
        }
    }

    [Header("Tentacle Frenzy")]
    [SerializeField] GameObject tentaclePrefab;
    [SerializeField] float tentacleSpawnInterval = 1f;
    [SerializeField] int minTentacles, maxTentacles;
    [SerializeField] float tentacleOffset;
    private int tentacleLimit, tentacleCounter;
    private int TentacleCounter
    {
        get => tentacleCounter;
        set
        {
            tentacleCounter = value;
            if(tentacleCounter >= tentacleLimit)
            {
                CancelInvoke(nameof(SpawnTentacle));
                StartIdle();
            }
        }
    }

    public void LeftStomp()
    {
        Instantiate(groundStompPrefab, leftHandSpawnOrigin.position, Quaternion.identity, spawnsParent).GetComponent<Rigidbody2D>().velocity = groundStompVelocity;
        StompCounter++;
    }

    public void RightStomp()
    {
        Instantiate(groundStompPrefab, rightHandSpawnOrigin.position, Quaternion.identity, spawnsParent).GetComponent<Rigidbody2D>().velocity = groundStompVelocity;
        StompCounter++;
    }

    protected override void Awake()
    {
        base.Awake();
        attacks = new Attack[4] {Attack.GroundStomp, Attack.TentacleFrenzy, Attack.PropWaves, Attack.TelegraphedHits};
        areas = new Area[3] {Area.Top, Area.Center, Area.Bottom};
    }

    protected override void Start()
    {
        base.Start();
        StartIdle();
        _animator.ResetTrigger("Idle");
    }

    private void StartStomps()
    {
        stompCounter = 0;
        stompLimit = Random.Range(minStomps, maxStomps + 1);
        _animator.Play("Ground Stomps", 4);
        _animator.Play("Attack", 0);
    }

    private void StartIdle()
    {
        _animator.Play("Idle", 0);
        // _animator.Play("Idle", 4);
        _animator.SetTrigger("Idle");

        float wait = Random.Range(minWaitBetweenAttacks, maxWaitBetweenAttacks);

        Attack nextAttack = attacks[Random.Range(0, attacks.Length)];
        Debug.Log("Next attack: " + nextAttack);
        switch(nextAttack)
        {
            // case Attack.GroundStomp:
            //     Invoke(nameof(StartStomps), wait);
            //     break;
            // case Attack.TelegraphedHits:
            //     Invoke(nameof(StartTelegraphed), wait);
            //     break;
            // case Attack.PropWaves:
            //     Invoke(nameof(StartPropWaves), wait);
            //     break;
            // case Attack.TentacleFrenzy:
            //     Invoke(nameof(StartTentacleFrenzy), wait);
            //     break;
            default:
                Invoke(nameof(StartStomps), wait);
                break;
        }
    }

    private void SpawnProjectile()
    {
        Vector2 spawnPos = new Vector2(topSpawnMapLimit.position.x, Random.Range(bottomSpawnMapLimit.position.y, topSpawnMapLimit.position.x));
        GameObject spawnedProjectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity, spawnsParent);
        Rigidbody2D projectileRigid = spawnedProjectile.GetComponent<Rigidbody2D>();

        projectileRigid.velocity = projectileVelocity;
    }

    private void StartTelegraphed()
    {
        telegraphedCounter = 0;
        telegraphedLimit = Random.Range(minTelegraphed, maxTelegraphed + 1);
        InvokeRepeating(nameof(SpawnProjectile), 1f, projectileSpawnInterval);

        _animator.Play("Telegraphed Attacks", 4);
        _animator.Play("Attack", 0);

        Invoke(nameof(TelegraphedAttack), 1.5f);
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
            CancelInvoke(nameof(SpawnProjectile));
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

    private void SpawnWave()
    {
        GameObject propPrefab;
        GameObject[] spawnedProps = new GameObject[propQuantity];
        Vector2 spawnPos = new Vector2(topSpawnMapLimit.position.x, 0f);
        
        for(int i = 0; i < propQuantity; i++)
        {
            spawnPos.y = ((topSpawnMapLimit.position.y - bottomSpawnMapLimit.position.y) / propQuantity) * (i + 1) + bottomSpawnMapLimit.position.y;
            propPrefab = propPrefabs[Random.Range(0, propPrefabs.Length)];
            spawnedProps[i] = Instantiate(propPrefab, spawnPos, Quaternion.identity, spawnsParent);
            Rigidbody2D propRigid = spawnedProps[i].GetComponent<Rigidbody2D>();
            propRigid.velocity = propVelocity;
            propRigid.angularVelocity = propAngularVelocity;
        }
        
        int newParryableIndex;
        do newParryableIndex = Random.Range(0, propQuantity);
        while(Mathf.Abs(newParryableIndex - lastParryableIndex) > maxParryablePropDistance);

        spawnedProps[newParryableIndex].layer = LayerMask.NameToLayer("Parryable Prop");
        spawnedProps[newParryableIndex].tag = "Parryable Prop";
        //!! TEMP
        spawnedProps[newParryableIndex].GetComponent<SpriteRenderer>().color = Color.yellow;

        lastParryableIndex = newParryableIndex;
        WaveCounter++;
    }

    private void StartPropWaves()
    {
        waveCounter = 0;
        waveLimit = Random.Range(minWaves, maxWaves + 1);

        OnPropWavesPreparation?.Invoke();
        InvokeRepeating(nameof(SpawnWave), 0.5f, propSpawnInterval);
        Invoke(nameof(PlayPropWavesAnimation), 0.5f);
        _animator.Play("Attack", 0);
    }

    private void PlayPropWavesAnimation()
    {
        _animator.Play("Prop Waves", 4);
        OnPropWavesStart?.Invoke();
    }

    private void SpawnTentacle()
    {
        if(BossesManager.instance.player.currrentArea == area)
            Instantiate(tentaclePrefab, BossesManager.instance.player.transform.position + Vector3.down * tentacleOffset, Quaternion.identity, spawnsParent);
        
        TentacleCounter++;
    }

    private void StartTentacleFrenzy()
    {
        tentacleCounter = 0;
        tentacleLimit = Random.Range(minTentacles, maxTentacles + 1);

        InvokeRepeating(nameof(SpawnTentacle), 1f, tentacleSpawnInterval);
        _animator.Play("Tentacle Frenzy", 4);
        _animator.Play("Attack", 0);
    }

}
