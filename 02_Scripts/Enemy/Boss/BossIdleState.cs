using Enums;
using UnityEngine.InputSystem.XR;
using static Constants.AnimatorHash;

public class BossIdleState : BaseState<BossEnemy>
{
    protected override StateEnum stateType => StateEnum.Idle;

    public override void Init(BossEnemy owner)
    {
        base.Init(owner);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.ChangeAnimation(EnemyIdleStateHash, true);
        owner.Controller.ResetPath();

    }
    public override void OnUpdate(float deltaTime)
    {
        if(owner.searchTarget.GetCurrentTarget() != null)
        {
            owner.Controller.SetLookDir(owner.searchTarget.CurrentTargetPos());

            if(owner.attackTargetRange < owner.searchTarget.CurrentTargetDis())
            {
                owner.Controller.ChangeState(StateEnum.Move);
            }
            else
            {
                owner.Controller.ChangeState(StateEnum.Attack);
            }
        }
    }

    public override void OnExit()
    {
        owner.ChangeAnimation(EnemyIdleStateHash, false);
    }
}
