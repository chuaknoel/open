using Enums;
using UnityEngine;
using static Constants.AnimatorHash;

public class BossController : BaseController<BossEnemy>
{
    public BossController(BossEnemy owner) : base(owner) { }
    private LookDirection LookDir;

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public void SetLookDir(Vector3 targetPos)
    {
        ChangeLookRotate(DetermineDir(targetPos));
    }

    public Vector2 DetermineDir(Vector3 targetPos)
    {
        Vector2 lookDir = Vector2.zero;

        Vector3 moveDir = (targetPos - owner.transform.position).normalized;

        float absX = Mathf.Abs(moveDir.x);
        float absY = Mathf.Abs(moveDir.y);

        float diff = Mathf.Abs(absX - absY);

        if (absX > absY)
        {
            if (moveDir.x > 0)
            {
                lookDir = Vector2.right;
            }
            else
            {
                lookDir = Vector2.left;
            }
        }
        else
        {
            if (moveDir.y > 0)
            {
                lookDir = Vector2.up;
            }
            else
            {
                lookDir = Vector2.down;
            }
        }

        return lookDir;
    }

    public void ChangeLookRotate(Vector2 lookDir)
    {
        if (lookDir == Vector2.right) { LookDir = LookDirection.East; }
        if (lookDir == Vector2.up) { LookDir = LookDirection.North; }
        if (lookDir == Vector2.left) { LookDir = LookDirection.West; }
        if (lookDir == Vector2.down) { LookDir = LookDirection.South; }

        owner.ChangeAnimation(LookDirHash, (float)LookDir);
    }

    public void ResetPath()
    {
        owner.seeker.StartPath(owner.transform.position, owner.transform.position);
    }
}
