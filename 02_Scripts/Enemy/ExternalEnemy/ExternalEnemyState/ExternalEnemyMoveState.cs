using Enums;
using System.Collections.Generic;
using UnityEngine;
using static Constants.AnimatorHash;

public class ExternalEnemyMoveState : BaseState<ExternalEnemy>
{
    protected override StateEnum stateType => StateEnum.Move;

    private ExternalEnemyController externalController;

    private bool isSuccessSetTargetPosition; // 목표 지점 설정 성공 유무를 판단해주는 변수. 41번 줄에서 사용

    private List<Vector2> targetPositionList; // 목표 지점 리스트

    private int currentTargetPositionNumber = -1; // 리스트 안 현재 목표 지점 번호
    private int lastTargetPositionNumber; // 리스트 안 이전 목표 지점 번호

    private Vector2 fixedTargetPosition; // 이동할 목표 지점
    
    public override void Init(ExternalEnemy owner)
    {
        base.Init(owner);

        externalController = owner.ExternalController;
        targetPositionList = owner.targetPositionList;
        Logger.Log(owner.targetPositionList);
    }
    
    public override void OnEnter()
    {
        base.OnEnter();

        //Logger.Log("ExternalMoveState 진입");
        owner.ChangeAnimation(EnemyMoveStateHash, true);
        isSuccessSetTargetPosition = false;

        if (currentTargetPositionNumber != -1) lastTargetPositionNumber = currentTargetPositionNumber;


        while (!isSuccessSetTargetPosition)
        {
            currentTargetPositionNumber = Random.Range(0, targetPositionList.Count);
            if (currentTargetPositionNumber != lastTargetPositionNumber)
            {
                isSuccessSetTargetPosition = true;
            }    
        }

        for (int i = 0; i < targetPositionList.Count; i++)
        {
            if ( i == currentTargetPositionNumber)
            {
                fixedTargetPosition = owner.targetPositionList[i];
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (Vector2.Distance(externalController.externalEnemyPosition, fixedTargetPosition) < 0.1f)
        {
            externalController.ChangeState(StateEnum.Idle);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        
        externalController.externalEnemyMoveDirection = (fixedTargetPosition - externalController.externalEnemyPosition).normalized;
        externalController.rigid.MovePosition(externalController.rigid.position + externalController.externalEnemyMoveDirection * externalController.moveSpeed * Time.fixedDeltaTime);
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.ChangeAnimation(EnemyMoveStateHash, false);
    }
}