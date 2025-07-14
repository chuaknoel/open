using Enums;
using System.Collections.Generic;
using UnityEngine;

public class ExternalEnemyController : BaseController<ExternalEnemy>
{
    public Vector2 externalEnemyMoveDirection; // 적 오브젝트가 이동할 방향
    private LookDirection LookDir; // 방향 Enum

    public float idleDuration = 0.0f; // 적 오브젝트가 Idle상태를 유지한 시간

    public float randomDurationTime; // Idle 상태를 유지할 시간 랜덤값
    public float minDurationTime = 1.0f; // 최솟값
    public float maxDurationTime = 1.5f; // 최댓값

    public float moveSpeed = 6.0f; // 적 오브젝트의 이동 속도

    public Rigidbody2D rigid; //Rigidbody

    public Vector2 externalEnemyPosition; // 적 오브젝트의 위치

    public ExternalEnemyController(ExternalEnemy owner) : base(owner)
    {
        rigid = owner.GetComponent<Rigidbody2D>();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        externalEnemyPosition = owner.transform.position;

        DetermineDir();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public void DetermineDir()
    {
        if (externalEnemyMoveDirection == Vector2.zero) return;

        float absX = Mathf.Abs(externalEnemyMoveDirection.x);
        float absY = Mathf.Abs(externalEnemyMoveDirection.y);

        float diff = Mathf.Abs(absX - absY);

        if (absX > absY)
        {
            if (externalEnemyMoveDirection.x > 0)
            {
                LookDir = LookDirection.East;
            }
            else
            {
                LookDir = LookDirection.West;
            }
        }
        else
        {
            if (externalEnemyMoveDirection.y > 0)
            {
                LookDir = LookDirection.North;
            }
            else
            {
                LookDir = LookDirection.South;
            }
        }


        owner.ChangeAnimation(Constants.AnimatorHash.LookDirHash, (float)LookDir);
    }   
}              
