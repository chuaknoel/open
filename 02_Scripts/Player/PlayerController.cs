using Enums;
using UnityEngine;
using UnityEngine.Events;
using static Constants.AnimatorHash; 

public class PlayerController : BaseController<Player>
{
    public Vector3 inputDir;
    public LookDirection lookDir;
    public LookDirection currentLookDir;

    public UnityAction<Vector2, float> OnChangeDir;

    public PlayerController(Player owner) : base(owner) { }
   
    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public void DetermineDir()                          //플레이어가 바라볼 방향 결정하기
    {
        if (inputDir == Vector3.zero) return;           //멈춰있을 경우는 현재 방향 유지를 위해 return처리

        if (inputDir.x != 0 && inputDir.y == 0)                                           //인풋이 X축을 더 많이 받을때 X축 방향을 구함
        {
            currentLookDir = inputDir.x > 0 ? LookDirection.East : LookDirection.West;    //x축이 양수면 오른쪽, 음수면 왼쪽을 바라보게 한다.
        }
        else if (inputDir.x == 0 && inputDir.y != 0)                                      //인풋이 Y축을 더 많이 받을때 Y축 방향을 구함
        {
            currentLookDir = inputDir.y > 0 ? LookDirection.North : LookDirection.South;  //y축이 양수면 위쪽, 음수면 아래쪽을 바라보게 한다.
        }
        else if (inputDir.x !=0 && inputDir.y !=0)                                        //대각선 이동중일때 기존 방향과 새로운 방향이 반대 방향일떄만 방향 전환 처리를 해준다.
        {
            if (!CheckOppositeDir())
            {
                return;
            }
        }

        lookDir = currentLookDir;
        owner.Interaction?.RotateInterction(LookDir().Item2);
        owner.ChangeAnimation(LookDirHash, (int)lookDir);
    }

    private bool CheckOppositeDir()      //기존 입력과 현재 입력이 반대되는 입력인지 체크하는 함수
    {                                    //대각 입력시 y축이 우선되기 때문에 x축 입력시 방향을 직접 할당할 필요가 있음
        if (lookDir == LookDirection.East && inputDir.x < 0) { currentLookDir = LookDirection.West; return true; }   
        if (lookDir == LookDirection.West && inputDir.x > 0) { currentLookDir = LookDirection.East; return true; }
        if (lookDir == LookDirection.North && inputDir.y < 0) { currentLookDir = LookDirection.South; return true; }
        if (lookDir == LookDirection.South && inputDir.y > 0) { currentLookDir = LookDirection.North; return true; }

        return false;
    }

    public (Vector2, float) LookDir()
    {
        if (lookDir == LookDirection.East) return (Vector2.right, 0);
        if (lookDir == LookDirection.North) return (Vector2.up, 90);
        if (lookDir == LookDirection.West) return (Vector2.left, 180);
        if (lookDir == LookDirection.South) return (Vector2.down, 270);

        return (Vector2.zero, 0);
    }

    public bool IsActionAble()
    {
        switch (currentState)
        {
            case PlayerIdleState:
            case PlayerMoveState:
                return true;
            default:
                return false;
        }
    }
}

