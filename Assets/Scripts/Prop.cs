using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField] Rigidbody2D rBody;
    [SerializeField] float speed;
    [SerializeField] Power.Type powerType;
    private Throwable.Type throwableType = Throwable.Type.Projectile;
    private bool reflected = false;

    void Start()
    {
        if (rBody == null)
            rBody = GetComponent<Rigidbody2D>();
    }

    public void Reflect(float force)
    {
        reflected = true;

        // TODO extra change direction rule
        Vector2 direction = powerType == Power.Type.Mind ? Vector2.left : Vector2.right;
        rBody.velocity = direction * force;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (reflected && other.CompareTag("Boss"))
        {
            PowerGaugeManager.instance.AddGaugeBonus(powerType, throwableType);
            BossesManager.instance.GetBoss(powerType).Shrink(powerType, throwableType);
            Destroy(this.gameObject);
        }
    }
}
