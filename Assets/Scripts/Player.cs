using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rBody;
    private SpriteRenderer sRenderer;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] float speed;
    private Vector2 movement;

    [Header("Areas")]
    [SerializeField] bool isInBodyArea;
    [SerializeField] Transform centerLine;
    public Power.Type currrentArea;

    [Space(5f)]
    [Header("Shoot")]
    [SerializeField] GameObject projectile;
    [SerializeField] float shootCooldown;


    [Space(5)]
    [Header("Prop reflection")]
    [SerializeField] Reflector reflector;
    private bool isReflecting;

    private bool isUsingPower;
    private bool isInvincible;

    public bool IsInBodyArea
    {
        get => isInBodyArea;
        private set 
        {
            isInBodyArea = value;
            sRenderer.flipX = value;
            animator.SetBool("InBodyArea", value);

            currrentArea = isInBodyArea ? Power.Type.Body : Power.Type.Mind;
            reflector.SetupColliders();
        }
    }

    public bool IsReflecting
    {
        get => isReflecting;
        private set 
        {
            isReflecting = value;
            reflector.gameObject.SetActive(value);
            animator.SetBool("Reflecting", value);
        }
    }

    public bool IsUsingPower
    {
        get => isUsingPower;
        private set 
        {
            isUsingPower = value;
            animator.SetBool("UsingPower", value);
        }
    }

    void Start()
    {
        rBody       = GetComponent<Rigidbody2D>();
        sRenderer   = GetComponent<SpriteRenderer>();
        animator    = GetComponent<Animator>();

        InvokeRepeating(nameof(Shoot), shootCooldown, shootCooldown);
    }

    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        animator.SetBool("Moving", movement != Vector2.zero);

        if (!IsInBodyArea && this.transform.position.x < centerLine.transform.position.x)
            IsInBodyArea = true;
        else if (IsInBodyArea && this.transform.position.x > centerLine.transform.position.x)
            IsInBodyArea = false;

        // TODO add controller support
        if (Input.GetKeyDown(KeyCode.Z) && !IsReflecting && !IsUsingPower)
        {
            if (!IsInBodyArea && PowerGaugeManager.instance.mindPowerReady)
            {
                IsUsingPower = true;
                PowerGaugeManager.instance.UsePower(Power.Type.Mind);
                // TODO animation event
                Invoke(nameof(OnPowerEnd), .5f);
            }
            else if (IsInBodyArea && PowerGaugeManager.instance.bodyPowerReady)
            {
                IsUsingPower = true;
                PowerGaugeManager.instance.UsePower(Power.Type.Body);
                // TODO animation event
                Invoke(nameof(OnPowerEnd), .5f);
            }
        }

        // TODO add controller support
        if (Input.GetKeyDown(KeyCode.X) && !IsReflecting && !IsUsingPower)
        {
            IsReflecting = true;
            // TODO animation event
            Invoke(nameof(OnReflectionEnd), .5f);
        }
    }

    void FixedUpdate()
    {
        if (!IsReflecting && !IsUsingPower)
            rBody.MovePosition(this.transform.position + (Vector3)movement * speed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        Quaternion rotation = IsInBodyArea ? Quaternion.Euler(180f, 180f, 0f) : Quaternion.identity;
        Instantiate(projectile, this.transform.position, rotation);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            GameManager.instance.GameOver();
        }
    }

    public void TakeDamage()
    {
        if (!isInvincible)
        {
            IsReflecting = false;
            IsUsingPower = false;
            isInvincible = true;

            animator.SetTrigger("Damage");
            BossesManager.instance.GetBoss(currrentArea).HitGrow();
        }
    }

    public void OnPowerEnd()
    {
        IsUsingPower = false;
    }

    public void OnReflectionEnd()
    {
        IsReflecting = false;
    }

    public void OnDamageEnd()
    {
        isInvincible = false;
    }
}
