using Enums;
using Pathfinding;
using System;
using UnityEngine;
using UnityEngine.Pool;

public class Companion : BaseCreature, IInteractable, IParty
{
    public CompanionController Controller => controller;
    private CompanionController controller;

    private bool isRecruitable = true;

    public event Action<IInteractable> OnInteracted;
    public bool IsInteractable => isRecruitable;

    public PartySystem MyParty => myParty;
    private PartySystem myParty;

    public bool IsLeader => false;

    public PartyRole PartyRole => partyRole;
    [SerializeField] private PartyRole partyRole;

    public SearchTarget searchTarget;

    public Seeker seeker;

    public PoolingManager poolingManager;
    public Projectile projectilePrefab;
    public IObjectPool<Projectile> connectedPool;

    public Transform projectilePivot;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (stat.isDeath) return;
        controller.OnUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (stat.isDeath) return;
        controller.OnFixedUpdate();
    }

    public override void Init()
    {
        base.Init();
        seeker = GetComponentInChildren<Seeker>();
        searchTarget ??= GetComponentInChildren<SearchTarget>();
       
        stat = GetComponent<CompanionStat>();
        stat.Init(this);

        RegisterState();

        foreach (CompanionAttackAnimeState attackAnimeState in animator.GetBehaviours<CompanionAttackAnimeState>())
        {
            attackAnimeState.Init(this);
        }

        foreach (CompanionDamagedAnimeState damagedAnimeState in animator.GetBehaviours<CompanionDamagedAnimeState>())
        {
            damagedAnimeState.Init(this);
        }

        SetProjectile();
    }

    public override void RegisterState()
    {
        controller = new CompanionController(this);
        controller.SetInitState(new CompanionIdleState());
        controller.RegisterState(new CompanionMoveState());
        controller.RegisterState(new CompanionChaseState());
        controller.RegisterState(new CompanionAttackState());
        controller.RegisterState(new CompanionDamagedState());
        controller.RegisterState(new CompanionDeathState());
    }

    public void SetInterface(bool active)
    {
       
    }

    public void OnInteraction()
    {
        if (isRecruitable)
        {
            isRecruitable = false;
            ConnectParty(PartyManager.Instance.playerParty);
            Logger.Log(myParty.leader);
        }
    }

    public CompanionStat GetStat()
    {
        return stat as CompanionStat;
    }

    public void ConnectParty(PartySystem partySystem)
    {
        PartyManager.Instance.playerParty.AddMember(this);
        myParty = partySystem;
    }

    public Vector2 GetPosition()
    {
        return (Vector2)transform.position;
    }

    public void SetProjectile()
    {
        if(projectilePrefab == null)
        {
            Logger.Log("사용할 발사체가 없습니다.");
            return;
        }

        poolingManager = PoolingManager.Instance;

        //발사체 프리팹 정보를 이용하여 오브젝트 풀에 등록
        poolingManager.RegisterPoolObject(projectilePrefab.name,

            new ObjectPool<Projectile>
            (
                CreateProjectile,                //발사체 오브젝트 생성 로직
                poolingManager.OnGet,            //생성된 오브젝트를 소환
                poolingManager.OnRelease,        //생성된 오브젝트 회수
                poolingManager.OnDes,            //생성된 오브젝트 파괴
                maxSize: 10                      //한번에 관리될 오브젝트 갯수
            ));

        connectedPool = poolingManager.FindPool<Projectile>(projectilePrefab.name);   //풀링 매니저에서 등록된 발사체 풀을 찾아와서 핸들 매니저에 연결

        for (int i = 0; i < 3; i++)                                       //한번에 관리될 오브젝트 개수를 풀링에 등록
        {
            Projectile projectiles = CreateProjectile();
            connectedPool.Release(projectiles);                           //미리 만들어 놓은 오브젝트이기 때문에 회수하여 저장
        }
    }

    public Projectile CreateProjectile()
    {
        Projectile projectilePool = Instantiate(projectilePrefab);
        projectilePool.Init(connectedPool, targetMask);     
        return projectilePool;
    }
}
