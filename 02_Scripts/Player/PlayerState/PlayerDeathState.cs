using Enums;
using System;
using static Constants.AnimatorHash;

public class PlayerDeathState : BaseState<Player>
{
    protected override StateEnum stateType => StateEnum.Death;
    //private SceneLoader loader;

    public override void Init(Player owner)
    {
        base.Init(owner);
        //loader = GameManager.Instance.sceneLoader;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.ChangeAnimation(DeathTriggerHash);
        owner.ChangeAnimation(DeathStateHash, true);
        owner.playerActions.Disable();
        BattleManager.Instance.PlayerDeath();
        //loader.MoveScene("GameOverScene");
    }
}
