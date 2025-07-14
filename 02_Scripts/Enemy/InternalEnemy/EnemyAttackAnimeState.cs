using Enums;
using UnityEngine;

public class EnemyAttackAnimeState : StateMachineBehaviour
{
    private Enemy enemy;
    private EnemyAttackState enemyAttackState;

    [SerializeField] private float exitAttackStateTime; // 초기값 1f
    [SerializeField] private float startAttackMotionTime; // 초기값 0.3f
    [SerializeField] private float endAttackMotionTime; // 초기값 0.7f
    [SerializeField] private float takeDamageTime; // 초기값 0.5f


    public void Init(Enemy enemy)
    {
        this.enemy = enemy;
        enemyAttackState = enemy.Controller.registedState[Enums.StateEnum.Attack] as EnemyAttackState;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= exitAttackStateTime)
        {
            enemy.Controller.enemyInteraction.isHItInMotion = false;
            enemy.Controller.ChangeState(Enums.StateEnum.Idle);
        }

        if (stateInfo.normalizedTime >= takeDamageTime) // 피격 타이밍 딜레이
        {
            enemy.Controller.enemyInteraction.isHItInMotion = true;
            enemy.canAttack = false;
        }

        if (stateInfo.normalizedTime >= startAttackMotionTime && stateInfo.normalizedTime <= endAttackMotionTime)
        {
            enemy.Controller.enemyInteraction.boxCollider.enabled = true;
        }
        else
        {
            enemy.Controller.enemyInteraction.boxCollider.enabled = false;
        }
    }
}
