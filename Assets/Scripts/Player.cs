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
    private bool isTransforming;

    public bool IsInBodyArea
    {
        get => isInBodyArea;
        private set 
        {
            isInBodyArea = value;
            sRenderer.flipX = true;
            animator.SetBool("InBodyArea", value);

            currrentArea = isInBodyArea ? Power.Type.Body : Power.Type.Mind;
            reflector.SetupColliders();

            isTransforming = true;
            animator.SetTrigger("Transform");
            CancelInvoke(nameof(Shoot));
        }
    }

    public bool IsReflecting
    {
        get => isReflecting;
        private set 
        {
            isReflecting = value;
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

        InvokeRepeating(nameof(Shoot), 0f, shootCooldown);
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
        if (Input.GetKeyDown(KeyCode.Z) && !IsReflecting && !IsUsingPower && !isTransforming)
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
        if (Input.GetKeyDown(KeyCode.X) && !IsReflecting && !IsUsingPower && !isTransforming)
        {
            IsReflecting = true;
        }
    }

    void FixedUpdate()
    {
        if (!IsReflecting && !IsUsingPower && !isTransforming)
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

    public void OnReflectionStart()
    {
        reflector.gameObject.SetActive(true);
    }

    public void OnReflectionEnd()
    {
        IsReflecting = false;
        reflector.gameObject.SetActive(false);
    }

    public void OnDamageEnd()
    {
        isInvincible = false;
    }

    public void OnTransformEnd()
    {
        isTransforming = false;
        InvokeRepeating(nameof(Shoot), 0f, shootCooldown);
        sRenderer.flipX = IsInBodyArea;
    }
}
