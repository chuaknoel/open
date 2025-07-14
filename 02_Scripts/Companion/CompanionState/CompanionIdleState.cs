using Enums;
using UnityEngine;
using static Constants.AnimatorHash;

public class CompanionIdleState : CompanionState
{
    protected override StateEnum stateType => StateEnum.Idle;

    public override void Init(Companion owner)
    {
        base.Init(owner);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Logger.Log(stateType);
        ownerPos = owner.GetPosition();
        GetMovePoint();
        owner.ChangeAnimation(IdleStateHash, true);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (leaderDis > searchRange)                         //리더와 너무 멀어졌다면 리더에게 돌아감.
        {
            controller.ChangeState(StateEnum.Move);
            return;
        }
        else
        {
            if (searchTarget.GetCurrentTarget() != null)     //리더가 범위 내에 있고 적이 있을때 행동 
            {
                InRangeTarget();
                return;
            }

            if ((movePoint - ownerPos).sqrMagnitude > stopDis)  //리더가 범위 내에 있고 적이 없을때 자신이 위치해야 될 포지션으로 이동
            {
                //Logger.Log($"{(movePoint - ownerPos).sqrMagnitude} : {stopDis}");
                controller.ChangeState(StateEnum.Move);
                return;
            }
        }

        lookDir = MyLeader ? MyLeader.Controller.LookDir().Item1 : Vector2.zero;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        ownerPos = owner.GetPosition();
        GetMovePoint(); 
    }

    protected override Vector2 GetMovePoint()
    {
        if (MyLeader == null)
        {
            movePoint = ownerPos;
            return movePoint;
        }
        Vector2 leaderPos = ((Vector2)MyLeader.transform.position); //플레이어의 위치 구하기
        movePoint = leaderPos + GetRolePosition();                  //자신이 위치할 자리 구하기
        return movePoint;
    }

    public override void OnExit()
    {
        owner.ChangeAnimation(IdleStateHash, false);
    }

    private void InRangeTarget()
    {
        Vector2 leaderPos = ((Vector2)MyLeader.transform.position);               //리더 위치 구하기

        float moveDis = (leaderPos - searchTarget.CurrentTargetPos()).magnitude;  //추적할 타겟의 위치
        //Logger.Log(moveDis);
        if (moveDis > searchTarget.searchRange)                                   //추적 위치에 도달했을때 리더가 탐지 범위 밖인지 확인
        {
            if ((movePoint - ownerPos).sqrMagnitude > stopDis)                    //자신의 포지션 위치가 아니면 포지션 위치를 찾기 위해 움직임         
            {
                controller.ChangeState(StateEnum.Move);
            }
            else
            {
                lookDir = MyLeader ? MyLeader.Controller.LookDir().Item1 : Vector2.zero;
            }
        }
        else                                                                      //타겟을 추적했을때 범위 안에 플레이어가 있다면 타격 추적 실행
        {
            //Logger.Log(searchTarget.GetCurrentTarget());
            controller.ChangeState(StateEnum.Chase);
        }
    }
}
