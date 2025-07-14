using Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants.AnimatorHash;

public class PlayerAttackState : BaseState<Player>
{
    protected override StateEnum stateType => StateEnum.Attack;

    private HashSet<Collider2D> hitTarget = new HashSet<Collider2D>();
    private Collider2D[] hitTargetArray;

    private int maxHitableCount;
    private bool isAttackable;

    public override void Init(Player owner)
    {
        base.Init(owner);
        owner.playerActions.Attack.started += Attack;
        maxHitableCount = 5;
        hitTargetArray = new Collider2D[maxHitableCount];
        isAttackable = true;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        isAttackable = true;
        owner.ChangeAnimation(AttackStateHash, true);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public override void OnExit()
    {
        ResetTarget();
        owner.ChangeAnimation(AttackStateHash, false);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!owner.Controller.IsActionAble()) return;
        owner.Controller.ChangeState(stateType);
    }

    public override void OnDestory()
    {
        base.OnDestory();
        owner.playerActions.Attack.started -= Attack;
    }

    public void DrawAttackArea()
    {
        if (!isAttackable) return;

        int hitCount = Physics2D.OverlapBoxNonAlloc(
        owner.Rb.position + owner.Controller.LookDir().Item1 * owner.weaponArea/2f,
        owner.weaponArea,
        owner.Controller.LookDir().Item2,
        hitTargetArray,
        owner.targetMask);

        OnAttack(hitCount);
    }

    public void OnAttack(int hitCount)
    {
        if (hitTarget.Count >= maxHitableCount) { isAttackable = false; return; };

        for (int i = 0; i < hitCount; i++)
        {
            if (hitTarget.Add(hitTargetArray[i]))
            {
                if (hitTargetArray[i].TryGetComponent<IDamageable>(out IDamageable target))
                {
                    target.TakeDamage(owner.GetStat().GetTotalAttack() + 5);
                }
            }
        }
    }

    public void ResetTarget()
    {
        isAttackable = false;
        hitTarget.Clear();
    }
}
