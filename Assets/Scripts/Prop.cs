﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField] Rigidbody2D rBody;
    [SerializeField] Power.Type powerType;
    private Throwable.Type throwableType = Throwable.Type.Projectile;
    private bool reflected = false;

    void Start()
    {
        if (rBody == null)
            rBody = GetComponent<Rigidbody2D>();
    }

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
        Vector2 direction = powerType == Power.Type.Mind ? Vector2.left : Vector2.right;
        rBody.velocity = direction * force;
    }

    void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (reflected && other.CompareTag("Boss"))
        {
            PowerGaugeManager.instance.AddGaugeBonus(powerType, throwableType);
            BossesManager.instance.GetBoss(powerType).Shrink(throwableType);
            Destroy(this.gameObject);
        }
        else if (!reflected && other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage();
        }
    }
}
