using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossesManager : MonoBehaviour
{
    public static BossesManager instance;

    public Boss mindBoss;
    public Boss bodyBoss;

    [Space(5)]
    [Header("Damage and growth")]
    [SerializeField] float projectileDamage;
    [SerializeField] float propDamage;
    public int hitGrowth;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public Boss GetBoss(Power.Type type)
    {
        if (type == Power.Type.Mind)
            return mindBoss;
        else if (type == Power.Type.Body)
            return bodyBoss;
        else 
        {
            Debug.LogError("Unkown boss type");
            return null;
        }
    }

    public float GetDamage(Throwable.Type damageType)
    {
        if (damageType == Throwable.Type.Projectile)
            return projectileDamage;
        else if (damageType == Throwable.Type.Prop)
            return propDamage;
        else 
        {
            Debug.LogError("Unkown damage type");
            return 0f;
        }
    }
}