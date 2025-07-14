using Enums;
using UnityEngine.Events;

public class ExternalEnemyStat : BaseStat, IDamageable
{
    private ExternalEnemy externalEnemy;

    public event UnityAction OnDamaged;
    public event UnityAction OnDeath;

    public override void Init(BaseCreature owner)
    {
        base.Init(owner);
        externalEnemy = owner as ExternalEnemy; // =(ExternalEnemy)owner
    }

    public void TakeDamage(float damage)
    {

    }

    public override void Death()
    {
        base.Death();
        isDeath = true;
        externalEnemy.ExternalController.ChangeState(StateEnum.Death);
    }
}