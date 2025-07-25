using UnityEngine;

public class EnemyInteraction : MonoBehaviour
{
    private Enemy enemy;

    private BoxCollider2D attackCollider;

    public bool isHItInMotion = false;
  

    public void Init(Enemy enemy)
    {
        this.enemy = enemy;
        attackCollider = GetComponent<BoxCollider2D>();
    }

    public void AttackBoxToggle(bool isActive)
    {
        attackCollider.enabled = isActive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemy.targetMask) != 0)
        {
            if (enemy.canAttack && isHItInMotion)
            {
                if (Vector2.Distance(collision.transform.position, enemy.transform.position) <= enemy.attackTargetRange) 
                    // 한명만 때리기 위해 공격 범위 안에는 있지만 플레이어 감지 영역에 벗어난 플레이어를 때리지 않게 하는 조건
                {
                    if (collision.TryGetComponent<IDamageable>(out IDamageable target))
                    {
                        target.TakeDamage(enemy.GetStat().GetTotalAttack() + 10f);
                        Logger.Log("Take Damage!");
                    }
                }
            }
        }
    }
}
