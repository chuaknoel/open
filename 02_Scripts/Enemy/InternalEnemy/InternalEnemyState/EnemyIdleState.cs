using Enums;
using Unity.VisualScripting;
using UnityEngine;
using static Constants.AnimatorHash;

public class EnemyIdleState : BaseState<Enemy>
{
    protected override StateEnum stateType => StateEnum.Idle;

    private EnemyController controller;

    public override void Init(Enemy owner)
    {
        base.Init(owner);
        Logger.Log("Idle : Init 타이밍");
        controller = owner.Controller;

        Logger.Log("IdleState : " + owner.Controller);

    }

    public override void OnEnter()
    {
        base.OnEnter();

        Logger.Log("Idle Enter 진입");

        controller.saveOriginState = stateType;

        owner.ChangeAnimation(EnemyIdleStateHash, true);

        owner.aiPath.canMove = false;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (owner.searchTarget.GetCurrentTarget() != null)
        {
            if (owner.attackTargetRange <= controller.distanceTargetToEnemy) 
            {
                controller.ChangeState(StateEnum.Move);
            }
            else
            {
                if (owner.canAttack)
                {
                    controller.ChangeState(StateEnum.Attack);
                }   
            }
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

        owner.aiPath.canMove = true;
    }
}
