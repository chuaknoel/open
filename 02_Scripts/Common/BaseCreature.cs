using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public abstract class BaseCreature : MonoBehaviour
{
    protected BaseStat stat;

    public Animator Animator => animator;
    protected Animator animator;

    public AnimatorOverrideController overrideController;
    public Rigidbody2D Rb => rb;
    protected Rigidbody2D rb;

    protected Collider2D hitBox;

    public LayerMask targetMask;

    public virtual void Init() 
    { 
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<Collider2D>();
    }

    public virtual void RegisterState() { }

    public virtual void ChangeAnimation(int stateHash)
    {
        animator.SetTrigger(stateHash);
    }

    public virtual void ChangeAnimation(int stateHash, bool isCheck)
    {
        animator.SetBool(stateHash, isCheck);
    }

    public virtual void ChangeAnimation(int stateHash, int stateTransition)
    {
        animator.SetInteger(stateHash, stateTransition);
    }

    public virtual void ChangeAnimation(int stateHash, float stateTransition)
    {
        animator.SetFloat(stateHash, stateTransition);
    }

    public virtual void ChangeState(StateEnum state) { }

    public T GetStat<T>() where T : BaseStat
    {
        return stat as T;
    }

    public Collider2D GetHitBox()
    {
        return hitBox;
    }
}
