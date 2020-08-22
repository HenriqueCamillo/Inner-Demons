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

    [Header("Different Sides")]
    [SerializeField] bool isInMindArea;
    [SerializeField] Transform centerLine;

    [Space(5f)]
    [Header("Shoot")]
    [SerializeField] GameObject projectile;
    [SerializeField] float shootCooldown;

    [Space(5)]
    [Header("Special Attacks")]
    [SerializeField] float bodyPowerGauge;
    [SerializeField] float mindPowerGauge;

    public bool IsInMindArea
    {
        get => isInMindArea;
        private set 
        {
            isInMindArea = value;
            animator.SetBool("InMindArea", value);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        rBody       = GetComponent<Rigidbody2D>();
        sRenderer   = GetComponent<SpriteRenderer>();
        animator    = GetComponent<Animator>();

        InvokeRepeating(nameof(Shoot), shootCooldown, shootCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!IsInMindArea && this.transform.position.x < centerLine.transform.position.x)
            IsInMindArea = true;
        else if (IsInMindArea && this.transform.position.x > centerLine.transform.position.x)
            IsInMindArea = false;
    }

    void FixedUpdate()
    {
        rBody.MovePosition(this.transform.position + (Vector3)movement * speed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        Quaternion rotation = IsInMindArea ? Quaternion.Euler(180f, 180f, 0f) : Quaternion.identity;
        Instantiate(projectile, this.transform.position, rotation);
    }
}
