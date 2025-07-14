using Enums;
using UnityEngine;
using static Constants.AnimatorHash;

public class CompanionAttackState : CompanionState
{
    protected override StateEnum stateType => StateEnum.Attack;
    protected BaseCreature target;    //공격할 타켓

    public override void Init(Companion owner)
    {
        base.Init(owner);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Logger.Log(stateType);
        GetMovePoint();
        ownerPos = owner.GetPosition();
        LookDir();
        LookRotate();
        owner.seeker.CancelCurrentPathRequest();
        owner.ChangeAnimation(AttackStateHash, true);
    }

    public override void OnUpdate(float deltaTime)
    {
        //base.OnUpdate(deltaTime);      
    }

    protected override void LookDir()
    {
        Vector2 targetDir = movePoint - ownerPos;       //타겟의 방향    
        lookDir = targetDir.normalized;                 //바라볼 방향
        owner.projectilePivot.localPosition = lookDir;
    }

    protected override Vector2 GetMovePoint()
    {
        target = searchTarget.GetCurrentTarget();
        movePoint = target.transform.position;
        return movePoint;
    }

    public void Attack()
    {
        if(owner.projectilePrefab != null)
        {
            owner.connectedPool.Get().SetProjectile(owner.projectilePivot.position , searchTarget.GetCurrentTarget() , owner.GetStat().GetTotalAttack());
        }
    }

    public override void OnExit()
    {
        owner.ChangeAnimation(AttackStateHash, false);
    }
}
