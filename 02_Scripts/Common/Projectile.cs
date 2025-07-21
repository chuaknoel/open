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

    [SerializeField] private ParticleSystem projectileParticle;
    [SerializeField] private ParticleSystem hitParticle;

    [SerializeField] private float projectileLife;
    private float releaseDelay;

    private WaitForSeconds projectileDelay;
    private WaitForSeconds releaseTime;

    private HashSet<IDamageable> targetHash = new(); 

    public void Init(IObjectPool<Projectile> objectPool, LayerMask targetMask)
    {
        this.connectedPool = objectPool;
        this.targetMask = targetMask;

        projectileDelay = new WaitForSeconds(projectileLife);

        releaseDelay = hitParticle.main.duration - projectileLife;
        releaseTime = new WaitForSeconds(releaseDelay);

        isActive = false;
    }

    public void SetProjectile(Vector2 pos , BaseCreature target , float damage)
    {
        transform.position = pos;
        this.target = target;

        targetDir = target.transform.position - transform.position;

        float angle = Mathf.Atan2(targetDir.y , targetDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        projectileParticle.gameObject.SetActive(true);
        projectileParticle.Simulate(0, true, true);
        projectileParticle.Play();

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
        targetHash.Clear();
        connectedPool.Release(this);
    }

    private IEnumerator ReleaseDelay()
    {
        //Logger.Log("Hit Enemy");
        yield return projectileDelay;

        projectileParticle.gameObject.SetActive(false);

        hitParticle.gameObject.SetActive(true);
        hitParticle.Simulate(0);
        hitParticle.Play();

        yield return releaseTime;

        hitParticle.Stop();
        hitParticle.gameObject.SetActive(false);
        ReleaseProjectile();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.transform.gameObject.layer) & targetMask) != 0)
        {
            if (collision.transform.TryGetComponent<IDamageable>(out IDamageable target))
            {
                if (targetHash.Count == 0)
                {
                    targetHash.Add(target);
                    target.TakeDamage(attackDamage);
                }
            }
            StartCoroutine(ReleaseDelay());
        }
    }
}

