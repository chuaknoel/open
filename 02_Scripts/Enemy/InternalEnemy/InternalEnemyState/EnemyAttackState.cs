using Enums;
using Pathfinding;
using UnityEngine;
using static Constants.AnimatorHash;

public class EnemyAttackState : BaseState<Enemy>
{
    protected override StateEnum stateType => StateEnum.Attack;
    
    private EnemyController controller;

    public override void Init(Enemy owner)
    {
        base.Init(owner);

        controller = owner.Controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        //Logger.Log("Attack Enter 진입");
        owner.canAttack = true;
        controller.saveOriginState = stateType;

        RotateCollider();

        owner.ChangeAnimation(EnemyAttackStateHash, true);
        owner.GetComponent<AIPath>().canMove = false;

        controller.enemyMoveDirection = Vector2.zero;
        owner.seeker.StartPath(controller.enemyPosition, controller.enemyPosition);
        owner.seeker.CancelCurrentPathRequest();
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
        owner.ChangeAnimation(EnemyAttackStateHash, false);
        
        owner.canAttack = false;

        owner.attackCoolDownTimer = 0.0f;

        owner.GetComponent<AIPath>().canMove = true;
    }

    public void RotateCollider()
    {
        Vector2 origin = (Vector2)owner.Rb.transform.position + controller.CheckLookDirection().Item1 * owner.weaponArea.x / 2f;

        // 회전 각도
        Quaternion rotation = Quaternion.Euler(0, 0, controller.CheckLookDirection().Item2);

        //Logger.Log(controller.CheckLookDirection().Item2);
        // 적용
        owner.AttackAreaTransform.position = origin;
        owner.AttackAreaTransform.rotation = rotation;
    }
}
