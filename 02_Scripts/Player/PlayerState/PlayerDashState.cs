using Enums;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants.AnimatorHash;

public class PlayerDashState : BaseState<Player>
{
    protected override StateEnum stateType => StateEnum.Dash;

    protected Vector2 posDir;

    protected float dashPower = 10f;

    public override void Init(Player owner)
    {
        base.Init(owner);
        owner.playerActions.Dash.started += Dash;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        owner.GetStat().isInvincible = true;
        owner.ChangeAnimation(DashStateHash, true);

        if (owner.Controller.inputDir == Vector3.zero)
        {
            posDir = owner.Controller.LookDir().Item1;
        }
        else
        {
            posDir = owner.Controller.inputDir;
        }

        owner.Rb.AddForce(posDir * dashPower, ForceMode2D.Impulse);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (!owner.Controller.IsActionAble()) return;
        owner.Controller.ChangeState(stateType);
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.Rb.velocity = Vector2.zero;
        owner.ChangeAnimation(DashStateHash, false);
        owner.GetStat().isInvincible = false;
    }

    public override void OnDestory()
    {
        base.OnDestory();
        owner.playerActions.Dash.started -= Dash;
    }
}
