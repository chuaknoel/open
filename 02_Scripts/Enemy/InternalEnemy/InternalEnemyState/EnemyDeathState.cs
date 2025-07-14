using Enums;
using UnityEngine;
using static Constants.AnimatorHash;

public class EnemyDeathState : BaseState<Enemy>
{
    protected override StateEnum stateType => StateEnum.Death;

    private EnemyController controller;

    public override void Init(Enemy owner)
    {
        base.Init(owner);

        controller = owner.Controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Logger.Log("Death Enter 진입");
        controller.saveOriginState = stateType;

        owner.GetComponent<CircleCollider2D>().enabled = false;
        owner.ChangeAnimation(EnemyDeathStateHash, true);
    }

    public override void OnUpdate(float deltaTime)
    { 
        base.OnUpdate(deltaTime);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
