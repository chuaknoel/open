using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossStat : BaseStat
{
    private BossEnemy enemy;
    
    public event UnityAction OnDamaged;
    public event UnityAction OnDeath;

    public override void Init(BaseCreature owner)
    {
        base.Init(owner);
        enemy = owner as BossEnemy; // =(Enemy)owner
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
       
        if (currentHealth > 0)
        {
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
