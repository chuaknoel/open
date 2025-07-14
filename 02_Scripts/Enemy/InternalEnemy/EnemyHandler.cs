using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyHandler : MonoBehaviour
{
    public void IdleStateUpdate()
    {
        //owner.attackCoolDownTimer += Time.deltaTime;
        //if (owner.attackCoolDown <= owner.attackCoolDownTimer)
        //{
        //    owner.canAttack = true;
        //}

        //if (owner.searchTarget.GetCurrentTarget() != null)
        //{
        //    if (owner.attackTargetRange <= controller.distanceTargetToEnemy)
        //    {
        //        controller.ChangeState(StateEnum.Move);
        //    }
        //    else
        //    {
        //        if (owner.canAttack)
        //        {
        //            controller.ChangeState(StateEnum.Attack);
        //        }
        //    }
        //}
    }

    public void MoveStateUpdate()
    {
        //    if (owner.searchTarget.GetCurrentTarget() == null)
        //    {
        //        if (controller.detectionLostTimer < controller.moveToIdleDelay)
        //        {
        //            Logger.Log("적이 움직임을 멈추는 중...");
        //        }

        //        controller.detectionLostTimer += Time.deltaTime;
        //        if (controller.detectionLostTimer >= controller.moveToIdleDelay)
        //        {
        //            controller.ChangeState(StateEnum.Idle);
        //            controller.detectionLostTimer = 0.0f;
        //        }
        //    }
        //    else
        //    {
        //        if (controller.distanceTargetToEnemy <= owner.attackTargetRange)
        //        {
        //            if (!owner.canAttack)
        //            {
        //                controller.ChangeState(StateEnum.Idle);
        //            }
        //            else
        //            {
        //                controller.ChangeState(StateEnum.Attack);
        //            }
        //        }
        //    }
    }

    public void AttackStateUpdate()
    {

    }
}
