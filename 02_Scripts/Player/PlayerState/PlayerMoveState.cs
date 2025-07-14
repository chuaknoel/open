using Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants.AnimatorHash;

public class PlayerMoveState : BaseState<Player>
{
    protected override StateEnum stateType => StateEnum.Move;

    protected Vector2 moveDir;
    protected PlayerController playerController;

    private float lastInputTime;
    private float multiInputThreshold = 0.01f;

    private LookDirection previousLookDir;

    public override void Init(Player owner)
    {
        base.Init(owner);
        owner.playerActions.Move.performed += OnMove;
        owner.playerActions.Move.canceled += OnMoveStop;
        playerController = owner.Controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.ChangeAnimation(MoveStateHash, true);
        playerController.inputDir = owner.playerActions.Move.ReadValue<Vector2>().normalized;
        playerController.DetermineDir();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (owner.Controller.inputDir == Vector3.zero)
        {
            playerController.ChangeState(StateEnum.Idle);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        moveDir = owner.Rb.position + ((Vector2)(owner.Controller.inputDir) * UpdateMoveSpeed() * Time.fixedDeltaTime);
        owner.Rb.MovePosition(moveDir);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!playerController.IsActionAble()) return;

        lastInputTime = Time.time;
        previousLookDir = playerController.lookDir;
        playerController.inputDir = context.ReadValue<Vector2>().normalized;
        playerController.DetermineDir();
    }

    public void OnMoveStop(InputAction.CallbackContext context)
    {
        if(Time.time - lastInputTime < multiInputThreshold)
        {
            owner.ChangeAnimation(LookDirHash, (int)previousLookDir);
            playerController.lookDir = previousLookDir;
            owner.Interaction?.RotateInterction(playerController.LookDir().Item2);
        }
        playerController.inputDir = Vector3.zero;
    }

    private float UpdateMoveSpeed()
    {
        
        return 8f;
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.ChangeAnimation(MoveStateHash, false);
    }

    public override void OnDestory()
    {
        base.OnDestory();
        owner.playerActions.Move.performed -= OnMove;
        owner.playerActions.Move.canceled -= OnMoveStop;
    }
}
