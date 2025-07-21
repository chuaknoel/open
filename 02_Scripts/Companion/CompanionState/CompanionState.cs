using Enums;
using UnityEngine;
using static Constants.AnimatorHash;

public class CompanionState : BaseState<Companion>
{
    protected CompanionController controller;
    protected SearchTarget searchTarget;
    protected PartySystem MyParty
    {
        get
        {
            if (myParty == null && owner != null)
            {
                myParty = owner.MyParty;
            }

            return myParty;
        }
    }
    private PartySystem myParty;

    protected Vector2 moveDir;        //목표지점 방향
    protected Vector2 movePoint;      //목표 지점
    protected Vector2 positionPoint;  //역할에 따라 플레이어를 기준으로 위치해야할 위치값

    protected Vector2 lookDir;        //동료가 바라볼 방향

    protected Vector2 nextPos;        //다음 프레임에 이동할 위치
    protected Vector2 ownerPos;       //자신의 위치

    protected float leaderDis;
    protected float searchRange;
  
    protected float stopDis; //목표지점과 자신의 거리가 stopDis 내면 도착했다고 판정

    protected override StateEnum stateType { get; }

    public override void Init(Companion owner)
    {
        base.Init(owner);
        controller = owner.Controller;

        searchTarget = owner.searchTarget;
        myParty = owner.MyParty;
       
        searchRange = searchTarget.searchRange;

        stopDis = Mathf.Pow(owner.GetHitBox().bounds.extents.x + 0.1f, 2);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (MyParty != null)
        {
            Vector3 dir = (owner.GetLeader().transform.position - owner.transform.position).normalized;
            float leaderBounds = (float)owner.GetLeader().GetHitBox()?.bounds.extents.x;
            Vector3 leaderPos = owner.GetLeader().transform.position - (dir * leaderBounds);
            leaderDis = (owner.transform.position - leaderPos).magnitude; //플레이어와 자신의 거리를 계산
        }

        LookRotate();
    }

    protected virtual Vector2 GetMovePoint()
    {
        return movePoint;
    }

    protected Vector2 GetRolePosition() //역할에 따라 플레이어를 기준으로 자리해야 할 위치 구하기
    {
        Vector2 leaderDir = (owner.GetLeader().Controller.LookDir().Item1);

        switch (owner.PartyRole)
        {
            case PartyRole.Tanker:                      //탱커 : 2칸 앞
                positionPoint = leaderDir * 2;
                break;
            case PartyRole.Healer:                      //힐러 : 3칸 뒤
                positionPoint = leaderDir * -2;
                break;
            case PartyRole.Dealer:                      //딜러 : 4칸 왼쪽
                positionPoint.x = -leaderDir.y;
                positionPoint.y = -leaderDir.x;
                positionPoint = positionPoint * 3;
                break;
            case PartyRole.Support:                    //서포터 : 3칸 오른쪽
                positionPoint.x = leaderDir.y;
                positionPoint.y = leaderDir.x;
                positionPoint = positionPoint * 1;
                break;
        }

        return positionPoint;
    }

    protected virtual void LookDir()
    {

    }

    protected void LookRotate()
    {
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        //Logger.Log($"{angle}");
        if (angle < 0) angle += 360;

        if (angle >= 315 || angle < 45) { owner.ChangeAnimation(LookDirHash, 0); return; }
        else if (angle >= 45 && angle < 135) { owner.ChangeAnimation(LookDirHash, 1); return; }
        else if (angle >= 135 && angle < 225) { owner.ChangeAnimation(LookDirHash, 2); return; }
        else { owner.ChangeAnimation(LookDirHash, 3); return; }
    }

    protected float UpdateMoveSpeed()
    {
        return owner.GetStat().GetTotalMoveSpeed();
    }
}
