using UnityEngine.Events;

public interface IDamageable
{
    event UnityAction OnDamaged;
    event UnityAction OnDeath;
    void TakeDamage(float damage);
}
