using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ExternalEnemy : BaseCreature
{
    public ExternalEnemyController ExternalController => externalController;
    private ExternalEnemyController externalController;

    public EnemyEnum enemyEnum;

    public List<Vector2> targetPositionList = new List<Vector2>();
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        externalController.OnUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        externalController.OnFixedUpdate();
    }

    public override void Init()
    {
        base.Init();

        stat = GetComponent<ExternalEnemyStat>();

        RegisterState();
        stat.Init(this);
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
        externalController = null;
    }

    public ExternalEnemyStat GetStat()
    {
        return stat as ExternalEnemyStat;
    }
}
