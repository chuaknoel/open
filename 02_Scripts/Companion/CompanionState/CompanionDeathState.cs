using Enums;
using static Constants.AnimatorHash;

public class CompanionDeathState : CompanionState
{
    protected override StateEnum stateType => StateEnum.Death;

    public override void Init(Companion owner)
    {
        base.Init(owner);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.ChangeAnimation(DeathTriggerHash);
        owner.ChangeAnimation(DeathStateHash, true);
    }
}
