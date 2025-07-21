using Enums;
using System.Collections.Generic;
using UnityEngine;
using static Constants.AnimatorHash;

public class CompanionMoveState : CompanionState
{
    protected override StateEnum stateType => StateEnum.Move;
    private List<Vector3> currentPath;

    private float updateTime;
    private float delay = .5f;

    public override void Init(Companion owner)
    {
        base.Init(owner);
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
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        GetMovePoint();                   //자신의 목표 위치 계산
        ownerPos = owner.GetPosition();   //현재 자신의 위치

        //Logger.Log($"{movePoint} : {(movePoint - ownerPos).sqrMagnitude} : {stopDis}");

        //Logger.Log(owner.aiPath.reachedEndOfPath);
        //if ((movePoint - ownerPos).sqrMagnitude < stopDis)  //목표 지점에 도달했는지 체크
        if(owner.aiPath.reachedEndOfPath)
        {
            ArriveMovePoint();
            return;
        }
        //도달하지 못했다면
        {
            MoveToMovePoint();
            return;
        }
    }

    private void ArriveMovePoint()
    {
        Player MyLeader = owner.GetLeader();
        lookDir = MyLeader ? MyLeader.Controller.LookDir().Item1 : Vector2.zero;

        if (MyLeader == null || MyLeader.Controller.inputDir == Vector3.zero) //목표 지점에 도착했고 플레이어가 움직이지 않으면 Idle로 전환
        {
            controller.ChangeState(StateEnum.Idle);
            return;
        }
        //플레이어가 계속 움직이고 있다면 목표지점이 계속해서 변한다.
        //목표지점에 가까워져서 Idle 상태가 되었다가도 다시 변한 movePoint로 인해 move 상태로 바뀐다.
        //그래서 플레이어가 움직이고 있다면 movePoint에 도달했어도 move 상태를 유지한다.
        else
        {
            //moveDir = (movePoint - ownerPos).normalized;     
            //nextPos = ownerPos + moveDir * (UpdateMoveSpeed()) * Time.fixedDeltaTime;
            //owner.Rb.MovePosition(nextPos);
            owner.aiPath.maxSpeed = UpdateMoveSpeed();
        }
    }

    private void MoveToMovePoint() 
    {
        if (owner.GetLeader() == null) return;

        Vector2 _movePoint = Vector2.zero;

        //플레이어가 이동을 멈췄고 동료가 이동 중일때 두가지 경우가 생긴다.
        //첫째, 플레이어가 방향을 180도 바꿔서 동료가 이동중일때,
        //둘째, 자신의 포지션 위치가 도달할 수 없는 구역일때.
        //if (MyLeader.Controller.inputDir == Vector3.zero)
        //{
        //    updateTime += Time.fixedDeltaTime;
        //    //플레이어가 멈춘지 delay시간 만큼 지났다면,
        //    //movePoint가 도달할 수 없는 지점으로 판단해 가장 가까운 경로를 받아와서 이동 시켜준다.
        //    if (updateTime > delay)
        //    {
        //        //owner.seeker.StartPath(ownerPos, GetMovePoint());
        //        GetMovePoint();
        //        _movePoint = currentPath.Count > 1 ? (Vector2)currentPath[1] : (Vector2)currentPath[^1];
        //        lookDir = ((Vector2)currentPath[^1] - ownerPos).normalized;
        //        //Logger.Log(lookDir);
        //    }
        //}
        //else
        //{
        //    updateTime = 0;          
        //}

        //moveDir = (_movePoint - ownerPos).normalized;                                //이동할 방향 계산
        //nextPos = ownerPos + moveDir * (UpdateMoveSpeed() + 3) * Time.fixedDeltaTime;//다음 위치 계산
        //owner.Rb.MovePosition(nextPos);

        _movePoint = movePoint;
        lookDir = owner.GetLeader() ? owner.GetLeader().Controller.LookDir().Item1 : Vector2.zero;
        owner.aiPath.maxSpeed = (UpdateMoveSpeed() + 3);
    }

    protected override Vector2 GetMovePoint()
    {
        if (owner.GetLeader() == null) return owner.transform.position;

        Vector2 leaderPos = ((Vector2)owner.GetLeader().transform.position); //플레이어의 위치 구하기
        Vector2 _movePoint = leaderPos + GetRolePosition();         //자신이 위치할 자리 구하기
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
