using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerGaugeManager : MonoBehaviour
{
    public static PowerGaugeManager instance;

    [Header("Body Power Gauge")]
    [SerializeField] float maxBodyPower;
    [SerializeField] Image bodyPowerBar;
    private float bodyPower;

    [Space(5)]
    [Header("Mind Power Gauge")]
    [SerializeField] float maxMindPower;
    [SerializeField] Image mindPowerBar;
    private float mindPower;

    [Space(5)]
    [Header("Gauge Bonuses")]
    [SerializeField] float projectileBonus;
    [SerializeField] float propBonus;

    public enum BonusType
    {
        Projectile,
        Prop
    }

    public float BodyPower
    {
        get => bodyPower;
        set 
        {
            if (value <= 0f)
                bodyPower = 0f;
            else if (value >= maxBodyPower)
                bodyPower = maxBodyPower;
            else
                bodyPower = value;

            bodyPowerBar.fillAmount = bodyPower / maxBodyPower;
        }
    }

    public float MindPower
    {
        get => mindPower;
        set 
        {
            if (value <= 0f)
                mindPower = 0f;
            else if (value >= maxMindPower)
                mindPower = maxMindPower;
            else
                mindPower = value;

            mindPowerBar.fillAmount = mindPower / maxMindPower;
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

    public void AddGaugeBonus(Power.Type powerType, BonusType bonusType)
    {
        float bonus = bonusType == BonusType.Projectile ? projectileBonus : propBonus;

        if (powerType == Power.Type.Body)
            BodyPower += bonus;
        else if (powerType == Power.Type.Mind)
            MindPower += bonus;
    }
}