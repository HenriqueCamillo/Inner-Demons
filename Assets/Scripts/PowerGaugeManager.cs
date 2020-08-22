using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PowerGaugeManager : MonoBehaviour
{
    public static PowerGaugeManager instance;

    [Header("Body Power Gauge")]
    [SerializeField] float maxBodyPower;
    [SerializeField] Image bodyPowerBar;
    private float bodyPower;
    public bool bodyPowerReady;

    [Space(5)]
    [Header("Mind Power Gauge")]
    [SerializeField] float maxMindPower;
    [SerializeField] Image mindPowerBar;
    private float mindPower;
    public bool mindPowerReady;

    [Space(5)]
    [Header("Gauge Bonuses")]
    [SerializeField] float projectileBonus;
    [SerializeField] float propBonus;

    public event Action OnMindPowerUsed;
    public event Action OnBodyPowerUsed;

    public float BodyPower
    {
        get => bodyPower;
        set 
        {
            bodyPower = Mathf.Clamp(value, 0f, maxBodyPower);
            bodyPowerBar.fillAmount = bodyPower / maxBodyPower;
            bodyPowerReady = bodyPower == maxBodyPower;
        }
    }

    public float MindPower
    {
        get => mindPower;
        set 
        {
            mindPower = Mathf.Clamp(value, 0f, maxMindPower);
            mindPowerBar.fillAmount = mindPower / maxMindPower;
            mindPowerReady = mindPower == maxMindPower;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        BodyPower = 0f;
        MindPower = 0f;
    }

    public void AddGaugeBonus(Power.Type powerType, Throwable.Type bonusType)
    {
        float bonus = bonusType == Throwable.Type.Projectile ? projectileBonus : propBonus;

        if (powerType == Power.Type.Body)
            BodyPower += bonus;
        else if (powerType == Power.Type.Mind)
            MindPower += bonus;
    }

    public void UsePower(Power.Type powerType)
    {
        if (powerType == Power.Type.Body)
        {
            Debug.Log("Used body power");
            BodyPower = 0f;
            OnBodyPowerUsed?.Invoke();
        }
        else if (powerType == Power.Type.Mind)
        {
            Debug.Log("Used mind power");
            MindPower = 0f;
            OnMindPowerUsed?.Invoke();
        }
        BossesManager.instance.GetBoss(powerType).SpecialShrink();
    }
}