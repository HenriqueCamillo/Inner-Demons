using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] int maxFill;
    [SerializeField] int fillChangeSmoothness = 1;
    [SerializeField] float fillChangeRate = 1f;
    [SerializeField] int fillGainAmount = 1;
    [SerializeField] float fillGainRate = 0.5f;
    [SerializeField] protected Power.Type area;
    [SerializeField] protected Transform center;

    protected Animator _animator;

    private int nextFill;
    private int NextFill
    {
        get => nextFill;
        set
        {
            previousFill = currentFill;
            lerpTime = 0f;
            nextFill = Mathf.Clamp(value, 0, maxFill);
            CancelInvoke(nameof(SmoothGrow));
            InvokeRepeating(nameof(SmoothGrow), 0f, fillChangeRate);
        }
    }

    private int previousFill, currentFill;

    private float lerpTime = 0f;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        previousFill = nextFill;
        InvokeRepeating(nameof(Grow), 0f, fillGainRate);
    }

    void SmoothGrow()
    {
        lerpTime += (float)fillChangeSmoothness / Mathf.Abs(nextFill - previousFill);
        currentFill = (int)Mathf.Lerp(previousFill, nextFill, lerpTime);
        _animator.SetFloat("Fill", currentFill / (float)maxFill);
        if(currentFill == nextFill)
        {
            CancelInvoke(nameof(SmoothGrow));
            if (currentFill == maxFill)
                GameManager.instance.GameOver();
        }
    }

    void Grow()
    {
        NextFill += fillGainAmount;
    }

    public void HitGrow()    
    {
        Debug.Log("HIt");
        NextFill += BossesManager.instance.hitGrowth;
    }

    public void Shrink(Throwable.Type damageType)
    {
        NextFill -= BossesManager.instance.GetDamage(damageType);
    }

    public void SpecialShrink()
    {
        NextFill -= BossesManager.instance.specialDamage;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
            NextFill -= 5;
        if(Input.GetKeyDown(KeyCode.L))
            NextFill += 5;
    }
}
