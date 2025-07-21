using Enums;
using Pathfinding;
using System;
using UnityEngine;
using UnityEngine.Pool;

public class Companion : BaseCreature, IInteractable, IParty
{
    public CompanionController Controller => controller;
    private CompanionController controller;

    public string ID;
    private CompanionData companionData;

    private bool isRecruitable = true;

    public event Action<IInteractable> OnInteracted;
    public bool IsInteractable => isRecruitable;

    public PartySystem MyParty => myParty;
    private PartySystem myParty;
    
    private Player myLeader;

    public bool IsLeader => false;

    public PartyRole PartyRole => partyRole;
    [SerializeField] private PartyRole partyRole;

    public SearchTarget searchTarget;

    public Seeker seeker;
    public AIPath aiPath;

    public PoolingManager poolingManager;
    public Projectile projectilePrefab;
    public IObjectPool<Projectile> connectedPool;

    public Transform projectilePivot;

    private PartyManager partyManager;

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
        partyManager = PartyManager.Instance;

        companionData = DataManager.Instance.CompanionDB[ID];

        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();

        searchTarget ??= GetComponentInChildren<SearchTarget>();
        searchTarget?.Init(targetMask);

        stat = GetComponent<CompanionStat>();
        stat.Init(this);

        RegisterState();

        SetProjectile();
        ConnectParty(partyManager.playerParty);
    }

    public void TestInit()
    {
        base.Init();
        partyManager = PartyManager.Instance;

        companionData = DataManager.Instance.CompanionDB[ID];

        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();

        searchTarget ??= GetComponentInChildren<SearchTarget>();

        stat = GetComponent<CompanionStat>();
        stat.Init(this);

        RegisterState();

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
            CompanionManager.Instance.JoinParty(companionData.ID);
            gameObject.SetActive(false);
            //Logger.Log(myParty.leader);
        }
    }

    public void ConnectParty(PartySystem partySystem)
    {
        partyManager.playerParty.AddMember(this);
        myParty = partySystem;
        myLeader = myParty.leader as Player;
    }

    public CompanionStat GetStat()
    {
        return stat as CompanionStat;
    }

    public Vector2 GetPosition()
    {
        return (Vector2)transform.position;
    }

    public void SetProjectile()
    {
        if(projectilePrefab == null)
        {
            //Logger.Log("사용할 발사체가 없습니다.");
            return;
        }

        poolingManager = PoolingManager.Instance;

        //발사체 프리팹 정보를 이용하여 오브젝트 풀에 등록
        connectedPool = poolingManager.CreatePool<Projectile>(projectilePrefab.name, CreateProjectile, 10);   //풀링 매니저에서 등록된 발사체 풀을 찾아와서 핸들 매니저에 연결

        for (int i = 0; i < 3; i++)                      //사용할 오브젝트를 필요한 갯수만큼 미리 생성하여 풀링에 등록
        {
            Projectile projectiles = CreateProjectile();
            connectedPool.Release(projectiles);          //미리 만들어 놓은 오브젝트이기 때문에 회수하여 저장
        }
    }

    public Projectile CreateProjectile()
    {
        Projectile projectilePool = Instantiate(projectilePrefab,poolingManager.projectilePool);
        Logger.Log(connectedPool == null);
        projectilePool.Init(connectedPool, targetMask);     
        return projectilePool;
    }

    public void StartBattle()
    {
        gameObject.SetActive(true);
        transform.position = GetLeader().transform.position;
    }

    public void EndBattle()
    {
        gameObject.SetActive(false);
        controller.ResetState();
    }

    public Player GetLeader()
    {
        return myLeader;
    }

    public CompanionData GetData()
    {
        return companionData;
    }
}
