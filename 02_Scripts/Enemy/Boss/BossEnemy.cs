using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BossEnemy : BaseCreature
{
    public BossController Controller => controller;
    private BossController controller;

    //public EnemyEnum enemyEnum;

    public BaseCreature target;

    public SearchTarget searchTarget;

    public float safeDistance; //적 오브젝트와 타깃 오브젝트가 유지할 거리

    public float attackTargetRange = 2.0f;

    public Vector2 weaponArea;

    public Transform AttackAreaTransform;

    public Seeker seeker;
    public AIPath aiPath;

    public IObjectPool<BossEnemy> connectPool;

    private void Update()
    {
        if (stat.isDeath) return;
        controller.OnUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        controller.OnFixedUpdate();
    }

    public override void Init()
    {
        base.Init();

        stat = GetComponent<BossStat>();

        searchTarget ??= GetComponentInChildren<SearchTarget>();
        searchTarget?.Init(targetMask);

        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();

        RegisterState();
        stat.Init(this);
    }

    public override void RegisterState()
    {
        controller = new BossController(this);
    }

    private void OnDestroy()
    {
        controller = null;
    }

    public EnemyStat GetStat()
    {
        return stat as EnemyStat;
    }

    public void SetBattle(Vector2 lookDir)
    {
        GetStat().ResetStat();
    }

    public void EndBattle()
    {
        if (gameObject.activeSelf) { Release(); return; }
    }

    public void Release()
    {
        connectPool.Release(this);
        controller.ResetState();
    }
}
