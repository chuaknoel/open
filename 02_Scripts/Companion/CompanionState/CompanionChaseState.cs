using Enums;
using System.Collections.Generic;
using UnityEngine;
using static Constants.AnimatorHash;

public class CompanionChaseState : CompanionState
{
    protected override StateEnum stateType => StateEnum.Chase;
    protected float attackRange;      //공격 사정거리
    protected BaseCreature target;    //공격할 타켓
    private float targetDis;          //타겟 과의 거리
    private List<Vector3> currentPath;

    public override void Init(Companion owner)
    {
        base.Init(owner);
        attackRange = owner.GetStat().GetAttackRange();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Logger.Log(stateType);     
        owner.ChangeAnimation(MoveStateHash, true);    
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        //Logger.Log($"{sqrLeaderDis} : {sqrSearchRange}");
        if (leaderDis > searchRange)
        {
            controller.ChangeState(StateEnum.Move);
            return;
        }

        if (searchTarget.GetCurrentTarget() == null)
        {
            controller.ChangeState(StateEnum.Idle);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        GetMovePoint();                     //적 위치 탐색
        ownerPos = owner.GetPosition();     //현재 자신의 위치

        LookDir();

        if (targetDis <= attackRange)                                            //적과의 거리가 어택범위 내면 공격 실행
        {
            controller.ChangeState(StateEnum.Attack);
            return;
        }

        nextPos = ownerPos + moveDir * UpdateMoveSpeed() * Time.fixedDeltaTime;  //다음 위치 계산
        owner.Rb.MovePosition(nextPos);                                          //실제 위치 이동
    }

    protected override void LookDir()
    {
        Vector2 targetDir = movePoint - ownerPos; //타겟의 방향
        
        targetDis = targetDir.magnitude;          //타겟과의 거리(보정 적용)

        moveDir = targetDir.normalized;           //이동할 방향 계산
        lookDir = moveDir;                        //바라볼 방향
    }

    protected override Vector2 GetMovePoint()
    {
        target = searchTarget.GetCurrentTarget();
        Vector2 _movePoint = searchTarget.CurrentTargetPos();
       
       owner.seeker.StartPath(ownerPos, _movePoint, path =>
        {
            if (!path.error)
            {
                currentPath = path.vectorPath;
                movePoint = currentPath[^1];
                //Logger.Log($"{currentPath.Count} : {currentPath[1]}");
            }
        });
        return movePoint;
    }

    public override void OnExit()
    {
        owner.ChangeAnimation(MoveStateHash, false);
    }
}
