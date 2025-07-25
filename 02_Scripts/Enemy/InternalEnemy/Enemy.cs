using Enums;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Enemy : BaseCreature
{
    public EnemyController Controller => controller;
    protected EnemyController controller;

    //public EnemyEnum enemyEnum;

    public BaseCreature target;

    public SearchTarget searchTarget;

    public bool canAttack = false;

    public float safeDistance; //적 오브젝트와 타깃 오브젝트가 유지할 거리

    public float attackCoolDownTimer; // 적의 공격 쿨타임 재기
    public float attackCoolDown = 2.0f; //적의 공격 쿨타임

    public float attackTargetRange = 2.0f; // 플레이어를 공격하게 하기 위한 영역 ,검은색 영역

    public Vector2 weaponArea;

    public Transform AttackAreaTransform;

    public Seeker seeker;
    public AIPath aiPath;

    public IObjectPool<Enemy> connectPool;

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

        attackCoolDownTimer = attackCoolDown;

        stat = GetComponent<EnemyStat>();

        searchTarget ??= GetComponentInChildren<SearchTarget>();
        searchTarget?.Init(targetMask);

        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();

        RegisterState();
        stat.Init(this);

        foreach (EnemyAttackAnimeState attackAnimeState in animator.GetBehaviours<EnemyAttackAnimeState>())
        {
            attackAnimeState.Init(this);
        }

        foreach(EnemyDeathAnimeState deathAnimeState in animator.GetBehaviours<EnemyDeathAnimeState>())
        {
            deathAnimeState.Init(this);
        }
    }

    public override void RegisterState()
    {
        controller = new EnemyController(this);
        //Logger.Log("에너미 생성");
        controller.SetInitState(new EnemyIdleState());
        controller.RegisterState(new EnemyMoveState());
        controller.RegisterState(new EnemyDamagedState());
        controller.RegisterState(new EnemyAttackState());
        controller.RegisterState(new EnemyDeathState());
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
        controller.ChangeLookRotate(lookDir);
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

//private void OnDrawGizmos()
//{
//    if (!Application.isPlaying) return;

//    Vector2 origin = (Vector2)rb.transform.position + Controller.CheckLookDirection().Item1 * weaponArea.x / 2f;
//    Quaternion rotation = Quaternion.Euler(0, 0, Controller.CheckLookDirection().Item2);

//    Gizmos.color = Color.yellow;
//    Gizmos.matrix = Matrix4x4.TRS(origin, rotation, weaponArea);
//    Gizmos.DrawWireCube(Vector3.zero, Vector2.one);

//    float radius = attackTargetRange;
//    Color gizmoColor = Color.black;

//    Gizmos.color = gizmoColor;
//    Gizmos.matrix = Matrix4x4.identity;
//    Gizmos.DrawWireSphere(transform.position, radius);
//}