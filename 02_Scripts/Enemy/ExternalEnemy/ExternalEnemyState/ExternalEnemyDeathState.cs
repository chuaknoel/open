using Enums;
using UnityEngine;
using static Constants.AnimatorHash;

public class ExternalEnemyDeathState : BaseState<ExternalEnemy>
{
    protected override StateEnum stateType => StateEnum.Death;

    private ExternalEnemyController externalController;

    public override void Init(ExternalEnemy owner)
    {
        base.Init(owner);

        externalController = owner.ExternalController;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //owner.isDeath = true;
        //owner.ChangeAnimation(EnemyDeathStateHash, true);
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
        //owner.ChangeAnimation(EnemyIdleStateHash, false);
    }
}
