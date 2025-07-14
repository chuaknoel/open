using Enums;
using UnityEngine;

public class PlayerAttackAnimeState : StateMachineBehaviour
{
    private Player player;
    private PlayerAttackState playerAttackState;

    public void Init(Player player)
    {
        this.player = player;
        playerAttackState = player.Controller.registedState[StateEnum.Attack] as PlayerAttackState;
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= 1f)
        {
            player.Controller.ChangeState(StateEnum.Idle);
        }

        if(stateInfo.normalizedTime >= 0.3f && stateInfo.normalizedTime <= 0.7f)
        {
            playerAttackState.DrawAttackArea();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    player.Controller.ChangeState(PlayerStateEnum.Idle);
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
