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
    [SerializeField] bool isInMindArea;
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

    public bool IsInMindArea
    {
        get => isInMindArea;
        private set 
        {
            isInMindArea = value;
            sRenderer.flipX = value;
            animator.SetBool("InMindArea", value);

            currrentArea = isInMindArea ? Power.Type.Mind : Power.Type.Body;
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

        if (!IsInMindArea && this.transform.position.x < centerLine.transform.position.x)
            IsInMindArea = true;
        else if (IsInMindArea && this.transform.position.x > centerLine.transform.position.x)
            IsInMindArea = false;

        // TODO add controller support
        if (Input.GetKeyDown(KeyCode.Z) && !IsReflecting && !IsUsingPower)
        {
            if (IsInMindArea && PowerGaugeManager.instance.mindPowerReady)
            {
                IsUsingPower = true;
                PowerGaugeManager.instance.UsePower(Power.Type.Mind);
                // TODO animation event
                Invoke(nameof(OnPowerEnd), .5f);
            }
            else if (!IsInMindArea && PowerGaugeManager.instance.bodyPowerReady)
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
        Quaternion rotation = IsInMindArea ? Quaternion.Euler(180f, 180f, 0f) : Quaternion.identity;
        Instantiate(projectile, this.transform.position, rotation);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            Debug.Log("Murreu");
        }
        else if (other.CompareTag("Prop") || other.CompareTag("Parryable Prop"))
        {
            IsReflecting = false;
            IsUsingPower = false;
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
}
