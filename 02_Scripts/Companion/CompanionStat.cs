using Enums;
using UnityEngine.Events;

public class CompanionStat : BaseStat, IDamageable
{
    private Companion companion;
    public event UnityAction OnDamaged;
    public event UnityAction OnDeath;

    public override void Init(BaseCreature companion)
    {
        base.Init(companion);
        this.companion = companion as Companion;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth > 0)
        {
            companion.Controller.ChangeState(StateEnum.Damaged);
            OnDamaged?.Invoke();
        }
        else if (currentHealth <= 0)
        {
            Death();
        }
    }

    public override void Death()
    {
        base.Death();
        if (!isDeath)
        {
            isDeath = true;
            companion.Controller.ChangeState(StateEnum.Death);
            OnDeath?.Invoke();
        }
    }
}
