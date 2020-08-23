using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField] Rigidbody2D rBody;
    [SerializeField] Power.Type powerType;
    private Throwable.Type throwableType = Throwable.Type.Prop;
    private bool reflected = false;

    void OnEnable()
    {
        if (powerType == Power.Type.Mind)
            PowerGaugeManager.instance.OnMindPowerUsed += SelfDestruct;
        else if (powerType == Power.Type.Body)
            PowerGaugeManager.instance.OnBodyPowerUsed += SelfDestruct;
    }

    void OnDisable()
    {
        if (powerType == Power.Type.Mind)
            PowerGaugeManager.instance.OnMindPowerUsed -= SelfDestruct;
        else if (powerType == Power.Type.Body)
            PowerGaugeManager.instance.OnBodyPowerUsed -= SelfDestruct;
    }

    public void Reflect(float force)
    {
        reflected = true;

        // TODO extra change direction rule
        Vector2 direction = powerType == Power.Type.Mind ? Vector2.right : Vector2.left;
        rBody.velocity = direction * force;
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

    public void SelfDestructAfter(float time)
    {
        Invoke(nameof(SelfDestruct), time);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (reflected && other.CompareTag("Boss"))
        {
            PowerGaugeManager.instance.AddGaugeBonus(powerType, throwableType);
            BossesManager.instance.GetBoss(powerType).Shrink(throwableType);
            Destroy(this.gameObject);
        }
        else if (!reflected && other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(powerType);
        }
    }
}
