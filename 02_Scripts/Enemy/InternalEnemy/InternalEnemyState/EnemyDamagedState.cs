using Enums;
using UnityEngine;
using static Constants.AnimatorHash;

public class EnemyDamagedState : BaseState<Enemy>
{
    protected override StateEnum stateType => StateEnum.Damaged;

    private EnemyController controller;

    private float DamagedDuration = 0.0f;
    private float DamagedToCurrentDelay = 0.5f; // 애니메이션 재생 시간

    public override void Init(Enemy owner)
    {
        base.Init(owner);

        controller = owner.Controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        //Logger.Log("Damaged Enter 진입");

        owner.ChangeAnimation(EnemyDamagedStateHash, true);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        DamagedDuration += Time.deltaTime;

        if(DamagedDuration >= DamagedToCurrentDelay)
        {
            if (controller.saveOriginState == StateEnum.Idle)
            {
                controller.ChangeState(StateEnum.Idle);
            }
            else if (controller.saveOriginState == StateEnum.Move)
            {
                controller.ChangeState(StateEnum.Move);
            }
            else if (controller.saveOriginState == StateEnum.Attack)
            {
                controller.ChangeState(StateEnum.Attack);
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

        DamagedDuration = 0.0f;
        owner.ChangeAnimation(EnemyDamagedStateHash, false);
    }
}
