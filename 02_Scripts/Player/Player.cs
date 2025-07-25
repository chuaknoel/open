using Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Player : BaseCreature, IParty
{
    public PlayerController Controller => controller;
    private PlayerController controller;

    private PlayerInputs playerInputs;
    public PlayerInputs.PlayerActions defaultActions;
    public PlayerInputs.BattleActionActions battleActions;

    public Vector2 weaponArea;

    public Interaction Interaction => interaction;

    public PartySystem MyParty => myParty;
    private PartySystem myParty;

    public bool IsLeader => true;

    public PartyRole PartyRole => PartyRole.Tanker;

    private Interaction interaction;

    public SkillTree skillTree;
    public Dictionary<int, IObjectPool<Skill>> skillPoolDictionary = new Dictionary<int, IObjectPool<Skill>>();

    private PoolingManager poolingManager;

    public EquipmentManager equipmentManager;
    private CharacterData characterData;

    private void Update()
    {
        if (stat.isDeath) return;
        controller.OnUpdate(Time.deltaTime);
        interaction.OnUpdate();

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (stat.isInvincible) return;
            GetStat().TakeDamage(10f);
        }
    }

    private void FixedUpdate()
    {
        if (stat.isDeath) return;
        controller.OnFixedUpdate();
    }

    public override void Init()
    {
        base.Init();

        playerInputs = InputManager.Instance.inputActions;
        defaultActions = playerInputs.Player;
        battleActions = playerInputs.BattleAction;

        characterData = DataManager.Instance.defaultData.KaelData;

        interaction = GetComponentInChildren<Interaction>();
        interaction.Init(this);

        RegisterState();

        stat = GetComponent<PlayerStat>();
        stat.Init(this);

        equipmentManager ??= UIManager.Instance.equipmentManager;

        equipmentManager.equipEvent += GetStat().EquipItem;
        equipmentManager.unequipEvent += GetStat().UnequipItem;
  
        foreach (PlayerAttackAnimeState attackAnimeState in animator.GetBehaviours<PlayerAttackAnimeState>())
        {
            attackAnimeState.Init(this);
        }

        foreach (PlayerDashAnimeState attackAnimeState in animator.GetBehaviours<PlayerDashAnimeState>())
        {
            attackAnimeState.Init(this);
        }

        //weaponArea = new Vector2(1, 2);

        skillTree ??= GetComponent<SkillTree>();
        skillTree.Init(characterData.skillTree);
        SetSkillData();

        PartyManager.Instance.Init(this);
    }

    public void SetSkillData()
    {
        poolingManager = PoolingManager.Instance;

        //Logger.Log(skillTree.skillTree.Count);

        foreach (var skillData in skillTree.skillTree)
        {
            int key = skillData.Key;
            //Logger.Log(key);
            //발사체 프리팹 정보를 이용하여 오브젝트 풀에 등록
            poolingManager.RegisterPoolObject(skillData.Value.skillData.skillName,

                new ObjectPool<Skill>
                (
                    () => CreateSkill(key),                
                    poolingManager.OnGet,            
                    poolingManager.OnRelease,        
                    poolingManager.OnDes,            
                    maxSize: 1                      
                ));

            skillPoolDictionary[key] = poolingManager.FindPool<Skill>(skillData.Value.skillData.skillName);   

            for (int i = 0; i < 1; i++)                                      
            {
                Skill skill = CreateSkill(key);
                skillPoolDictionary[key].Release(skill);                          
            }
        }
    }

    public Skill CreateSkill(int skillCode)
    {
        //Logger.Log(skillCode);
        Skill skillPool = Instantiate(skillTree.skillTree[skillCode], SkillManager.Instance.playerSkill);
        skillPool.Init(skillTree.skillTree[skillCode].skillData, this, skillPoolDictionary[skillCode]);
        return skillPool;
    }

    public override void RegisterState()
    {
        controller = new PlayerController(this);
        controller.SetInitState(new PlayerIdleState());
        controller.RegisterState(new PlayerMoveState());
        controller.RegisterState(new PlayerDashState());
        controller.RegisterState(new PlayerAttackState());
        controller.RegisterState(new PlayerDamagedState());
        controller.RegisterState(new PlayerSkillState());
        controller.RegisterState(new PlayerDeathState());
    }

    public override void ChangeState(StateEnum state)
    {
        base.ChangeState(state);
        controller.ChangeState(state);
    }

    public CharacterData GetData()
    {
        return characterData;
    }

    public PlayerStat GetStat()
    {
        return stat as PlayerStat;
    }

    public void ConnectParty(PartySystem partySystem)
    {
        myParty = partySystem;
    }

    public void UnLoad()
    {
        playerInputs = null;

        interaction = null;

        controller.OnDestroy();
        controller = null;   //new 생성자로 만든 객체는 오브젝트가 파괴되어도 살아있을 수 있음. 해제 해주도록 하자.      
      
        equipmentManager.equipEvent -= GetStat().EquipItem;
        equipmentManager.unequipEvent -= GetStat().UnequipItem;

        equipmentManager = null;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 origin = rb.position + Controller.LookDir().Item1 * weaponArea.x / 2f;
        Quaternion rotation = Quaternion.Euler(0, 0, Controller.LookDir().Item2); // 회전 각도

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(origin, rotation, weaponArea);
        Gizmos.DrawWireCube(Vector3.zero, Vector2.one);
    }
}


