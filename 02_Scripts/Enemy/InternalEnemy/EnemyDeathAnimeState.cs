using Enums;
using UnityEngine;

public class EnemyDeathAnimeState : StateMachineBehaviour
{
    private Enemy enemy;
    private EnemyDeathState enemyDeathState;

    [SerializeField] private float exitDeathStateTime; //초기값 1f

    public void Init(Enemy enemy)
    {
        this.enemy = enemy;
        enemyDeathState = enemy.Controller.registedState[StateEnum.Death] as EnemyDeathState;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.TryGetComponent<Enemy>(out enemy))
        {
            enemyDeathState = enemy.Controller.registedState[StateEnum.Death] as EnemyDeathState;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= exitDeathStateTime)
        {
            enemy.Release();
        }
    }
}
