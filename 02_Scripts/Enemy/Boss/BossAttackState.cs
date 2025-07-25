using Enums;
using Pathfinding;
using UnityEngine;
using static Constants.AnimatorHash;

public class BossAttackState : BaseState<BossEnemy>
{
    protected override StateEnum stateType => StateEnum.Attack;

    public override void Init(BossEnemy owner)
    {
        base.Init(owner);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.ChangeAnimation(EnemyAttackStateHash, true);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.ChangeAnimation(EnemyAttackStateHash, false);
    }
}
