using Enums;
using UnityEngine;
using static Constants.AnimatorHash;

public class PlayerIdleState : BaseState<Player>
{
    protected override StateEnum stateType => StateEnum.Idle;
    protected PlayerController controller;

    public override void Init(Player owner)
    {
        base.Init(owner);
        controller = owner.Controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.ChangeAnimation(IdleStateHash, true);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        
        if(owner.playerActions.Move.ReadValue<Vector2>().sqrMagnitude > 0)
        {
            controller.ChangeState(StateEnum.Move);
        }
    }

    public override void OnExit() 
    {
        owner.ChangeAnimation(IdleStateHash, false);
    }
}
