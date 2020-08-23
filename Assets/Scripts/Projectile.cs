using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rBody;
    [SerializeField] float offset;
    [SerializeField] float speed;
    private Power.Type powerType;
    private Throwable.Type throwableType = Throwable.Type.Projectile;

    void Awake()
    {
        rBody.velocity = this.transform.right * speed;

        if (rBody.velocity.x < 0)
        {
            powerType = Power.Type.Body;
            offset *= -1;
        }
        else 
            powerType = Power.Type.Mind;

        this.transform.position += Vector3.right * offset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            PowerGaugeManager.instance.AddGaugeBonus(powerType, throwableType);
            BossesManager.instance.GetBoss(powerType).Shrink(throwableType);
        }
        
        Destroy(this.gameObject);
    }
}
