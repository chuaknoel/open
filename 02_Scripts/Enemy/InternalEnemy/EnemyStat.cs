using Enums;
using UnityEngine.Events;

public class EnemyStat : BaseStat, IDamageable
{
    private Enemy enemy;
    private EnemyHealth enemyHealth;

    public event UnityAction OnDamaged;
    public event UnityAction OnDeath;

    public override void Init(BaseCreature owner)
    {
        base.Init(owner);
        enemy = owner as Enemy; // =(Enemy)owner

        enemyHealth = GetComponentInChildren<EnemyHealth>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        enemyHealth.DamagedEffect(currentHealth, maxHealth);

        if (currentHealth > 0)
        {

            enemy.Controller.ChangeState(StateEnum.Damaged);
            OnDamaged?.Invoke();
        }
        else
        {
            Death();
        }
    }
    public override void Death()
    {
        base.Death();
        isDeath = true;
        enemy.Controller.ChangeState(StateEnum.Death);
        OnDeath?.Invoke();
    }

    public void SetStats(float health, float stamina)
    {
        currentHealth = health;
    }
}
