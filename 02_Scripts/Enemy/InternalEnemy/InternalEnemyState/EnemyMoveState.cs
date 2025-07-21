using Enums;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using static Constants.AnimatorHash;

public class EnemyMoveState : BaseState<Enemy>
{
    protected override StateEnum stateType => StateEnum.Move;
    
    private EnemyController controller;

    
    public override void Init(Enemy owner)
    {
        base.Init(owner);
        controller = owner.Controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        //Logger.Log("Move Enter 진입");

        controller.saveOriginState = stateType;

        owner.ChangeAnimation(EnemyMoveStateHash, true);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (owner.searchTarget.GetCurrentTarget() == null)
        {
            if (controller.detectionLostTimer < controller.moveToIdleDelay)
            {
                //Logger.Log("적이 움직임을 멈추는 중...");
            }

            controller.detectionLostTimer += Time.deltaTime;
            if (controller.detectionLostTimer >= controller.moveToIdleDelay)
            {
                controller.ChangeState(StateEnum.Idle);
                controller.detectionLostTimer = 0.0f;
            }
        }
        else
        {
            if (controller.distanceTargetToEnemy <= owner.attackTargetRange)
            {
                if (!owner.canAttack)
                {
                    controller.ChangeState(StateEnum.Idle);
                }
                else
                {
                    controller.ChangeState(StateEnum.Attack);  
                }
            }
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        EnemyMovement();
        //if ( owner.searchTarget.GetCurrentTarget() != null || controller.detectionLostTimer < controller.moveToIdleDelay ) // 적 이동 조건 
        //    // ( 적의 플레이어 감지 거리 안 || 적이 플레이어를 감지하지 못하는 시간 )
        //    // owner.searchTarget.searchRange => Inspecter에서 설정해 줄 감지 범위 값
        //{
        //    controller.enemyMoveDirection = (controller.targetPosition - controller.enemyPosition).normalized;
        //    controller.rigid.MovePosition(controller.rigid.position + controller.enemyMoveDirection * controller.moveSpeed * Time.fixedDeltaTime);

        //}
    }

    private void EnemyMovement()
    {
        if (owner.searchTarget.GetCurrentTarget() != null || controller.detectionLostTimer < controller.moveToIdleDelay)
        {
            //float offset = 0.5f;
            //Vector2 dir = controller.targetPosition - (controller.targetPosition - controller.enemyPosition).normalized * offset;
            
            //if ( controller.distanceTargetToEnemy < 2.0f)
            //{
            //    owner.seeker.CancelCurrentPathRequest();
            //    owner.GetComponent<AIPath>().canMove = false;   
            //    return;
            //}
            //else
            //{
            //    owner.GetComponent<AIPath>().canMove = true;
            
            //}
            controller.enemyMoveDirection = (controller.targetPosition - controller.enemyPosition).normalized;

            if((controller.targetPosition - controller.enemyPosition).sqrMagnitude >= 2.0f * 2.0f)
            {
                owner.seeker.StartPath(controller.enemyPosition, controller.targetPosition);
            }
            else
            {
                //controller.ChangeState(StateEnum.Attack);
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.ChangeAnimation(EnemyMoveStateHash, false);
    }
}
