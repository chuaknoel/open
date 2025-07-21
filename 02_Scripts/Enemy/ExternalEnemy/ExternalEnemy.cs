using Enums;
using System.Collections.Generic;
using UnityEngine;

public class ExternalEnemy : BaseCreature
{
    public ExternalEnemyController ExternalController => externalController;
    private ExternalEnemyController externalController;

    private EnemyManager enemyManager;
    private BattleManager battleManager;

    public EnemyData enemy;
    public List<EnemyData> enemyGroup;

    public List<Vector2> targetPositionList = new List<Vector2>();

    public bool isDeath;
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (isDeath) return;
        externalController.OnUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (isDeath) return;
        externalController.OnFixedUpdate();
    }

    public override void Init()
    {
        base.Init();
        enemyManager = EnemyManager.Instance;
        battleManager = BattleManager.Instance;
        RegisterState();
    }

    public void ResetState()
    {

    }

    public override void RegisterState()
    {
        externalController = new ExternalEnemyController(this);
        externalController.SetInitState(new ExternalEnemyIdleState());
        externalController.RegisterState(new ExternalEnemyMoveState());
        externalController.RegisterState(new ExternalEnemyDeathState());
    }

    private void OnDestroy()
    {
        externalController.OnDestroy();
        externalController = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetMask) != 0)
        {
            //enemyManager.currentPlayerPosition = collision.transform.position;

            BattleManager.Instance.EnterBattle(this);
        }
    }
}
