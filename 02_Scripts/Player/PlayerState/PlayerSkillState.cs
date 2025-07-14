using Enums;
using UnityEngine;
using System.Text;
using static Constants.AnimatorHash;

public class PlayerSkillState : BaseState<Player>
{
    protected override StateEnum stateType => StateEnum.Skill;
    protected StringBuilder animeName = new StringBuilder();

    public override void Init(Player owner)
    {
        base.Init(owner);
        animeName.Clear();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.ChangeAnimation(SkillStateHash, true);
        animeName.Append($"SkillButton{ReadSkillButtonNum()}_{ReadDirString()}");
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        AnimatorStateInfo info = owner.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(animeName.ToString()) && info.normalizedTime >= 1f)
        {
            owner.Controller.ChangeState(StateEnum.Idle);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    private int ReadSkillButtonNum()
    {
        return owner.Animator.GetInteger(SkillButtonHash);
    }

    private string ReadDirString()
    {
        string dir = "";
        switch (owner.Animator.GetInteger(LookDirHash))
        {
            case 0:
                dir = "E";
                break;

            case 1:
                dir = "N";
                break;

            case 2:
                dir = "W";
                break;

            case 3:
                dir = "S";
                break;
        }

        return dir;
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.ChangeAnimation(SkillStateHash, false);
        animeName.Clear();
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }
}
