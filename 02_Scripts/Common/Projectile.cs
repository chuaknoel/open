using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public float moveSpeed;
    public float attackDamage;
    public LayerMask targetMask;
    public BaseCreature target;

    private Vector3 targetDir;

    private bool isActive = false;
    private float currentDuration;
    [SerializeField] private float duration;

    private IObjectPool<Projectile> connectedPool;

    public void Init(IObjectPool<Projectile> objectPool, LayerMask targetMask)
    {
        this.connectedPool = objectPool;
        this.targetMask = targetMask;

        isActive = false;
    }

    public void SetProjectile(Vector2 pos , BaseCreature target , float damage)
    {
        transform.position = pos;
        this.target = target;

        targetDir = target.transform.position - transform.position;

        float angle = Mathf.Atan2(targetDir.y , targetDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        attackDamage = damage;

        currentDuration = 0;
        isActive = true;
    }

    private void Update()
    {
        if (!isActive) return;

        currentDuration += Time.deltaTime;

        if (currentDuration >= duration || target == null)
        {
            ReleaseProjectile();
            return;
        }

        targetDir = (target.transform.position - transform.position).normalized;
        transform.position += targetDir * moveSpeed * Time.deltaTime;
    }

    private void ReleaseProjectile()
    {
        connectedPool.Release(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.transform.gameObject.layer) & targetMask) != 0)
        {
            if (collision.transform.TryGetComponent<IDamageable>(out IDamageable target))
            {
                //target.TakeDamage(attackDamage);
            }
            //ReleaseProjectile();
        }
    }
}

