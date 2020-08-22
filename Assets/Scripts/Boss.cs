using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject[] parryableObjects;
    [SerializeField] int maxFill;
    [SerializeField] int fillChangeSmoothness = 1;
    [SerializeField] float fillChangeRate = 1f;
    [SerializeField] int fillGainAmount = 1;
    [SerializeField] float fillGainRate = 0.5f;

    private Animator _animator;

    private int currentFill;
    private int CurrentFill
    {
        get => currentFill;
        set
        {
            lerpTime = 0f;
            currentFill = Mathf.Clamp(value, 0, maxFill);
            CancelInvoke(nameof(SmoothGrow));
            InvokeRepeating(nameof(SmoothGrow), 0f, fillChangeRate);
        }
    }

    private int previousFill;

    private float lerpTime = 0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        previousFill = currentFill;
        InvokeRepeating(nameof(Grow), 0f, fillGainRate);
    }

    void SmoothGrow()
    {
        previousFill = (int)Mathf.Lerp(previousFill, currentFill, lerpTime);
        _animator.SetFloat("Fill", previousFill / (float)maxFill);
        if(previousFill == currentFill)
            CancelInvoke(nameof(SmoothGrow));
        lerpTime += (float)fillChangeSmoothness / Mathf.Abs(currentFill - previousFill);
    }

    void Grow()
    {
        CurrentFill += fillGainAmount;
    }

    public void HitGrow()    
    {
        CurrentFill += BossesManager.instance.hitGrowth;
    }

    public void Shrink(Throwable.Type damageType)
    {
        CurrentFill -= BossesManager.instance.GetDamage(damageType);
    }

    public void SpecialShrink()
    {
        CurrentFill -= BossesManager.instance.specialDamage;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
            CurrentFill -= 5;
        if(Input.GetKeyDown(KeyCode.L))
            CurrentFill += 5;
    }
}
