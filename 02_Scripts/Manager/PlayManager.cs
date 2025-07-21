using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Logger.LogError($"[YourManager] Duplicate instance detected on '{gameObject.name}'. " +
                        $"This may indicate a missing unload or unintended duplicate. Destroying this instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public SkillManager skillManager { get; private set; }
    public PoolingManager poolingManager { get; private set; }
    public PartyManager partyManager { get; private set; }
    public EnemyManager enemyManager { get; private set; }
    public CompanionManager companionManager { get; private set; }
    public BattleManager battleManager { get; private set; }

    public UIManager uiManager { get; private set; }

    public Player player;
    public void Init()
    {
        SetComponents();

        InitComponents();
        
    }

    private void SetComponents()
    {
        skillManager = SkillManager.Instance;
        poolingManager = PoolingManager.Instance;
        partyManager = PartyManager.Instance;

        enemyManager = GetComponentInChildren<EnemyManager>();
        companionManager = CompanionManager.Instance;
        
        uiManager = UIManager.Instance;
        battleManager = GetComponentInChildren<BattleManager>();

        player = FindObjectOfType<Player>();
    }

    private void InitComponents()
    {
        skillManager.Init();
        //equipmentManager.Init();
        enemyManager.Init();

        uiManager?.Init();
        battleManager.Init();

        player.Init();
        companionManager?.Init();
    }

    public void UnLoad()
    {
        if (Instance == this)
        {
            skillManager?.UnLoad();
            poolingManager?.UnLoad();
            partyManager?.UnLoad();
            enemyManager?.UnLoad();
            uiManager?.UnLoad();
            player?.UnLoad();
            battleManager?.UnLoad();
            Instance = null;
        }
        else if (Instance == null)
        {
            Logger.LogError($"[YourManager] UnLoad called, but Instance was already null. Possible duplicate unload or uninitialized state.");
        }
        else
        {
            Logger.LogError($"[YourManager] UnLoad called by a non-instance object: {gameObject.name}. Current Instance is on {Instance.gameObject.name}");
        }
    }
}
