using Enums;
using static Constants.AnimatorHash;

public class CompanionDamagedState : CompanionState
{
    protected override StateEnum stateType => StateEnum.Damaged;

    public override void Init(Companion owner)
    {
        base.Init(owner);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        owner.ChangeAnimation(DamagedStateHash, true);
    }

    public override void OnExit()
    {
        owner.ChangeAnimation(DamagedStateHash, false);
    }
}
