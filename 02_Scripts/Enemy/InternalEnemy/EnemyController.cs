using Enums;
using UnityEngine;
using static Constants.AnimatorHash;

public class EnemyController : BaseController<Enemy>
{
    public Vector2 enemyMoveDirection; // 적 오브젝트가 이동할 방향

    public float detectionLostTimer = 0.0f; // 적과 플레이어가 멀어져 적이 플레이어를 감지하지 못하게 된 시점부터의 경과 시간
    public float moveToIdleDelay = 0.8f; // Idle 상태로 돌아가기전 Delay할 시간 설정

    public Rigidbody2D rigid; // Rigidbody

    private LookDirection LookDir; // 방향 Enum 변수

    public Vector2 targetPosition; // 목표 대상의 위치값
    public Vector2 enemyPosition; // 적의 위치값

    public float distanceTargetToEnemy; // 타깃과 적 사이의 거리

    public StateEnum saveOriginState; // 이전 상태 저장

    public EnemyInteraction enemyInteraction;

    public EnemyController(Enemy owner) : base(owner)
    {
       //Logger.Log("생성자");

        rigid = owner.GetComponent<Rigidbody2D>();
        enemyInteraction = owner.GetComponentInChildren<EnemyInteraction>();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        //owner.target = owner.searchTarget.GetCurrentTarget();

        if (owner.target == null) return;

        targetPosition = (Vector2)owner.target.transform.position - (enemyMoveDirection * owner.safeDistance);

        enemyPosition = owner.transform.position;

        distanceTargetToEnemy = Vector2.Distance(targetPosition, enemyPosition);

        DetermineDir();

        if (!owner.canAttack)
        {
            owner.attackCoolDownTimer += Time.deltaTime;
            if (owner.attackCoolDown <= owner.attackCoolDownTimer)
            {
                owner.canAttack = true;
            }
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public void DetermineDir()
    {
        if (distanceTargetToEnemy <= 0.5f) return;

        if (enemyMoveDirection == Vector2.zero) return;

        float absX = Mathf.Abs(enemyMoveDirection.x);
        float absY = Mathf.Abs(enemyMoveDirection.y);

        float diff = Mathf.Abs(absX - absY);
        float threshold = 0.05f;

        if (diff < threshold) return;
        if (absX > absY)
        {
            if (enemyMoveDirection.x > 0)
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
            if (enemyMoveDirection.y > 0)
            {
                LookDir = LookDirection.North;
            }
            else
            {
                LookDir = LookDirection.South;
            }
        }
        owner.ChangeAnimation(LookDirHash, (float)LookDir);
    }   

    public (Vector2, float) CheckLookDirection()
    {
        if (LookDir == LookDirection.East) return (Vector2.right, 0);
        if (LookDir == LookDirection.North) return (Vector2.up, 90);
        if (LookDir == LookDirection.West) return (Vector2.left, 180);
        if (LookDir == LookDirection.South) return (Vector2.down, 270);

        return (Vector2.zero, 0);
    }

    public void ChangeLookRotate(Vector2 lookDir)
    {
        if (lookDir == Vector2.right) { LookDir = LookDirection.East; }
        if (lookDir == Vector2.up) { LookDir = LookDirection.North; }
        if (lookDir == Vector2.left) { LookDir = LookDirection.West; }
        if (lookDir == Vector2.down) { LookDir = LookDirection.South; }

        owner.ChangeAnimation(LookDirHash, (float)LookDir);
    }
}