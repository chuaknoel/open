using Enums;
using UnityEngine;
using static Constants.AnimatorHash;

public class ExternalEnemyIdleState : BaseState<ExternalEnemy>
{
    protected override StateEnum stateType => StateEnum.Idle;

    private ExternalEnemyController externalController;

    public override void Init(ExternalEnemy owner)
    {
        base.Init(owner);

        externalController = owner.ExternalController;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        //Logger.Log("ExternalIdleState 진입");
        externalController.idleDuration = 0.0f;
        externalController.randomDurationTime = Random.Range(externalController.minDurationTime, externalController.maxDurationTime);

        owner.ChangeAnimation(EnemyIdleStateHash, true);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (externalController.idleDuration < externalController.randomDurationTime)
        {
            externalController.idleDuration += Time.deltaTime;
        }

        if (externalController.idleDuration >= externalController.randomDurationTime)
        {
            externalController.ChangeState(StateEnum.Move);
        }        
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.ChangeAnimation(EnemyIdleStateHash, false);
    }
}
