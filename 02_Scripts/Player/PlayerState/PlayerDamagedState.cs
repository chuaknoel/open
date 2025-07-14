using Enums;
using static Constants.AnimatorHash;

public class PlayerDamagedState : BaseState<Player>
{
    protected override StateEnum stateType => StateEnum.Damaged;

    public override void Init(Player owner)
    {
        base.Init(owner);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.ChangeAnimation(DamagedStateHash, true);
    }

    public override void OnUpdate(float deltaTime)
    {
        //Logger.Log(owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if(owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            owner.Controller.ChangeState(StateEnum.Idle);
        }
    }

    public override void OnExit()
    {
        owner.ChangeAnimation(DamagedStateHash, false);
    }
}
